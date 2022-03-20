import { Component } from '@angular/core';
import { AzureKeyCredential, WebPubSubServiceClient } from '@azure/web-pubsub';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Lightning UI';
  //cred = new AzureKeyCredential();
  clientAccessUri = "wss://tc-lightning.webpubsub.azure.com/client/hubs/stream?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjpbIndlYnB1YnN1Yi5qb2luTGVhdmVHcm91cC5zdHJlYW0iXSwibmJmIjoxNjQ3NzM1NzExLCJleHAiOjE2NDc3MzkzMTEsImlhdCI6MTY0NzczNTcxMSwiYXVkIjoiaHR0cHM6Ly90Yy1saWdodG5pbmcud2VicHVic3ViLmF6dXJlLmNvbS9jbGllbnQvaHVicy9zdHJlYW0ifQ.P0Xhy0HZdKP0-YiAxOVN5IUqwBIviBKuhMOmX2jpGMQ";

  constructor() { }

  async ngOnInit(): Promise<void> {
    this.subscribeNotification();
  }

  async subscribeNotification() {
    let ws = new WebSocket(this.clientAccessUri);
    ws.onmessage = function (e) {
      var server_message = e.data;
      console.log(server_message);
    }
  };
}
