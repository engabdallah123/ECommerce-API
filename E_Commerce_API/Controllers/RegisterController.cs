using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTO.Image;
using Models.DTO.Register;
using System;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RegisterController : ControllerBase
    {
        UnitWork unitWork;
        public RegisterController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegisterById(int id)
        {
            try
            {
                var register = await unitWork.RegisterRepo.GetByIdAsync(id);
                if (register == null)
                    return NotFound();
                else
                {
                    RegisterDTO registerDTO = new RegisterDTO()
                    {
                        Id = register.Id,
                        FullName = register.FullName,
                        Email = register.Email,
                        Password = register.Password,
                        Repassword = register.Repassword,
                        PhoneNumber = register.PhoneNumber,


                    };
                    return Ok(registerDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,new {Error=ex.Message});
            }
           
                
        }
        [HttpGet("image/{id}")]
        public IActionResult GetImageById(int id)
        {
            try
            {
                var imageData = unitWork.db.FolderImages
                    .Where(x => x.RegisterId == id)
                    .Select(x => x.Data)
                    .FirstOrDefault();
                if (imageData == null)
                    return NotFound("No image found.");
                else
                    return File(imageData, "image/jpeg");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });

            }

        }
        [HttpPost]
        public IActionResult AddRegister([FromForm] RegisterDTO registerDTO)
        {
            try
            {
                if (registerDTO == null)
                    return BadRequest("Invalid data.");
                else
                {

                    var register = new Register()
                    {
                        FullName = registerDTO.FullName,
                        Email = registerDTO.Email,
                        Password = registerDTO.Password,
                        Repassword = registerDTO.Repassword,
                        PhoneNumber = registerDTO.PhoneNumber

                    };
                    unitWork.RegisterRepo.Add(register);
                    unitWork.Save();

                    if (registerDTO.Image != null)
                    {
                        using var ms = new MemoryStream();
                        registerDTO.Image.CopyTo(ms);

                        var folderImage = new FolderImage
                        {
                            RegisterId = register.Id,
                            Data = ms.ToArray(),
                        };
                        unitWork.db.FolderImages.Add(folderImage);
                        unitWork.Save();


                    }
                    return Created(nameof(GetRegisterById), register);
                }
            

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }


        }
        [HttpDelete("DeleteAccount/{id}")]
        [Authorize]
        public IActionResult DeleteAccount(int id)
        {
            var register = unitWork.RegisterRepo.GetById(id);
            if (register == null)
                return NotFound("Account not found.");
            else
            {
                unitWork.RegisterRepo.Delete(id);
                unitWork.Save();
                return Ok("Account deleted successfully.");
            }
        }
    }
}
