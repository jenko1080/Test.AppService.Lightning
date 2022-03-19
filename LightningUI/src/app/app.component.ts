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
  clientAccessUri = "wss://tc-lightning.webpubsub.azure.com/client/hubs/stream?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjpbIndlYnB1YnN1Yi5zZW5kVG9Hcm91cC5zdHJlYW0iLCJ3ZWJwdWJzdWIuam9pbkxlYXZlR3JvdXAuc3RyZWFtIl0sIm5iZiI6MTY0NzY3MDY0MSwiZXhwIjoxNjQ3Njc0MjQxLCJpYXQiOjE2NDc2NzA2NDEsImF1ZCI6Imh0dHBzOi8vdGMtbGlnaHRuaW5nLndlYnB1YnN1Yi5henVyZS5jb20vY2xpZW50L2h1YnMvc3RyZWFtIn0.cwoqKhm00wyma2Ec0ZB4Q9LJxn3PInzB4vvmDSDpLlg";

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
