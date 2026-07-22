import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LicenseGenerator } from './license-generator';

describe('LicenseGenerator', () => {
  let component: LicenseGenerator;
  let fixture: ComponentFixture<LicenseGenerator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LicenseGenerator],
    }).compileComponents();

    fixture = TestBed.createComponent(LicenseGenerator);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
