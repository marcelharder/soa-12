/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AddValveComponent } from './add-valve.component';

describe('AddValveComponent', () => {
  let component: AddValveComponent;
  let fixture: ComponentFixture<AddValveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddValveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddValveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
