import { Component, inject } from '@angular/core';
import { FormsModule, FormGroup, ReactiveFormsModule, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';   // UI Container
import { MatDatepickerModule } from '@angular/material/datepicker';   // UI Datepicker
import { provideNativeDateAdapter } from '@angular/material/core'; 
import { dateRangeValidator } from './data-range.validator';

import { ToastService } from '../../shared/toast.service';
import { CreateLicenseDto } from '../../models/license-models/create-license.dto';
import { LicenseService } from '../../services/license service/license-service';
import { Router } from '@angular/router';

const today = new Date();
const month = today.getMonth();
const year = today.getFullYear();

@Component({
  selector: 'app-license-generator',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule,
    MatInputModule, MatFormFieldModule, MatDatepickerModule
  ],
  templateUrl: './license-generator.html',
  styleUrl: './license-generator.css',
})

export class LicenseGenerator {
  
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private licenseService = inject(LicenseService);
  private toast = inject(ToastService);

  myForm = this.fb.group({
    invoiceid: ['', [Validators.required, Validators.pattern(/^INV-\d{8}$/)]],
    serialnumber: ['', [Validators.required, Validators.pattern(/^[A-Z0-9]{8,15}$/)]],
    activationDate: [null, [Validators.required]],
    expirationDate: [null, [Validators.required]],
    activationstatus: 'Inactive'
  },
  {
    validators: dateRangeValidator
  }); 
  
  generateLicense(){
    if (this.myForm.invalid) {
      this.myForm.markAllAsTouched();
      return;
    }

    const dto: CreateLicenseDto = {
      InvoiceID: this.myForm.value.invoiceid!,
      SerialNumber: this.myForm.value.serialnumber!,
      Activation_Date: this.myForm.value.activationDate!,
      Expiration_Date: this.myForm.value.expirationDate!,
      ActivationStatus: this.myForm.value.activationstatus!
    };
    console.log("Sending details to backend...");
    console.log(dto);

    this.licenseService.createLicense(dto).subscribe({
      next: (license) => {
        console.log("Success Callback.");
        this.toast.success(
          "SUCCESSFUL!", // "LICENSE GENERATED SUCCESSFULLY",
          "License was generated successfully." // "The license has been generated successfully."
        );
      },
      error: (err) => {
        console.error("Backend returned an error: ");
          this.toast.error(
            "LICENSE GENERATION FAILED", err.error
          );
        console.log(err.status);
        console.log(err);
      }
    });
  }
}

