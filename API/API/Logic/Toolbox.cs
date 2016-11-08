using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Logic
{
    public static class Toolbox
    {
        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }
}