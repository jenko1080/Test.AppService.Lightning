import { Injectable } from "@angular/core";
import { webSocket, WebSocketSubject } from "rxjs/webSocket";
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subject } from "rxjs";

@Injectable()
export class WebSocketService {
  private wsNegotiateUrl = "https://app-tc-lightning.azurewebsites.net/pubsub/negotiate";
  private clientAccessUri = "";

  // Socket status to allow watchers to re-subscribe when needed
  private socketStatus = new Subject<string>();
  public status$ = this.socketStatus.asObservable();

  private socket$!: WebSocketSubject<any>;
  public messages$: Observable<any> = of();
  
  constructor(private http: HttpClient) { 
    this.http.get(this.wsNegotiateUrl).subscribe(
      (data: any) => {
        this.clientAccessUri = data.url.result;
        this.socket$ = webSocket(this.clientAccessUri);
        this.messages$ = this.socket$.asObservable();
        this.socketStatus.next("connected");
      },
      (error) => {
        console.log(error);
      }
    );
  }
}