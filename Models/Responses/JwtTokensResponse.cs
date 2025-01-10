using Models.Models;

namespace Models.Responses;

public record struct JwtTokensResponse(JwtToken AccessToken, JwtToken RefreshToken);
