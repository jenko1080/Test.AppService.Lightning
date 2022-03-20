import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BoundingBox } from './boundingbox';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  constructor(private http: HttpClient) { }

  getBoundingBox(): Observable<BoundingBox> {
    return this.http.get<BoundingBox>('https://app-tc-lightning.azurewebsites.net/config/boundingbox');
  }
}
