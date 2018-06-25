import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

import { CommonService } from '../../../shared/services/common.service';
import { LocalStorageServices } from '../storage/localstorage.service';
import { AppConfig } from '../../../shared/config/app.config';
import { HttpClient } from '../http.client';
import { Account } from '../../models/account/account';
import { Role } from '../../models/account/role';

@Injectable()
export class AccountServices extends CommonService {
  private _accountFetchedInfoSubject: Subject<boolean> = new Subject<boolean>();

  constructor(private _appConfig: AppConfig, private _httpClient: HttpClient,
    private _localStorageServices: LocalStorageServices) {
    super();
  }

  public fetchAccount(): Observable<boolean> {
    const accountInfo = this._localStorageServices.getAccountInfo();

    this.getAccountById(accountInfo.accountId).subscribe((res) => {
      this._localStorageServices.setUserAccount(res);
      this._accountFetchedInfoSubject.next(true);
      }, (err) => {
        this._accountFetchedInfoSubject.next(false);
      });

    return this._accountFetchedInfoSubject.asObservable();
  }

  public getLocalAccount(): Account {
    return this._localStorageServices.getUserAccount();
  }

  public setLocalAccount(account: Account) {
    this._localStorageServices.setUserAccount(account);
  }

  public getAccountRole(): Role {
    return this.getRoleByCode(this.getLocalAccount().role);
  }

  public getRoleByCode(roleCode: string): Role {
    let role: Role = null;

    switch (roleCode) {
      case 'S': {
        role = Role.Supervisore;
        break;
      }
      case 'U': {
        role = Role.Utente;
        break;
      }
    }

    return role;
  }

  public updateAccount(account: Account) {
    return this._httpClient.put(this._appConfig.AccountsUrl, account)
      .map(this.extractCommandResult)
      .catch(this.handleError);
  }

  public updateSettings(account: Account) {
    return this._httpClient.put(this._appConfig.AccountSettingsUrl, account)
      .map(this.extractCommandResult)
      .catch(this.handleError);
  }

  public createAccount(account: Account) {
    return this._httpClient.post(this._appConfig.AccountsUrl, account)
    .map(this.extractCommandResult)
    .catch(this.handleError);
  }

  private getAccountById(accountId: string): Observable<Account> {
    return this._httpClient.get(this._appConfig.AccountsUrl + '/' + accountId)
      .map(this.extractJsonData)
      .catch(this.handleError);
  }

  // public getAccounts(): Observable<Account[]> {
  //   return this._httpClient.get(this._appConfig.AccountsUrl)
  //     .map(this.extractVectorData)
  //     .catch(this.handleError);
  // }

  public resetPassword(activationCode: string, password: string) {
    // return this._httpClient.put(this._appConfig.AccountActivationUrl, new PasswordResetData(activationCode, password))
    // .map(this.extractJsonData)
    // .catch(this.handleError);
  }
}
