using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context): BaseApiController {

[HttpPost("register")] // /account/register
public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto) {
if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

using var hmac = new HMACSHA512();

var user = new AppUser {
    UserName = registerDto.Username.ToLower(),
    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
    PasswordSalt = hmac.Key
};
context.Users.Add(user);
await context.SaveChangesAsync();
return user;
}
private async Task<bool> UserExists(string username) {
    return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
}
}