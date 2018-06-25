import { Injectable } from '@angular/core';

import { Subject, Observable } from 'rxjs/Rx';

import { Settings } from '../../../app.settings.model';
import { LocalStorageServices } from '../storage/localstorage.service';
import { AccountServices } from '../auth/account.service';
import { Account } from '../../models/account/account';

@Injectable()
export class SettingsService {
  private _showSettings: boolean;
  private _settingsSaveSubject = new Subject<boolean>();

  constructor(private _accountServices: AccountServices,
    private _localStorageServices: LocalStorageServices) {
    this._showSettings = false;
  }

  public toggleShowSettings(): void {
    this._showSettings = !this._showSettings;
  }

  public areSettingsShown(): boolean {
    return this._showSettings;
  }

  public getSettings(): Settings {
    const account: Account = this._localStorageServices.getUserAccount();
    return account.getCurrentSettings();
  }

  public saveSettings(settings: Settings): Observable<boolean> {
    const account = this._accountServices.getLocalAccount();
    account.changeSettings(settings);

    this._accountServices.updateSettings(account).subscribe(res => {
      if (!res) {
        this._settingsSaveSubject.next(false);
      } else {
        this._settingsSaveSubject.next(true);
      }
      },
      err => {
        this._settingsSaveSubject.next(false);
      }
    );

    return this._settingsSaveSubject.asObservable();
  }

  public setLocalSettings(settings: Settings) {
    const account = this._localStorageServices.getUserAccount();
    account.changeSettings(settings);
    this._localStorageServices.setUserAccount(account);
  }
}
