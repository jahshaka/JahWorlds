export class TokenModel {
    accessToken: string | null;
    refreshToken: string | null;
}

export class SessionUserModel {
    id: string | null;
    name: string | null;
    avatar: string | null;
}

export class SessionStateModel {
    isLoggedIn: boolean | false;
    tokenData: TokenModel;
    userData: SessionUserModel;
}
