import { Component, ViewEncapsulation } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { FormGroup, FormControl, AbstractControl, FormBuilder, Validators} from '@angular/forms';
import { CustomValidators } from 'ng2-validation';
import { AuthService } from '../../shared/services/auth/auth.service';
import { AccountServices } from '../../shared/services/auth/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent {
  public router: Router;
  public form: FormGroup;

  public username: AbstractControl;
  public password: AbstractControl;
  public remember: AbstractControl;
  public loading: boolean;
  public authenticationError: boolean;

  constructor(router: Router, fb: FormBuilder,
    private _authService: AuthService,
    private _accountServices: AccountServices) {
      this.router = router;
      this.form = fb.group({
        'username': ['', Validators.compose([Validators.required])],
        'password': ['', Validators.compose([Validators.required, Validators.minLength(6)])],
        'remember': [false]
      });

    this.username = this.form.controls['username'];
    this.password = this.form.controls['password'];
    this.remember = this.form.controls['remember'];
  }

  public onSubmit(values: Object): void {
    this.loading = true;
    this.authenticationError = false;

    if (this.form.valid) {
      this._authService.login(this.username.value, this.password.value, this.remember.value)
      .subscribe(() => {
        if (this._authService.isLoggedIn) {
          // Fetch the account
          this._accountServices.fetchAccount().subscribe(res => {
            if (!res) {
              this.onAuthenticationError();
            } else {
              // Get the redirect URL from our auth service
              // If no redirect has been set, use the default
              const redirect = this._authService.redirectUrl ? this._authService.redirectUrl : '/pages/dashboard';

              // Set our navigation extras object
              // that passes on our global query params and fragment
              const navigationExtras: NavigationExtras = {
                preserveQueryParams: true,
                preserveFragment: true
              };

              // Redirect the user
              this.router.navigate([redirect], navigationExtras);
            }
          });
        }
      },
      (err) => {
        this.onAuthenticationError();
      });
    }
  }

  // tslint:disable-next-line:use-life-cycle-interface
  ngAfterViewInit() {
      document.getElementById('preloader').classList.add('hide');
  }

  private onAuthenticationError() {
    this.loading = false;
    this.authenticationError = true;
  }

}
