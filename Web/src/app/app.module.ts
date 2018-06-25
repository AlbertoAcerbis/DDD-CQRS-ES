import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AgmCoreModule } from '@agm/core';
import { CalendarModule } from 'angular-calendar';

import { routing } from './app.routing';
import { AppSettings } from './app.settings';

import { AppComponent } from './app.component';
import { NotFoundComponent } from './pages/errors/not-found/not-found.component';

import { registerLocaleData } from '@angular/common';
//#region locale Angular5 Breaking change - manually import locale
// https://stackoverflow.com/questions/47263717/missing-locale-data-for-the-locale-ru-angular-5/47263949
import localeIt from '@angular/common/locales/it'
registerLocaleData(localeIt);
//#endregion

import { SharedModule } from './shared/shared.module';
import { LocalStorageServices } from './shared/services/storage/localstorage.service';
import { AccountServices } from './shared/services/auth/account.service';
import { SettingsService } from './shared/services/utilities/settings.services';
import { UsersServices } from './shared/services/users/users.services';
import { FormValidationService } from './shared/services/utilities/formvalidation.service';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyDe_oVpi9eRSN99G4o6TwVjJbFBNr58NxE'
    }),
    SharedModule,
    CalendarModule.forRoot(),
    routing
  ],
  providers: [ AppSettings, LocalStorageServices, SettingsService, AccountServices, UsersServices, FormValidationService ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
