import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';

import { AuthService } from './auth/auth.service';

@Injectable()
export class HttpClient {

  constructor(private http: Http, private _authService: AuthService) {}

  private createAuthorizationHeader(headers: Headers) {
    if (this._authService.isLoggedIn()) {

      //const accountInfo = this._authService.getAccountInfo();
      const token = this._authService.getBearerToken();

      headers.append('Content-Type', 'application/json');
      headers.append('Authorization', 'Bearer ' + token);
    }
  }

  public get(url) {
    const headers = new Headers();
    this.createAuthorizationHeader(headers);
    return this.http.get(url, {
      headers: headers
    });
  }

  public post(url, data) {
    const headers = new Headers();
    this.createAuthorizationHeader(headers);
    return this.http.post(url, data, {
      headers: headers
    });
  }

  public put(url, data) {
    const headers = new Headers();
    this.createAuthorizationHeader(headers);
    return this.http.put(url, data, {
      headers: headers
    });
  }

  public delete(url) {
    const headers = new Headers();
    this.createAuthorizationHeader(headers);
    return this.http.delete(url, {
      headers: headers
    });
  }
}
