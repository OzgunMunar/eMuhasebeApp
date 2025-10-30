import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { signalRApi } from '../constants';

@Injectable({
  providedIn: 'root'
})

export class SignalrService {
  
  hub: signalR.HubConnection | undefined

  constructor() {

  }

  connect(callBack: () => void) {

    this.hub = new signalR.HubConnectionBuilder()
      .withUrl(`${signalRApi}/report-hub`)
      .build()

    this.hub
      .start()
      .then(() => {
        console.log("Report Hub connection is successful.")
        callBack()
      })
  }

}
