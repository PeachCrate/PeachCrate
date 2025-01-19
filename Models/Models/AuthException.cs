namespace Models.Models;

public class AuthException : Exception
{
    public AuthErrorType Type { get; }

    public AuthException(AuthErrorType type) : base(GetMessage(type))
    {
        Type = type;
    }

    public string GetMessage()
    {
        return GetMessage(Type);
    }

    public static string GetMessage(AuthErrorType type)
    {
        return type switch
        {
            AuthErrorType.InvalidToken => "Invalid refresh token.",
            AuthErrorType.TokenExpired => "Token expired.",
            AuthErrorType.InvalidUserIdInJwtClaims => "User ID in JWT is invalid.",
            AuthErrorType.UserNotFound => "User not found.",
            AuthErrorType.BadCredentials => "Bad credentials.",
            AuthErrorType.LoginTaken => "This login is taken.",
            AuthErrorType.EmailIsUsed => "This email is in use.",
            _ => ""
        };
    }
}

public enum AuthErrorType
{
    InvalidUserIdInJwtClaims,
    InvalidToken,
    TokenExpired,
    UserNotFound,
    BadCredentials,
    LoginTaken,
    EmailIsUsed,
}
