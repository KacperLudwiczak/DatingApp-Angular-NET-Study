namespace API.DTOs;

public class UserDto
{
    public required string UserName { get; set; }
    public required string Token { get; set; }
    public string? PhotoUrl { get; set; }

}