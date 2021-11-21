using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EVoterAPI.Context;
using E_VoterApi.Models;

namespace EVoterAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EVoterContext _context;

        public UsersController(EVoterContext context)
        {
            _context = context;
        }

        public record GetUsersRequest(int modifiedSinceTicks, int MaxCount);
        // GET: api/Users
        [HttpPost]
        public async Task<object> GetUsers([FromBody] GetUsersRequest body)
        {
            var modifiedUsers = _context.Users.OrderBy(u => u.modifiedTicks).Where(u => u.modifiedTicks > body.modifiedSinceTicks);

            var users = new List<User>();
            users.AddRange(await modifiedUsers.Take(body.MaxCount).ToListAsync());

            return new
            {
                ModifiedCount = await modifiedUsers.CountAsync(),
                Users = users
            };
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
