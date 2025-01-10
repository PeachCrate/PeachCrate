export interface RegisterRequest {
  login: string;
  email: string;
  password: string;
  clerkId: string;
}

export interface RegisterResponse {
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

export interface HelloResp {
  message: string;
}
