import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ClientiListComponent } from './list/clienti-list.component';
@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        component: ClientiListComponent,
        children: [
          {
            path: 'clienti/list',
            component: ClientiListComponent
          }
        ]
      }
    ])],
    exports: [
        RouterModule
    ]
})
export class ClientiRoutingModule { }

export const RoutedComponents = [ ClientiListComponent ];
