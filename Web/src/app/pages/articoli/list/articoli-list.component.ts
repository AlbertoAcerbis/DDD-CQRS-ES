import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';

import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { AppConfig } from '../../../shared/config/app.config';
import { ArticoloServices } from '../services/articoli.services';
import { Articolo } from '../models/articolo';

@Component({
  selector: 'articoli-list',
  templateUrl: 'articoli-list.component.html',
  styleUrls: ['./articoli-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ArticoliListComponent implements OnInit, OnDestroy {
  private _hubConnection: HubConnection;

  public loading: boolean;
  public signalrErrorStatus: boolean;
  public articoli: Articolo[];

  constructor(private _appConfig: AppConfig, 
    private _articoloServices: ArticoloServices) {
    
  }

  ngOnInit() {
    this.openSignalrConnection();
    this.getArticoli();
  }

  ngOnDestroy() {
    this._hubConnection.stop();
  }

  private getArticoli() {
    this.loading = true;
    this.articoli = [];

    this._articoloServices.getArticoli().subscribe(
      result => {
        this.articoli = result;
        this.loading = false;
      }, (error) => {
        this.loading = false;
        alert(error);
      }
    );
  }

  //#region signalR
  private openSignalrConnection() {
    this._hubConnection = new HubConnectionBuilder()
    .withUrl(this._appConfig.ArticoliHub)
    .build();

    this._hubConnection.start()
      .then(() => this.subscribeSignalrEvents())
      .catch((error) => {
        this.signalrErrorStatus = true;
        this.showSignalrMessages('SignalR connection not started!!! ' + error)
      });
  }

  private subscribeSignalrEvents() {
    this.signalrErrorStatus = false;
    this._hubConnection.on("ArticoloCreated", (hubResponse) => {
      this.getArticoli();
    });
  }

  private showSignalrMessages(message: string) {
    console.log(message);
  }
  //#endregion

  //#region UI-EventHandler
  public addArticolo() {
    const articolo = new Articolo();
    //articolo.articoloDescrizione = "Design Thinking";
    //articolo.articoloDescrizione = "DDD BlueBook";
    articolo.articoloDescrizione = "DDD RedBook";
    // articolo.articoloDescrizione = "Implementing DDD C#";
    // articolo.articoloDescrizione = "Functional Programming in C#";
    articolo.unitaMisura = "NR";
    articolo.scortaMinima = 10;

    this._articoloServices.addArticolo(articolo).subscribe(
      (result) => {

      }
    )
  }
  //#endregion
}