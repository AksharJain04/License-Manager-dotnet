import { Component, inject } from '@angular/core';
import { FormsModule, FormGroup, ReactiveFormsModule, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';   // UI Container
import { MatDatepickerModule } from '@angular/material/datepicker';   // UI Datepicker

import { DeviceService } from '../../services/device service/device-service'; 
import { InsertDeviceDto } from '../../models/device-models/insert-device.dto'; 

@Component({
  selector: 'app-device',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule,
    MatInputModule, MatFormFieldModule, MatDatepickerModule
  ],
  templateUrl: './device.html',
  styleUrl: './device.css',
})

export class Device {
  private deviceservice = inject(DeviceService);
  private fb = inject(FormBuilder);

  deviceStatuses = ['Available', 'Sold', 'Replaced'];

  myForm = this.fb.group({
    serialnumber: ['', [Validators.required, Validators.pattern(/^[A-Z0-9]{8,15}$/)]],
    deviceid: ['', [Validators.required, Validators.pattern(/^DEV\d{5}$/)]],
    modelid: ['', [Validators.required, Validators.pattern(/^MOD\d{5}$/)]],
    saledate: [null, [Validators.required]],
    devicestatus: ['', [Validators.required]]
  });

  generateDevice(){
    if (this.myForm.invalid) {
      this.myForm.markAllAsTouched();
      return;
    }

    const dto: InsertDeviceDto = {
      SerialNumber: this.myForm.value.serialnumber!,
      DeviceID: this.myForm.value.deviceid!,
      ModelID: this.myForm.value.modelid!,
      SaleDate: this.myForm.value.saledate!,
      DeviceStatus: this.myForm.value.devicestatus!
    };

    console.log("Sending details to backend...");
    console.log(dto);

    this.deviceservice.createDevice(dto).subscribe({
      next: (device) => {
        console.log("Device entered successfully!");
        console.log(device);
      },
      error: (err) => {
        console.log("Backend returned an error: ")
        console.log(err.status)
        console.log(err.error);
        console.error(err);
      }
    });
  }
}
