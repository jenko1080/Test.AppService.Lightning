import {
  Component,
  OnInit,
  OnDestroy,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import {
  Map,
  Layer,
  MapOptions,
  tileLayer,
  latLng,
  circle,
  marker,
  LayerGroup,
  icon,
} from 'leaflet';
import { WebSocketService } from '../websocket.service';

@Component({
  selector: 'app-lightning-map',
  templateUrl: './lightning-map.component.html',
  styleUrls: ['./lightning-map.component.scss'],
  providers: [WebSocketService],
})
export class LightningMapComponent implements OnInit, OnDestroy {
  @Output() map$: EventEmitter<Map> = new EventEmitter();
  @Output() zoom$: EventEmitter<number> = new EventEmitter();
  @Input() options: MapOptions = {
    layers: [
      tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        opacity: 0.7,
        maxZoom: 19,
        detectRetina: true,
        attribution:
          '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
      }),
    ],
    zoom: 8,
    center: latLng(-37.80882, 144.973906),
  };

  public map!: Map;
  public zoom!: number;

  public lightningGroup = new LayerGroup();

  layersControl = {
    baseLayers: {
      'Open Street Map': tileLayer(
        'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
          maxZoom: 18,
          attribution:
            '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        }
      ),
      'Open Topo Map': tileLayer(
        'http://{s}.tile.opentopomap.org/{z}/{x}/{y}.png',
        { maxZoom: 18, attribution: '...' }
      ),
    },
    overlays: {},
  };

  constructor(private webSocketService: WebSocketService) {
    this.webSocketService.status$.subscribe((status) => {
      console.log('WebSocket status: ', status);
      if (status === 'connected') {
        this.webSocketService.messages$.subscribe((message) => {
          if (message.st == 'KA') {
            console.log('KA: ', message);
          } else if (message.st === 'CG' || message.st === 'IC') {
            this.addLightningStrike(message.st, message.lat, message.lon);
          } else {
            console.log('Unknown message: ', message);
          }
        });
      }
    });
  }

  ngOnInit(): void {}

  ngOnDestroy() {
    this.map.clearAllEventListeners;
    this.map.remove();
  }

  onMapReady(map: Map) {
    this.map = map;
    this.map$.emit(map);
    this.zoom = map.getZoom();
    this.zoom$.emit(this.zoom);

    this.lightningGroup.addTo(this.map);
  }

  addLightningStrike(type: string, lat: number, long: number) {
    var mk: Layer;
    if(type === "CG") {
      mk = marker([lat, long], {
        icon: icon({
          iconSize: [10, 17],
          iconAnchor: [2, 17],
          iconUrl: 'assets/bolt.svg',
        })
      });
    }
    else if(type === "IC") {
      mk = circle([lat, long], 200);
    }
    else{
      mk = marker([lat, long]);
      console.log("Unknown type: ", type);
    }
    
    console.log('Added marker: ', mk);
    this.lightningGroup.addLayer(mk);
  }
}
