using Data.DTOModels;
using Data.Models;
using Google.OrTools.LinearSolver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchedulePlanner
{
    public class ScheduleSolver
    {
        private List<Variable[,]> _togetherList;
        private int _revertByFactor;
        private readonly IEnumerable<Shift> _shifts;
        private readonly IEnumerable<Barista> _preferences;
        private readonly Tuple<int[], int[][]> _additionalInfo;
        private readonly int[][] _dislikes;
        private List<int>[] _likeMap;

        public ScheduleSolver(IEnumerable<Shift> shifts, IEnumerable<Barista> preferences,
            Tuple<int[], int[][]> additionalInfo, int[][] dislikes)
        {
            _shifts = shifts;
            _preferences = preferences;
            _additionalInfo = additionalInfo;
            _dislikes = dislikes;
            _likeMap = new List<int>[preferences.ToList().Count];
        }

        public IEnumerable<AssignmentDTO> Solve()
        {
            var solver = new Solver("AnalogSchedulePlanner", Solver.CBC_MIXED_INTEGER_PROGRAMMING);

            _revertByFactor = _preferences.SelectMany(p => p.Preferences.Select(pr => pr.Priority)).Distinct().Count();
            var wantShifts = _additionalInfo.Item1;
            var likes = _additionalInfo.Item2;

            var pref = new int[_preferences.ToList().Count, _shifts.ToList().Count];
            foreach(var barista in _preferences)
            {
                foreach(var shift in _shifts)
                {
                    var p = barista.Preferences.FirstOrDefault(s => s.ScheduledShiftId == shift.Id);
                    if (p != null)
                    {
                        pref[barista.IndexId, shift.IndexId] = revertInt(p.Priority, _revertByFactor);
                    }
                    else
                    {
                        pref[barista.IndexId, shift.IndexId] = 0;
                    }
                }
            }

            var requiredEmployees = _shifts.Select(s => s.MaxOnShift).ToArray();

            var employeeMatrix = solver.MakeIntVarMatrix(_shifts.ToList().Count, _preferences.ToList().Count, 0, 1, "employeeShifts");

            var maxLikes = (from like in likes where like != null select like.Count(l => l > 0)).Concat(new[] { 0 }).Max(); // gets the largest amount of likes a barista have

            _togetherList = new List<Variable[,]>();
            for (var i = 0; i < _shifts.ToList().Count; i++)
            {
                _togetherList.Add(solver.MakeIntVarMatrix(_preferences.ToList().Count, maxLikes, 0, 1, $"employeeWorkTogether_{i}"));
            }

            var cons = new List<Constraint>();

            // Constraint 1: All shifts doesn't have more than their "Maximum coverage constraint"
            for (var j = 0; j < _shifts.ToList().Count; j++)
            {
                cons.Add(solver.Add(_preferences.Select((e, index) => employeeMatrix[j, index]).ToArray().Sum() <= requiredEmployees[j]));
                for (var i = 0; i < _preferences.ToList().Count; i++)
                {
                    for (var k = 0; k < _preferences.ToList().Count; k++)
                    {
                        if (i == k) continue;
                        if (likes[i] == null || likes[i][k] == 0) continue;
                        AddLikedEmployee(i, k);
                        cons.Add(solver.Add(_togetherList[j][i, GetEmployeeIndex(i, k)] <= employeeMatrix[j, i]));
                        cons.Add(solver.Add(_togetherList[j][i, GetEmployeeIndex(i, k)] <= employeeMatrix[j, k]));
                    }
                }
            }
            for (var i = 0; i < _preferences.ToList().Count; i++)
            {
                for (var j = 0; j < _shifts.ToList().Count; j++)
                {
                    //Constraint 2: All baristas only gets shifts they prefer
                    cons.Add(solver.Add(employeeMatrix[j, i] <= pref[i, j]));
                }

                // Constraint 3: All baristas has at least 1 shift
                cons.Add(solver.Add(_shifts.Select((s, index) => employeeMatrix[index, i]).ToArray().Sum() >= 1));

                // Constraint 4: All bariastas can have up to their limit of shifts they want
                cons.Add(solver.Add(_shifts.Select((s, index) => employeeMatrix[index, i]).ToArray().Sum() <= wantShifts[i]));
            }

            //Constraint 5: All baristas is not with a barista they don't like
            for (var i = 0; i < _preferences.ToList().Count; i++)
            {
                for (var k = 0; k < _preferences.ToList().Count; k++)
                {
                    if (i == k) continue;
                    if (_dislikes[i] == null || _dislikes[i][k] == 0) continue;
                    for (var s = 0; s < _shifts.ToList().Count; s++)
                    {
                        solver.Add(employeeMatrix[s, i] + employeeMatrix[s, k] <= 1);
                    }
                }
            }


            // Objective
            solver.Maximize(GetOptimization(employeeMatrix, pref).ToArray().Sum());

            var status = solver.Solve();

            var schedule = new List<AssignmentDTO>();

            if (status != Solver.OPTIMAL)
            {
                Console.WriteLine("PROTOTYPE 4: The problem don't have an optimal solution.");
                return schedule;
            }

            for (var i = 0; i < _shifts.ToList().Count; i++)
            {
                for (var j = 0; j < _preferences.ToList().Count; j++)
                {
                    if (employeeMatrix[i, j].SolutionValue() == 0.0) continue;
                    schedule.Add(new AssignmentDTO { ShiftId = _shifts.ElementAt(i).Id, BaristaId = _preferences.ElementAt(j).Id });
                }
            }
            return schedule;
        }

        private static int revertInt(int i, int max)
        {
            return i % max + 1;
        }

        private int GetEmployeeIndex(int employee, int likedEmployee)
        {
            if (_likeMap[employee] == null) return 0;
            var foundId = _likeMap[employee].IndexOf(likedEmployee);
            return foundId == -1 ? 0 : foundId;
        }

        private void AddLikedEmployee(int employee, int likedEmployee)
        {
            if (_likeMap[employee] == null) _likeMap[employee] = new List<int>();
            if (_likeMap[employee].Contains(likedEmployee)) return;
            _likeMap[employee].Add(likedEmployee);
        }

        private IEnumerable<LinearExpr> GetOptimization(Variable[,] employeeMatrix, int[,] pref)
        {
            var likes = _additionalInfo.Item2;
            var employees = _preferences.Select(s => s.IndexId).ToList();
            for (var i = 0; i < employees.Count; i++)
            {
                for (var k = 0; k < employees.Count; k++)
                {
                    if (i == k) continue;

                    for (var j = 0; j < _shifts.ToList().Count; j++)
                    {
                        if (likes[i] != null && likes[i][k] != 0)
                        {
                            yield return employeeMatrix[j, i] * pref[i, j] + _togetherList[j][i, GetEmployeeIndex(i, k)] * likes[i][k];
                        }
                        else
                        {
                            yield return employeeMatrix[j, i] * pref[i, j];
                        }
                    }
                }
            }
        }
    }
}
