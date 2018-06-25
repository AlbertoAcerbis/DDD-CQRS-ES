import { Injectable } from '@angular/core';
// tslint:disable-next-line:max-line-length
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild, NavigationExtras, CanLoad, Route } from '@angular/router';

import { AuthService } from './auth.service';
import { TimeServices } from '../utilities/timeservice';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {
    constructor(private _authService: AuthService,
      private _timeServices: TimeServices,
      private _router: Router) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      const url: string = state.url;
      return this.chkLogin(url);
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      return this.canActivate(route, state);
    }

    canLoad(route: Route): boolean {
      const url = `/${route.path}`;
      return this.chkLogin(url);
    }

    chkLogin(url: string): boolean {
      const now = this._timeServices.getNowInEpochFormat();
      if (this._authService.isLoggedIn() && this._authService.getTokenExpiration() > now) {
        return true;
      }

      // Store the attempted URL for redirecting
      this._authService.redirectUrl = url;

      // Create a session id
      // let sessionId = this._authService.generateSessionId();

      // // Set our navigation extras object
      // // that contains our global query params and fragment
      // let navigationExtras: NavigationExtras = {
      //   queryParams: { 'session_id': sessionId },
      //   fragment: ''
      // };

      // Navigate to the login page with extras
      // this._router.navigate(['/login'], navigationExtras);
      this._router.navigate(['/login']);
      return false;
    }

    chkLogin2(url: string): boolean {
      const now = this._timeServices.getNowInEpochFormat();
      const accountInfo = this._authService.getAccountInfo();
      if (accountInfo
        && accountInfo.accountName !== ''
        && accountInfo.password !== '') {
          this._authService.login(accountInfo.accountName, accountInfo.password, true)
            .subscribe(result => {
              // Store the attempted URL for redirecting
              this._authService.redirectUrl = url;
              return true;
            }, error => {
              // Navigate to the login page with extras
              // this._router.navigate(['/login'], navigationExtras);
              this._router.navigate(['/login']);
              return false;
            });
      }

      // Create a session id
      // let sessionId = this._authService.generateSessionId();

      // // Set our navigation extras object
      // // that contains our global query params and fragment
      // let navigationExtras: NavigationExtras = {
      //   queryParams: { 'session_id': sessionId },
      //   fragment: ''
      // };

      return false;
    }
}
