using api.Data;
using api.DTO;
using api.Entities;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace api.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        public AccountsController(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<MemberDto>>(users));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (currentUser == user) return BadRequest("You can not delete yourself!");

            var result =  await  _userManager.DeleteAsync(user);

            if (result.Succeeded) return Ok("User has been removed successfully");

            return BadRequest("Failed to delete user");
        }

        [Authorize(Policy = "RequireAdminRole")]    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, MemberUpdateDto memberUpdateDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null) return BadRequest("User does not exist!");

            _mapper.Map(memberUpdateDto, user);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok("User has been updated successfully");

            return BadRequest("Failed to update user");
        }

        [Authorize]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == username);

            _mapper.Map(memberUpdateDto, user);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok("User has been updated successfully");

            return BadRequest("Failed to update user");
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username.ToLower()))
                return BadRequest("Username is aldready taken");

            var user = new AppUsers
            {
                UserName = registerDto.Username.ToLower()
            };

            var Pass_result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!Pass_result.Succeeded) return BadRequest(Pass_result.Errors);

            var Role_result = await _userManager.AddToRoleAsync(user, "Member");
            if (!Role_result.Succeeded) return BadRequest(Role_result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null)
                return Unauthorized("Invalid Username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
