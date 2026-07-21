import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { DeviceDto } from '../../models/device-models/device.dto';
import { DeviceService } from '../../services/device service/device-service';

type DeviceViewModel = DeviceDto & {
  currentStatus: string;
  dirty: boolean
};

interface Device {
  serialNumber: string;
  deviceId: string;
  modelId: string;
  saleDate: string;
  status: string;
};

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './device-list.html',
  styleUrl: './device-list.css',
})

export class DeviceList implements OnInit {
  devices : DeviceViewModel[] = [];
  private deviceService = inject(DeviceService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.deviceService.getAllDevices().subscribe({
      next: data => {
        this.devices = data.map(device => ({
          ...device,
          currentStatus: device.deviceStatus,
          dirty: false
      }
    ));
    console.log(this.devices);
    this.cdr.detectChanges();
    },
    error: (err) => {
          console.error(err);
      }
    });
  }

  onStatusChange(device: DeviceViewModel) {
    device.dirty = device.deviceStatus !== device.currentStatus;
  }

  updateDeviceStatus(device: DeviceViewModel) {
    console.log("Sending PATCH");

    this.deviceService.updateDeviceStatus(
        device.deviceId,
        device.deviceStatus
    ).subscribe({
        next: (updateddevice) => {
          console.log("Device status updated successfully.");
          device.currentStatus = device.deviceStatus;
          device.dirty = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
            console.error(err);
        }
    });
  }
}