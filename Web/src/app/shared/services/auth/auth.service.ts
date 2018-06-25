import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { Router } from '@angular/router';
import { Subject } from 'rxjs/Subject';

import * as JWT from 'jwt-decode';

import 'rxjs/add/observable/of';
import 'rxjs/add/operator/delay';
import 'rxjs/add/operator/do';

import { AppConfig } from '../../config/app.config';
import { AccountInfo } from '../../models/account/account-info';
import { TimeServices } from '../utilities/timeservice';
import { LocalStorageServices } from '../storage/localstorage.service';

@Injectable()
export class AuthService {
  private _accountInfo: AccountInfo;

  // Observable boolean sources
  private _logStatusAnnounced = new Subject<AccountInfo>();
  // Observable boolean streams
  public logStatusAnnounced$ = this._logStatusAnnounced.asObservable();
  // store the URL so we can redirect after logging in
  redirectUrl: string;

  constructor(private http: Http, private _config: AppConfig,
    private _router: Router,
    private _localStorageServices: LocalStorageServices,
    private _timeServices: TimeServices) {

      this._accountInfo = new AccountInfo();
  }

  public getAccountInfo(): AccountInfo {
    return this._localStorageServices.getAccountInfo();
  }

  // public getTokenExpiration(): boolean {
  //   const accountInfo = this._localStorageServices.getAccountInfo();

  //   const now = this._timeServices.getNowInEpochFormat();

  //   let tokenIsValid = true;
  //   if (accountInfo &&  accountInfo.tokenExpiration < now) {
  //     tokenIsValid = false;
  //   }

  //   if (!tokenIsValid) {
  //     this.logout();
  //   }

  //   return tokenIsValid;
  // }
  public getTokenExpiration(): number {
    const accountInfo = this._localStorageServices.getAccountInfo();

    return accountInfo.tokenExpiration;
  }

  public getBearerToken(): string {
    // const accountInfo = this._localStorageServices.getAccountInfo();
    // return accountInfo.accessToken;

    return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI1NmUyN2MwOS04NDA4LTQ0NDQtYTM1ZS0xODMzM2YyNWU0ZjgiLCJpYXQiOjI4LCJhY2NvdW50TmFtZSI6ImFsYmVydG8uYWNlcmJpc0A0c29saWQuaXQiLCJhY2NvdW50SWQiOiJkYTJiZDA3NDE1ZmE0NzQ4OWJhMTRlZTdjNjEyNmI5ZiIsImFjY291bnRMYW5ndWFnZSI6Iml0Iiwicm9sZSI6IkMiLCJuYmYiOjE1MjcxNjcxODgsImV4cCI6MTU1ODcyNzE4OCwiaXNzIjoiNFNvbGlkSXNzdWVyIiwiYXVkIjoiNFNvbGlkQXVkaWVuY2UifQ.XU4NEdIQ_y4t5WWW6mE-r1xyJrQcT3wkQeErz6yAqgE";
  }

  public isLoggedIn() {
    // const accountInfo = this._localStorageServices.getAccountInfo();

    // if (!accountInfo) {
    //   return false;
    // }

    // return accountInfo.isLoggedIn;
    return true;
  }

  public login(username: string, password: string, remember: boolean) {
    const headers = new Headers({'Content-Type': 'application/x-www-form-urlencoded'});
    const options = new RequestOptions({headers: headers});
    const body = 'username=' + username + '&password=' + password;

    return this.http.post(this._config.TokenUrl, body, options)
      .map((response: Response) => {
        // login successful if there's a jwt token in the response
        const token = response.json();
        if (token && token.accessToken) {
          const tokenDecoded = JWT(token.accessToken);
          const accountInfo = new AccountInfo();
          accountInfo.accountId = tokenDecoded.accountId;
          accountInfo.accountName = tokenDecoded.accountName;
          accountInfo.password = password;
          accountInfo.role = tokenDecoded.role;
          accountInfo.accessToken = token.accessToken;
          accountInfo.isLoggedIn = true;
          accountInfo.tokenExpiration = token.expiration;

          this._localStorageServices.setAccountInfo(accountInfo);
        }
      });
  }

  public logout(): void {
    this._localStorageServices.clearAccountInfo();
    this._logStatusAnnounced.next(this._accountInfo);

    this._router.navigate(['/login']);
    // this._navCtrl.push(AuthPage);
  }
}
