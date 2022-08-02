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

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class AllocationController : ControllerBase
    {
        private IAllocationService _allocationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AllocationController(
            IAllocationService allocationService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _allocationService = allocationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize(Roles = Role.AdminOrUser)]
        [HttpPost("/v1/alocacoes")]
        public IActionResult Create([FromBody] CreateAllocationModel model)
        {
            // map model to entity
            var allocation = _mapper.Map<Allocation>(model);

            try
            {
                _allocationService.Create(allocation);
                return Ok("Horas alocadas ao projeto");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}

