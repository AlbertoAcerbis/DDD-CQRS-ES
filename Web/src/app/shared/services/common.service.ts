import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Rx';

@Injectable()
export class CommonService {
  private _eventAnnounced = new Subject<string>();
  public eventAnnounced$ = this._eventAnnounced.asObservable();

  constructor() { }

  public raiseEvent(event: string) {
    this._eventAnnounced.next(event);
  }

  protected extractData(res: Response) {
    const body = res.json();
    if (body) {
      return body.data || {};
    } else {
      return null;
    }
  }

  protected extractVectorData(res: Response) {
    const body = res.json();

    return Array.from(body);
  }

  protected extractScalarData(res: Response) {
    const body = res.json();

    // tslint:disable-next-line:radix
    return parseInt(body);
  }

  protected extractJsonData(res: Response) {
    return res.json();
  }

  protected extractJsonDataTrends(res: Response) {
    const x = res.json();

    const y = x.trend.map(item => {});

    return x;
  }

  protected extractCommandResult(res: Response) {
    return res.ok ? true : false;
  }

  protected handleError (error: Response | any) {
    // TODO: Replace with a remote logging infrastructure
    let errMsg: string;
    if (error instanceof Response) {
      const body = error.json() || '';
      const err = body.error || JSON.stringify(body);
      errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    console.error(errMsg);
    return Observable.throw(errMsg);
  }
}
