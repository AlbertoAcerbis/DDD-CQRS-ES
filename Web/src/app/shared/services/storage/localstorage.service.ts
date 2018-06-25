import { Injectable } from '@angular/core';

import { AccountInfo } from '../../models/account/account-info';
import { Account } from '../../models/account/account';

@Injectable()
export class LocalStorageServices {
  //#region Authentication
  public setAccountInfo(accountInfo: AccountInfo) {
    localStorage.setItem('accountInfo', JSON.stringify(accountInfo));
  }

  public getAccountInfo(): AccountInfo {
    let accountInfo = new AccountInfo();
    const accountJson = localStorage.getItem('accountInfo');
    if (accountJson != null) {
      accountInfo = JSON.parse(accountJson);
    }

    return accountInfo;
  }

  public clearAccountInfo() {
    localStorage.removeItem('accountInfo');
  }
  //#endregion

  //#region Account
  public setSelectesAccount(account: Account) {
    localStorage.setItem('selectedAccount', JSON.stringify(account));
  }

  public getSelectedAccount(): Account {
    const dataFromStorage = localStorage.getItem('selectedAccount');
    if (dataFromStorage === null) {
      return new Account();
    }

    return JSON.parse(dataFromStorage);
  }

  public clearSelectedAccount() {
    localStorage.removeItem('selectedAccount');
  }
  //#endregion

  //#region User Account (Account of the user who has logged in)
  public setUserAccount(account: Account) {
    localStorage.setItem('userAccount', JSON.stringify(account));
  }

  public getUserAccount(): Account {
    const dataFromStorage = localStorage.getItem('userAccount');
    if (dataFromStorage === null) {
      return new Account();
    }

    const account = new Account();
    Object.assign(account, JSON.parse(dataFromStorage));
    return account;
  }

  public clearUserAccount() {
    localStorage.removeItem('userAccount');
  }
  //#endregion

  //#region Clear
  public clearLocalStorageFromAppData() {
    this.clearAccountInfo();
    this.clearSelectedAccount();
   }
  //#endregion
}
