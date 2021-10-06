using System;
using ShiftPlanning.DTOs.Employee;

namespace ShiftPlanning.Shifty.States
{
    public class LoginState
    {
        private EmployeeLoginResponse _userLogin;

        public EmployeeLoginResponse UserLogin
        {
            get => _userLogin;
            set
            {
                _userLogin = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}