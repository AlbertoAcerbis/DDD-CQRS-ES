import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ArticoliListComponent } from './list/articoli-list.component';
@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        component: ArticoliListComponent,
        children: [
          {
            path: 'articoli/list',
            component: ArticoliListComponent
          }
        ]
      }
    ])],
    exports: [
        RouterModule
    ]
})
export class ArticoliRoutingModule { }

export const RoutedComponents = [ ArticoliListComponent ];
