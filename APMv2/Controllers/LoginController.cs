using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using APMv2.Model.Entities;
using APMv2.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;   
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APMv2.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly APMv2Context _context;
        public LoginController(APMv2Context context)
        {
            _context = context;
        }
        [HttpPost]
        //[Authorize]
        public IActionResult Login([FromBody]LoginRequest model)
        {

            var user = _context.User.Where(en => en.Username == model.Username && en.Password == model.Password).FirstOrDefault();
            if (user == null)
            {
                var res = new
                {
                    isValid = false,
                    errorMessage = "Tài khoản hoặc mật khẩu chưa đúng",
                };
                return Ok(res);
            }
            else
            {
                string tokenString = "";
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@123"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        //new Claim(ClaimTypes.Role, "Manager")
                    };

                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: signingCredentials
                    );
                //HttpContext.Session.SetString("jwt", new JwtSecurityTokenHandler().WriteToken(tokenOptions));
                //HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                //HttpContext.Session.SetString("username", user.Username);
                //var username = HttpContext.Session.GetString("username");
                //if (user.ActiveAccount == Activate.Active)
                //{
                //    tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                //}

                tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                var userReturn = new User()
                {
                    Id = user.Id,
                    FullName = user.Username,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Position = user.Position,
                };
                return Ok(new
                {
                    isValid = true,
                    user = Newtonsoft.Json.JsonConvert.SerializeObject(userReturn),
                    Token = tokenString
                });
                //Newtonsoft.Json.JsonConvert.SerializeObject(listSprintBacklog);
            }
        }

        [HttpPost("CheckToken")]
        public IActionResult CheckToken()
        {
            var token = HttpContext.Session.GetString("username");
            return Ok(true);
        }
    }
}