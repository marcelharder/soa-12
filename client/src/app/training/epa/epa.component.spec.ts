/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EpaComponent } from './epa.component';

describe('EpaComponent', () => {
  let component: EpaComponent;
  let fixture: ComponentFixture<EpaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EpaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EpaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
