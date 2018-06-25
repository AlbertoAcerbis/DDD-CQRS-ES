import { Routes, RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { PagesComponent } from './pages.component';

export const routes: Routes = [
    {
        path: '',
        component: PagesComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', loadChildren: 'app/pages/dashboard/dashboard.module#DashboardModule', data: { breadcrumb: 'Home page' }  },
            { path: 'articoli', loadChildren: 'app/pages/articoli/articoli.module#ArticoliModule', data: { breadcrumb: 'Articoli' }  },
            { path: 'clienti', loadChildren: 'app/pages/clienti/clienti.module#ClientiModule', data: { breadcrumb: 'Clienti' }  },
       ]
    }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
