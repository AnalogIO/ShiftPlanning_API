using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class ScheduleMapper : IMapper<Data.Models.Schedule, Schedule>
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Data.Models.Institution, Institution> _institutionMapper;
        private readonly IMapper<Data.Models.ScheduledShift, ScheduledShift> _scheduledShiftMapper;
        private readonly IMapMany<ScheduledShift, Data.Models.ScheduledShift> _scheduledShiftMapMany; 

        public ScheduleMapper(IShiftPlannerDataContext context,
            IMapper<Data.Models.Institution, Institution> institutionMapper,
            IMapper<Data.Models.ScheduledShift, ScheduledShift> scheduledShiftMapper,
            IMapMany<ScheduledShift, Data.Models.ScheduledShift> scheduledShiftMapMany)
        {
            _context = context;
            _institutionMapper = institutionMapper;
            _scheduledShiftMapper = scheduledShiftMapper;
            _scheduledShiftMapMany = scheduledShiftMapMany;
        }

        public Schedule MapToEntity(Data.Models.Schedule model)
        {
            return new Schedule
            {
                Name = model.Name,
                NumberOfWeeks = model.NumberOfWeeks,
                Institution = _context.Institutions.Single(i => i.Id == model.Institution.Id),
                Shifts = model.Shifts.Select(_scheduledShiftMapper.MapToEntity).ToList()
            };
        }

        public Data.Models.Schedule MapToModel(Schedule entity)
        {
            return new Data.Models.Schedule()
            {
                Id = entity.Id,
                Name = entity.Name,
                NumberOfWeeks = entity.NumberOfWeeks,
                Shifts = _scheduledShiftMapMany.Map(entity.Shifts).ToList(),
                Institution = _institutionMapper.MapToModel(entity.Institution)
            };
        }
    }
}
