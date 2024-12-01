namespace Models.Models;

public class JwtToken
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime IssuedAt { get; set; }
}
