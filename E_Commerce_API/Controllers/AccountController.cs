using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Models.Domain;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Unit_Of_Work;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.DTO;
using System.Threading.Tasks;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // FIX: Use primary constructor (IDE0290) and disambiguate UserManager
    public class AccountController : ControllerBase
    {
        private readonly JWToption jwtOption;
        private readonly UserManager<ApplicationUser> userManager;
        
        public AccountController( JWToption jwtOption, UserManager<ApplicationUser> userManager)
        {
     
            this.jwtOption = jwtOption;
            this.userManager = userManager;
        }

        // Fix ambiguous reference by fully qualifying RegisterDTO and LoginDTO
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = registerDto.FullName,
                        Email = registerDto.Email,
                        FullName = registerDto.FullName,
                        Address = registerDto.Address,
                        PhoneNumber = registerDto.Phone,
                        State = registerDto.State

                    };
                    var result = await userManager.CreateAsync(user, registerDto.Password);
                    if (result.Succeeded)
                    {
                        // generate token 
                        var secretKey = jwtOption.SecretKey;
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                        var crids = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var roles = await userManager.GetRolesAsync(user);
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Name, user.FullName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("userID",user.Id),
                        };

                        // add roles as claims
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        var tokenDiscription = new SecurityTokenDescriptor()
                        {
                            Subject = new ClaimsIdentity(claims),
                            SigningCredentials = crids,
                            Expires = DateTime.UtcNow.AddDays(1)

                        };
                        var token = new JwtSecurityTokenHandler().CreateToken(tokenDiscription);
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                        return Ok(new
                        {
                            token = tokenString,
                            Expire = token.ValidTo
                        });
                    }
                    else
                    {
                        return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
                    }
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { Error = ex.Message });
            }
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult> Login([FromBody]LoginDTO loginDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByNameAsync(loginDTO.UserName);
                    if (user != null)
                    {
                        var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
                        if (result == true)
                        {
                            // generate token

                            var secretKey = jwtOption.SecretKey;
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                            var crids = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var roles = await userManager.GetRolesAsync(user);
                            var claims = new List<Claim>
                            {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("userID",user.Id),
                            };

                            // add roles as claims
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                            var tokenDiscription = new SecurityTokenDescriptor()
                            {
                                Subject = new ClaimsIdentity(claims),
                                SigningCredentials = crids,
                                Expires = DateTime.UtcNow.AddDays(1)

                            };
                            var token = new JwtSecurityTokenHandler().CreateToken(tokenDiscription);
                            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                            return Ok(new
                            {
                                token = tokenString,
                                Expire = DateTime.UtcNow.AddDays(1)
                            });

                        }
                        return BadRequest("UserName Or Password Wrong");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "UserName Or Password Wrong");
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
        {
            var users = await userManager.Users.ToListAsync();
            var customers = new List<CustomerDTO>();

            foreach (var user in users)
            {
                CustomerDTO customer = new CustomerDTO()
                {
                    CustomerID = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    State = user.State,
                    CreatedAt = user.CreatedAt ?? DateTime.MinValue
                };
                customers.Add(customer);
            }
            return Ok(customers);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User Not Found"); 
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors); 
            }

            return NoContent(); 
        }


    }
}
