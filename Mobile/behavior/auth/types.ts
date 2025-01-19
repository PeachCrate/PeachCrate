export interface RegisterRequest {
  login: string;
  email: string;
  password: string;
  clerkId: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface OAuthSignInRequest {
  sessionId: string;
}

export interface Tokens {
  accessToken: AccessToken;
  refreshToken: RefreshToken;
}

export interface AccessToken {
  token: string;
  expiresAt: string;
  issuedAt: string;
}

export interface RefreshToken {
  token: string;
  expiresAt: string;
  issuedAt: string;
}

export interface IsLoginAndEmailTakenRequest {
  login: string;
  email: string;
}

export interface PingResponse {
  message: string;
}
