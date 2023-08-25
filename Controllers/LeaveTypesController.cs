using AutoMapper;
using Infrastructure.Contracts;
using LeaveManagmentSystemAPI.Data;
using LeaveManagmentSystemAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagmentSystemAPI.Controllers
{
     [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : BaseController
    {
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var leavetypes = _repo.FindAllUsingStoreProc().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            return Ok(model);
        }
    }
}
