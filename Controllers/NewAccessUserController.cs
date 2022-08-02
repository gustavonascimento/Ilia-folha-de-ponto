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
using WebApi.Models.NewAccess;
using System.Text.RegularExpressions;
using WebApi.Services.EmailService;

namespace WebApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NewAccessUserController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private INewAccessUserService _newAccessUserService;
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public NewAccessUserController(
            IEmailSender emailSender,
            INewAccessUserService newAccessUserService,
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _emailSender = emailSender;
            _newAccessUserService = newAccessUserService;
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _newAccessUserService.GetAll();
            var model = _mapper.Map<IList<NewAccessUsersDto>>(users);
            return Ok(model);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("acceptUser")]
        public IActionResult CreateAccessUser([FromBody] AcceptUserModel usuario)
        {
            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);
            if (usuario.Id != currentUserId && !User.IsInRole(Role.Admin))
                return Forbid();

            var user = _newAccessUserService.GetById(usuario.Id);
            var password = CreatePassword(8);

            string body = "<b>Olá,</b>" +
                          "<br/>" +
                          "<br/>" +
                          "Parabéns, o seu cadastro foi aceito pelo nosso Administrador! Para fazer o login, acesse o link abaixo com o seu endereço de e-mail cadastrado e senha." +
                          "<br/>" +
                          "<br/>" +
                          "<b>Portal do cliente:</b>" +
                          "<br/>" +
                          "<b>Endereço de e-mail:</b> " + user.Email +
                          "<br/>" +
                          "<b>Senha:</b> " + password +
                          "<br/>" +
                          "<br/>" +
                          "Agradecemos por utilizar nosso portal. Ele terá atualizações mensais, para ficar do jeito que <b>você</b> quer!" +
                          "<br/>" +
                          "<br/>" +
                          "Qualquer dúvida ou sugestão, não hesite em me chamar por e-mail, telefone ou WhatsApp." +
                          "<br/>" +
                          "<br/>" +
                          "Abraço," +
                          "<br/>" +
                          "<br/>" +
                          "<b></b>" +
                          "<br/>" +
                          "<b></b>";

            if (user == null)
                return NotFound();

            User model = new User()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Role = "User",
                Cpf = user.Cpf
            };

            if (usuario.Aceite == true)
            {
                try
                {
                    // create user
                    _userService.Create(model, password);
                    _emailSender.SendEmailAsync(model.Email, "Portal do Cliente - Senha de Acesso", body);
                    _newAccessUserService.Delete(usuario.Id);
                    return Ok("Usuário criado com sucesso");
                }
                catch (AppException ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }
            }
            else
            {
                return BadRequest("User not accepted");
            }
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _newAccessUserService.Delete(id);
            return Ok();
        }

    }
}