using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Helpers;
using System.Diagnostics;
using WebApi.Models.Allocations;
using WebApi.Models.Moment;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class MomentController : ControllerBase
    {
        private IMomentService _momentService;
        private IRegisterService _registerService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public MomentController(
            IMomentService momentService,
            IRegisterService registerService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _momentService = momentService;
            _registerService = registerService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize(Roles = Role.AdminOrUser)]
        [HttpPost("/v1/batidas")]
        public IActionResult Create([FromBody] CreateMomentModel model)
        {
            // map model to entity
            var moment = _mapper.Map<Moment>(model);
            var registers = _registerService.GetAll();

            try
            {
                _momentService.Create(moment, registers.ToList());
                return Ok("Created");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}

