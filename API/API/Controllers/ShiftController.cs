using API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage shifts
    /// </summary>
    [RoutePrefix("api/shifts")]
    public class ShiftController : ApiController
    {
        private IShiftRepository _shiftRepository;
        public ShiftController()
        {
            var context = new ShiftPlannerDataContext();
            _shiftRepository = new ShiftRepository(context);
        }



    }
}