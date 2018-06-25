import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import { RuntimeConfig } from 'app/shared/config/runtime-config';

if (environment.production) {
  enableProdMode();
}

// platformBrowserDynamic().bootstrapModule(AppModule);
RuntimeConfig.loadInstance('assets/config/config.json')
  .then(() => {
      platformBrowserDynamic().bootstrapModule(AppModule);
  });
