namespace Models.Models;

public class RefreshToken
{
    public int RefreshTokenId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
