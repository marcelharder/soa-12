using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    public class AccountController : BaseApiController
    {

        private readonly ITokenService _ts;
        private IMapper _mapper;

        private static readonly HttpClient client = new HttpClient();

        private readonly Microsoft.Extensions.Options.IOptions<ComSettings> _com;
        private readonly UserManager<AppUser> _manager;
        private readonly SignInManager<AppUser> _signIn;
        private readonly IConfiguration _config;
        private IUsers _users;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AccountController(
            Microsoft.Extensions.Options.IOptions<ComSettings> com,
            ITokenService ts,
            IMapper mapper,
            IConfiguration config,
            UserManager<AppUser> manager,
            IUsers users,
            IWebHostEnvironment hostEnvironment,
            SignInManager<AppUser> signIn)
        {
            _config = config;
            _manager = manager;
            _signIn = signIn;
            _mapper = mapper;
            _ts = ts;
            _com = com;
            _users = users;
            _hostEnvironment = hostEnvironment;



        }

        [HttpGet("checkIfUserExists/{email}")]
        public async Task<int> userexists(string email)
        {
            var result = 0;
            var user = await _manager.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null) { result = 1; }
            return result;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserForRegisterDto registerDto)
        {
            // NB: this is the only place where a new hospital can be added

            var user = await _manager.Users.SingleOrDefaultAsync(x => x.UserName == registerDto.UserName.ToLower());
            if (user != null) { return BadRequest("User already exists ..."); }


           /*  var help = checkHospitalExists(registerDto.currentHospital);
            // if help == 1 then a new hospital is added .... */

            user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                Country = registerDto.country,
                City = registerDto.city,
                KnownAs = registerDto.knownAs,
                hospital_id = Convert.ToInt32(registerDto.currentHospital),
                worked_in = registerDto.currentHospital,
                Created = DateTime.Now,
                LastActive = DateTime.Now,
                PaidTill = DateTime.Now.AddDays(30),
                Email = registerDto.UserName.ToLower(),
                Gender = "Male",
                Mobile = registerDto.mobile,
                active = registerDto.active,
                ltk = registerDto.ltk
            };

            var result = await _manager.CreateAsync(user, registerDto.password);
            if (!result.Succeeded) { return BadRequest(result.Errors); }

            var roleResult = await _manager.AddToRoleAsync(user, "Surgery");
            if (!roleResult.Succeeded) { return BadRequest(roleResult.Errors); }

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _ts.CreateToken(user),
                UserId = user.Id,
                paidTill = user.PaidTill
            };


        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserForLoginDto ufl)
        {
            var user = await _manager.Users.SingleOrDefaultAsync(x => x.UserName == ufl.UserName.ToLower());
            if (user == null) return Unauthorized();

            var result = await _signIn.CheckPasswordSignInAsync(user, ufl.password, false);
            if (!result.Succeeded) return Unauthorized();


            return new UserDto
            {
                UserName = user.UserName,
                Token = await _ts.CreateToken(user),
                UserId = user.Id,
                paidTill = user.PaidTill
            };
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> CChangePwd([FromBody] ChangePasswordDto ufl)
        {
            var user = await _manager.FindByEmailAsync(ufl.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var resultp = await _signIn.CheckPasswordSignInAsync(user, ufl.CurrentPassword, false);
            if (!resultp.Succeeded) return BadRequest("Pls use correct password ...");

            var result = await _manager.ChangePasswordAsync(user, ufl.CurrentPassword, ufl.Password);
            if (!result.Succeeded) return BadRequest("Changing password failed ...");

            return Ok("Password changed");
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _manager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null) return BadRequest("Invalid Request");

            var token1 = await _manager.GeneratePasswordResetTokenAsync(user);
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token1));

            var param = new Dictionary<string, string> { { "token", token }, { "email", forgotPasswordDto.Email } };
            var callback = RequestUriUtil.GetUriWithQueryString(forgotPasswordDto.ClientURI, param);

            var values = new Dictionary<string, string>
            {
            {"to",user.Email},
            {"callback",callback}
            };

            /*  var comaddress = _com.Value.emailHtmlURL;
             var content = new FormUrlEncodedContent(values);
             var response = await client.PostAsync(comaddress, content);
             var responseString = await response.Content.ReadAsStringAsync();
  */
            var comaddress = _com.Value.emailHtmlURL;
            string result = "";
            var jsonString = JsonSerializer.Serialize(values);
            var payLoad = new StringContent(jsonString, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress, payLoad))
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(result);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _manager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null) return BadRequest("Invalid Request");

            var decodedToken = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            var resetPassResult = await _manager.ResetPasswordAsync(user, normalToken, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("hardresetPassword/{email}/{password}")]
        public IActionResult HardResetPassword(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email and Password are required");
            }
            _users.HardResetPassword(email, password);
            return Ok(new { message = "password changed" });
        }


       /*  private async Task<string> checkHospitalExists(string id)
        {
            var result = "0";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/checkIfHospitalExists/" + id;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            return result;
        } */

    }
    public class RequestUriUtil
    {
        public static string GetUriWithQueryString(string requestUri,
            Dictionary<string, string> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUri);
            foreach (var parameter in queryStringParams)
            {
                if (parameter.Value == null)
                {
                    continue;
                }

                sb.Append(startingQuestionMarkAdded ? '&' : '?');
                sb.Append(parameter.Key);
                sb.Append('=');
                sb.Append(parameter.Value);
                startingQuestionMarkAdded = true;
            }
            return sb.ToString();
        }
    }
}