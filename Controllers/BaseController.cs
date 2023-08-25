using Microsoft.AspNetCore.Mvc;
using LeaveManagmentSystemAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagmentSystemAPI.Controllers
{
  
    [TypeFilter(typeof(LogActionFilter))]
    public class BaseController : ControllerBase  
    {
        
        
    }
}
