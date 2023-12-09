/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CmeComponent } from './cme.component';

describe('CmeComponent', () => {
  let component: CmeComponent;
  let fixture: ComponentFixture<CmeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CmeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CmeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
