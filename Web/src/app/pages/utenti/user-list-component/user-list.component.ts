import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../../../shared/models/user/user';
import { UsersServices } from '../../../shared/services/users/users.services';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserListComponent implements OnInit, OnDestroy {

  public userList: User[] = [];

  ngOnDestroy(): void {
    // throw new Error('Method not implemented.');
  }

  constructor( private _usersServices: UsersServices,
               private _router: Router) { }

  ngOnInit() {
    this.getUsers();
  }

  public getUsers() {
    this._usersServices.getUsers()
      .subscribe((users) => {
        this.userList = users;
      });
  }

  // tslint:disable-next-line:member-ordering
  public settings = {
    selectMode: 'single',  // single|multi
    hideHeader: false,
    hideSubHeader: false,
    actions: {
      columnTitle: 'Actions',
      add: false,
      edit: true,
      delete: true,
      custom: [],
      position: 'right' // left|right
    },
    add: {
      addButtonContent: '<h4 class="mb-1"><i class="fa fa-plus ml-3 text-success"></i></h4>',
      createButtonContent: '<i class="fa fa-check mr-3 text-success"></i>',
      cancelButtonContent: '<i class="fa fa-times text-danger"></i>'
    },
    edit: {
      editButtonContent: '<i class="fa fa-pencil mr-3 text-primary"></i>',
      saveButtonContent: '<i class="fa fa-check mr-3 text-success"></i>',
      cancelButtonContent: '<i class="fa fa-times text-danger"></i>'
    },
    delete: {
      deleteButtonContent: '<i class="fa fa-trash-o text-danger"></i>',
      confirmDelete: true
    },
    noDataMessage: 'No data found',
    mode: 'external',
    columns: {
      Username: {
        title: 'Username',
        editable: false,
        type: 'html',
        valuePrepareFunction: (value) => { return '<div class="text-center">' + value + '</div>'; }
      },
      FullName: {
        title: 'Full Name',
        type: 'string',
        filter: true
      },
      Email: {
        title: 'E-mail',
        type: 'string'
      },
      UserType: {
        title: 'User Type',
        type: 'number'
      }
    },
    pager: {
      display: true,
      perPage: 10
    }
  };

  public onDeleteConfirm(event): void {
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();
    } else {
      event.confirm.reject();
    }
  }

  public onRowSelect(event) {
    console.log(event);
  }

  public goToCreateUser() {
    this._router.navigateByUrl('/pages/create-user');
  }

  public goToEditUser(event) {
    // evet -> user -> local storage
    this._router.navigateByUrl('/pages/create-user');
  }

}
