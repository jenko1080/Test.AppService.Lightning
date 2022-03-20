import { Injectable } from "@angular/core";
import { webSocket } from "rxjs/webSocket";
import { HttpClient } from '@angular/common/http';

@Injectable()
export class WebSocketService {
  clientAccessUri = "wss://tc-lightning.webpubsub.azure.com/client/hubs/stream?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjpbIndlYnB1YnN1Yi5qb2luTGVhdmVHcm91cC5zdHJlYW0iXSwibmJmIjoxNjQ3NzQ2ODIzLCJleHAiOjE2NDc3NTA0MjMsImlhdCI6MTY0Nzc0NjgyMywiYXVkIjoiaHR0cHM6Ly90Yy1saWdodG5pbmcud2VicHVic3ViLmF6dXJlLmNvbS9jbGllbnQvaHVicy9zdHJlYW0ifQ.ahK35a0uTKq0Z5jcE8egWemU3gpAbfepNkW4b6VQqgM";

  private socket$  = webSocket(this.clientAccessUri);
  public messages$ = this.socket$.asObservable();

  constructor(private http: HttpClient) { }
}