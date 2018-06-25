import { Menu } from './menu.model';

export const verticalMenuItems = [
  new Menu (10, 'Home page', '/pages/dashboard', null, 'home', null, false, 0, '*'),
  new Menu (20, 'Articoli', '/pages/articoli', null, 'check-square', null, false, 0, '*'),
  new Menu (30, 'Clienti', '/pages/clienti', null, 'check-square', null, false, 0, '*')
]

export const horizontalMenuItems = [
  new Menu (10, 'Home page', '/pages/dashboard', null, 'home', null, false, 0, '*'),
  new Menu (20, 'Articoli', '/pages/articoli', null, 'check-square', null, false, 0, '*'),
  new Menu (30, 'Clienti', '/pages/clienti', null, 'check-square', null, false, 0, '*')
]
