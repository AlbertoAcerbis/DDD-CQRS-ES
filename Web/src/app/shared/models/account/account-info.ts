export class AccountInfo {
  public accountId: string;
  public accountName: string;
  public password: string;
  public isLoggedIn: boolean;
  public accessToken: string;
  public clientId: string;
  public audience: string;
  public subscriber: string;
  public issuer: string;
  public clientUuid: string;
  public role: string;
  public tokenExpiration: number;

  constructor() {
    this.accountId = '';
    this.accountName = '';
    this.isLoggedIn = false;
    this.accessToken = '';
    this.clientId = '';
    this.audience = '';
    this.subscriber = '';
    this.issuer = '';
    this.role = '';
    this.tokenExpiration = 0;
  }
}
