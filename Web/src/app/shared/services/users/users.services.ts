import { Injectable } from '@angular/core';
// tslint:disable-next-line:import-blacklist
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

import 'rxjs/add/observable/of';
import 'rxjs/add/operator/delay';
import 'rxjs/add/operator/do';

import { HttpClient } from './../http.client';
import { AppConfig } from './../../config/app.config';
import { CommonService } from './../common.service';
import { User } from './../../models/user/user';

@Injectable()
export class UsersServices extends CommonService {
  constructor(private httpClient: HttpClient,
              private appConfig: AppConfig) {
    super();
  }

  public getUsers(): Observable<User[]> {
    return this.httpClient.get(this.appConfig.GetUserApi)
     .map(this.extractData)
     .catch(this.handleError);
  }

  public saveUser(user: User): Observable<User> {
      return this.httpClient.post(this.appConfig.PostUserApi, user)
        .map(this.extractData)
        .catch(this.handleError);
  }

}
