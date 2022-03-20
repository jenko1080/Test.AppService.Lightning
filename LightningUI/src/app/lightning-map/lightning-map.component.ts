import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Map, Control, DomUtil, ZoomAnimEvent , Layer, MapOptions, tileLayer, latLng, circle, polygon } from 'leaflet';

@Component({
  selector: 'app-lightning-map',
  templateUrl: './lightning-map.component.html',
  styleUrls: ['./lightning-map.component.scss']
})
export class LightningMapComponent implements OnInit, OnDestroy {

  @Output() map$: EventEmitter<Map> = new EventEmitter;
  @Output() zoom$: EventEmitter<number> = new EventEmitter;
  @Input() options: MapOptions= {
                      layers:[tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                        opacity: 0.7,
                        maxZoom: 19,
                        detectRetina: true,
                        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                      })],
                      zoom:8,
                      center:latLng(-37.808820,144.973906)
  };
  public map!: Map;
  public zoom!: number;
  
  layersControl = {
    baseLayers: {
      'Open Street Map': tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 18, attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors' }),
      'Open Topo Map': tileLayer('http://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', { maxZoom: 18, attribution: '...' })
    },
    overlays: {
      'Lightning': circle([ -37.808820, 144.973906 ], { radius: 5000 })
    }
  }

  constructor() { }

  ngOnInit(): void {
  }

  ngOnDestroy() {
    this.map.clearAllEventListeners;
    this.map.remove();
  };

  onMapReady(map: Map) {
    this.map = map;
    this.map$.emit(map);
    this.zoom = map.getZoom();
    this.zoom$.emit(this.zoom);
  }
}
