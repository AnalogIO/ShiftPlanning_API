using System;

namespace ShiftPlanning.WebApi.Helpers
{
    public static class Toolbox
    {
        public static DateTime RoundUp(DateTime dt)
        {
            if (dt.Minute < 30)
            {
                return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
            }
            else
            {
                return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0);
            }

            //return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks); //old implementation
        }
    }
}