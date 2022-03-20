import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LightningMapComponent } from './lightning-map.component';

describe('LightningMapComponent', () => {
  let component: LightningMapComponent;
  let fixture: ComponentFixture<LightningMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LightningMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LightningMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
