import { Injectable } from '@angular/core';

import * as moment from 'moment/moment';

@Injectable()
export class TimeServices {

  public getNowInEpochFormat(): number {
    return moment().utc().unix();
  }

  public getHHmmssFromTotalSeconds(seconds: number): string {

    if (seconds === 0) {
      return '---';
    } else {
      const h = Math.floor(seconds / 3600);
      const m = Math.floor(seconds % 3600 / 60);
      // const s = Math.floor(seconds % 3600 % 60);

      return ('0' + h).slice(-2) + 'h' + ('0' + m).slice(-2) + 'm'; // + ':' + ('0' + s).slice(-2);
    }
  }
}
