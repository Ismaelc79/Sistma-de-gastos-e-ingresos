namespace Application.DTOs.Auth;

public class UpdateProfileRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Currency { get; set; }
    public string? Language { get; set; }
    public string? Avatar { get; set; }
}
