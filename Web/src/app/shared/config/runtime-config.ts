import { Injectable } from '@angular/core';

@Injectable()
export class RuntimeConfig {

  private static cache = {};

  constructor(private data: any) { }

  // tslint:disable-next-line:member-ordering
  public static loadInstance(jsonFile: string) {
    return new Promise((resolve, reject) => {
      const xobj = new XMLHttpRequest();
      xobj.overrideMimeType('application/json');
      xobj.open('GET', jsonFile, true);
      xobj.onreadystatechange = () => {
        if (xobj.readyState === 4) {
          if (xobj.status === 200) {
            this.cache[jsonFile] = new RuntimeConfig(JSON.parse(xobj.responseText));
            resolve();
          } else {
            reject(`Could not load file '${jsonFile}': ${xobj.status}`);
          }
        }
      };
      xobj.send(null);
    });
  }

  // tslint:disable-next-line:member-ordering
  public static getInstance(jsonFile: string) {
    if (jsonFile in this.cache) {
      return this.cache[jsonFile];
    }
    throw new Error(`Could not find config '${jsonFile}', did you load it?`);
  }

  public get(key: string) {
    if (this.data == null) {
      return null;
    }
    if (key in this.data) {
      return this.data[key];
    }
    return null;
  }
}
