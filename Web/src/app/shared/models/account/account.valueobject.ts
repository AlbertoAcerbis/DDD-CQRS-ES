export class ApplicationSettings {
  public name: string;
  public title: string;
  public theme: Theme;
}

export class Theme {
  public menu: string;
  public menuType: string;
  public showMenu: boolean;
  public navbarIsFixed: boolean;
  public footerIsFixed: boolean;
  public sidebarIsFixed: boolean;
  public showSideChat: boolean;
  public sideChatIsHoverable: boolean;
  public skin: string;
}
