import { Injectable } from '@angular/core';
import { RuntimeConfig } from './runtime-config';

@Injectable()
export class AppConfig {

  public BaseMembershipUrl: string;
  public TokenUrl: string;
  public MembershipUrl: string;
  public AccountsUrl: string;
  public AccountSettingsUrl: string;

  public ApiBaseUrl: string;

  public ArticoliBaseUrl: string;
  public ArticoliHub: string;

  public UserBaseApi: string;
  public GetUserApi: string;
  public PostUserApi: string;

  constructor(private _runtimeConfig: RuntimeConfig) {

    this.ApiBaseUrl = this._runtimeConfig.get('baseURL');

    // Authentication
    this.BaseMembershipUrl = '';
    this.TokenUrl = this.BaseMembershipUrl + 'token';
    this.MembershipUrl = this.BaseMembershipUrl +  'api/account/';
    this.AccountsUrl = this.BaseMembershipUrl + 'api/account/';
    this.AccountSettingsUrl = this.AccountsUrl + 'settings/';

    // Articoli
    this.ArticoliBaseUrl = this.ApiBaseUrl + ":44336/v1/articoli";
    this.ArticoliHub = this.ApiBaseUrl + ":44336/articoli";

    // Api Devices
    // this.AccettazioneBaseV1Url = this.ApiDeviceBaseV1Url + '/v1';
    // this.AccettazioneHub = this.ApiDeviceBaseV1Url + '/prestazione';

    // User API Url
    this.UserBaseApi = 'http:google.com';
    this.GetUserApi = this.UserBaseApi + '/get';
    this.PostUserApi = this.UserBaseApi + '/save';
  }

  public iAmAlive() {
    return true;
  }
}
