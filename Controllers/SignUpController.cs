using login.DTOs;
using login.Repo.Login_Repo;
using login.Repo.Signup_Repo;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace login.Controllers
{


    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ISignUP _repo;
    
        public SignUpController(ISignUP repo)
        {
            _repo = repo;
        }
    
        // 🔹 Get User by ID (Accessible by Admin & User)
        [HttpGet("get/{id}")]
        [Authorize(Roles = "Admin,User")] 
        public IActionResult GetUserById(int id)
        {
            var user = _repo.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { Status = false, Message = "User not found", Owner = "Cycleny" });
            }
            return Ok(new { Status = true, User = user });
        }
    
        // 🔹 User Signup (Public - No Authorization Required)
        [HttpPost("signup")]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] SignUpAdd log)
        {
            if (log == null)
                return BadRequest(new { Status = false, Message = "Invalid signup data" });
    
            var result = _repo.SignUpFunction(log);
            if (result == null)
                return BadRequest(new { Status = false, Message = "Signup failed" });
    
            return Ok(new { Status = true, Message = "Signup successful", Data = result });
        }
    
        // 🔹 Update User (Only Admin can update any user, User can update their own profile)
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult UpdateUser(int id, [FromBody] SignUpUpadte sign)
        {
            if (sign == null)
                return BadRequest(new { Status = false, Message = "Invalid update data" });
    
            var response = _repo.UpdateUser(id, sign);
            if (response == null)
                return NotFound(new { Status = false, Message = "User not found" });
    
            return Ok(new { Status = true, Message = "User updated successfully", Data = response });
        }
    
        // 🔹 Delete User (Only Admins Can Delete Users)
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            var response = _repo.DeleteUser(id);
            if (response == null)
                return NotFound(new { Status = false, Message = "User not found" });
    
            return Ok(new { Status = true, Message = "User deleted successfully" });
        }
    }

}
