import { ApplicationSettings } from './account.valueobject';
import { Settings } from '../../../app.settings.model';

export class Account {
  public accountId: number;
  public username: string;
  public password: string;
  public name: string;
  public surname: string;
  public gender: string;
  public role: string;
  public image: string;
  public isActive: boolean;
  public isDeleted: boolean;
  public registrationDate: Date;
  public lastJoinedDate: Date;
  public passwordNeverExpires: boolean;
  public passwordDaysDuration: number;
  public passwordStarted: Date;
  public passwordExpired: Date;
  public applicationSettings: ApplicationSettings[];

  private _currentSettingsIndex: number;

  constructor(fields?: {
    accountId: string, username: string, password: string, name: string, surname: string, gender: string, image: string,
    isActive: boolean, isDeleted: boolean, registrationDate: Date, lastJoinedDate: Date, passwordNeverExpires: boolean,
    passwordDaysDuration: number, passwordStarted: Date, passwordExpired: Date, applicationSettings: Settings[]}) {
    if (fields) {
      Object.assign(this, fields);
    } else {
      this.isActive = false;
      const defaultTheme = {
            menu: 'vertical', // horizontal , vertical
            menuType: 'mini', // default, compact, mini
            showMenu: true,
            navbarIsFixed: true,
            footerIsFixed: false,
            sidebarIsFixed: true,
            showSideChat: false,
            sideChatIsHoverable: false,
            skin: 'light'  // light , dark, blue, green, combined, purple, orange, brown, grey, pink
      };
      const defaultApplicationSettings: Settings = {
        name: 'Default',
        title: '',
        theme: defaultTheme
      };
      this.applicationSettings = [ defaultApplicationSettings ];
      this._currentSettingsIndex = 0;
    }
  }

  public getCurrentSettings() {
    return this.applicationSettings[this._currentSettingsIndex];
  }

  public changeSettings(settings: Settings) {
    this.applicationSettings[this._currentSettingsIndex] = settings;
  }
}
