import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators} from '@angular/forms';
import { Location } from '@angular/common';
import { CustomValidators } from 'ng2-validation';

import { User } from '../../../shared/models/user/user';
import { UsersServices } from '../../../shared/services/users/users.services';
import { FormValidationService } from '../../../shared/services/utilities/formvalidation.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CreateUserComponent implements OnInit {

  public userCreateForm: FormGroup;
  public user = new User;

  public username = '';
  public fullname = '';
  public email = '';
  public type = 'terzista';

  constructor(private _formBuilder: FormBuilder,
              private _userService: UsersServices,
              private _formValidationService: FormValidationService,
              private _location: Location) { }

  ngOnInit() {
    const password = new FormControl('', Validators.compose([Validators.required, Validators.minLength(6)]));
    const confirmPassword = new FormControl('', Validators.compose([Validators.required, CustomValidators.equalTo(password)]));

    this.userCreateForm = this._formBuilder.group({
      'username': ['', Validators.required],
      'fullname': ['', Validators.required],
      'password': password,
      'confirmPassword': confirmPassword,
      'email': ['', Validators.compose([Validators.required, CustomValidators.email])],
    });
  }

  public getTypeValue(selectedValue) {
    this.type = selectedValue;
  }

  public createUser(userData) {
      if (this.userCreateForm.valid) {
        this.user.Username = userData.username;
        this.user.Fullname = userData.fullname;
        this.user.Email = userData.email;
        this.user.UserType = this.type;
        console.log(this.user);

        // userService -> POST new user
      } else {
        this._formValidationService.validateAllUserCreateFormFields(this.userCreateForm);
      }
  }

  public goBack() {
    this._location.back();
  }
}
