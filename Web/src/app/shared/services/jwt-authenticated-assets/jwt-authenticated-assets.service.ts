// http://blog.jsgoupil.com/request-image-files-with-angular-2-and-an-bearer-access-token/

import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscriber } from 'rxjs/Subscriber';

import { AuthService } from '../auth/auth.service';
import { AppConfig } from '../../config/app.config';

@Injectable()
export class JwtAuthenticatedAssetsService {
  constructor(
    private http: Http,
    private _authService: AuthService,
    private _appConfig: AppConfig) {
  }

  getHeaders(): Headers {
    const headers = new Headers();

    //// let token = this.authService.getCurrentToken();
    const token = { access_token: this._authService.getBearerToken() }; // Get this from your auth service.
    if (token) {
      headers.set('Authorization', 'Bearer ' + token.access_token);
    }

    return headers;
  }
}
