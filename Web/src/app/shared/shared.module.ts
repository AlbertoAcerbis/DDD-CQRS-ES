import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Http, HttpModule } from '@angular/http';

import { Ng2SmartTableModule } from 'ng2-smart-table';

import { AppConfig } from './config/app.config';

import { AuthService } from './services/auth/auth.service';
import { UsersServices } from './services/users/users.services';

import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RuntimeConfig } from './config/runtime-config';
import { HttpClient } from './services/http.client';
import { TimeServices } from './services/utilities/timeservice';
import { LocalStorageServices } from './services/storage/localstorage.service';
import { AccountServices } from './services/auth/account.service';
import { JwtAuthenticatedAssetsService } from './services/jwt-authenticated-assets/jwt-authenticated-assets.service';

const SharedComponents = [
];

export function getRuntimeConfigInstance() {
  return RuntimeConfig.getInstance('assets/config/config.json');
}

@NgModule({

  imports: [
    CommonModule,
    HttpModule,
    FormsModule,
    Ng2SmartTableModule,
    NgbModule
  ],

  declarations: [
    SharedComponents
  ],
  providers: [
    // { provide: RuntimeConfig, useFactory: () => RuntimeConfig.getInstance('assets/config/config.json') },
    { provide: RuntimeConfig, useFactory: getRuntimeConfigInstance },
    AuthService,
    HttpClient,
    TimeServices,
    LocalStorageServices,
    AccountServices,
    JwtAuthenticatedAssetsService,
    AppConfig,
    UsersServices
  ],
  exports: [
    SharedComponents
  ],
  entryComponents: [
  ]
})
export class SharedModule {}


