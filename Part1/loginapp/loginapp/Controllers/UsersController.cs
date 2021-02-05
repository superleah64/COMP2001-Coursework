using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using loginapp.Models;

namespace loginapp.Controllers
{
    [ApiController]
    [Route("loginapp/Users")]
   
    public class UsersController : ControllerBase
    {
        private readonly DataAccess _context;

        [NonAction]
        public void register(Users user, out string Register)
        {
            string outRegister;
            _context.RegisterUser(user, out outRegister);

            Register = outRegister;
        }

        [NonAction]
        public bool getValidation(Users user)
        {
            return _context.ValidateUser(user);
        }

        public UsersController(DataAccess context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> Get(Users user)
        {

            bool validation = getValidation(user);

            if (validation)
            {
                return Ok(true);
            }

            else
            {
                return StatusCode(208);
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            _context.UpdateUser(user, id);

            return StatusCode(200);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Post(Users user)
        {
            string Response;

            register(user, out Response);

            if (Response.Contains("200"))
            {
                return Ok(user);
            }
            else
            {
                return StatusCode(208);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _context.DeleteUser(id);
            return StatusCode(200);
        }

        [NonAction]
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
