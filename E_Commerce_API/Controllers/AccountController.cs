using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Models.Domain;
using Models.DTO.Register;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        UnitWork unitWork;
        public AccountController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp(string FullName,string Email,string Password,string Repassword,string PhoneNumber)
        {
            if (FullName == "Abdallah Ebrahim" &&
                Email == "engabdallah067@gmail.com" &&
                Password == "1662003aboarab" &&
                Repassword == "1662003aboarab" &&
                PhoneNumber == "01062592321")
            {
                // generate a token
                string secretKey = "you can't see me ya aaroooo 123456789";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "Abdallah Ebrahim"),
                        new Claim(ClaimTypes.Email, "engabdallah067@gmail.com"),
                        new Claim("Hello", "Nawartenaaa")
                    }),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = creds
                };
                var token = new JwtSecurityTokenHandler().CreateToken(tokenDescription); // object of token
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);   // token string
               return Ok(tokenString); // return the token string

            }
            else
            {
                return BadRequest("Invalid credentials");
            }
            
        }
        [HttpPost("LogIn")]
        public ActionResult Login(string email, string password)
        {
            if(email == "engabdallah067@gmail.com" &&  password =="1662003aboarab")
            {
                string secretKey = "you can't see me ya aaroooo 123456789";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Hello","Elmakan makanak"),
                        new Claim(ClaimTypes.Email,"engabdallah067@gmail.com")
                    }),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = creds
                };
                var token = new JwtSecurityTokenHandler().CreateToken(tokenDescription); // object of token
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);   // token string
                return Ok(new { token = tokenString });
               

            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }
        [HttpGet("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await unitWork.db.Registers.FirstOrDefaultAsync(p => p.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            // send email to user
            else
            {
                RegisterDTO registerDTO = new RegisterDTO
                {
                    Password = user.Password,
                };
                return Ok(new { Password = registerDTO.Password });

            }
        }


    }
}
