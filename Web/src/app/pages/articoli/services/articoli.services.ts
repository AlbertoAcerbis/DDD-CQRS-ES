import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

import { CommonService } from '../../../shared/services/common.service';
import { HttpClient } from '../../../shared/services/http.client';
import { AppConfig } from '../../../shared/config/app.config';
import { Articolo } from '../models/articolo';


@Injectable()
export class ArticoloServices extends CommonService {

  constructor(private _httpClient: HttpClient, private _appConfig: AppConfig){
    super();
  }

  public getArticoli(): Observable<Articolo[]> {
    return this._httpClient.get(this._appConfig.ArticoliBaseUrl)
      .map(this.extractJsonData)
      .catch(this.handleError);
  }

  public addArticolo(articolo: Articolo) {
    return this._httpClient.post(this._appConfig.ArticoliBaseUrl, articolo)
      .map(this.extractCommandResult)
      .catch(this.handleError);
  }
}