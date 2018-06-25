import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { AuthService } from '../../../shared/services/auth/auth.service';
import { SettingsService } from '../../../shared/services/utilities/settings.services';
import { AccountServices } from '../../../shared/services/auth/account.service';
import { AccountInfo } from '../../../shared/models/account/account-info';
import { Role } from '../../../shared/models/account/role';

@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserMenuComponent implements OnInit {
  public accountInfo: AccountInfo;
  public account: Account;
  public roleDescription: string;

  constructor(private _authService: AuthService,
    private _settingsService: SettingsService,
    private _accountServices: AccountServices) { }

  ngOnInit() {
  }

  private getAccountInfo() {
    this.accountInfo = this._authService.getAccountInfo();
    this.roleDescription = this.getRoleDescription(this._accountServices.getAccountRole());
  }

  public toggleSettings(): void {
    this._settingsService.toggleShowSettings();
  }

  public logout() {
    this._authService.logout();
  }

  private getRoleDescription(role: Role): string {
    switch (role) {
      case Role.Supervisore:
        return 'Supervisore';
      case Role.Utente:
        return 'Utente';
      default:
        return 'Guest';
    }
  }
}
