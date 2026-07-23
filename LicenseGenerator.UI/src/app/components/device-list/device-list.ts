import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Pagination } from '../../shared/pagination/pagination';
import { DeviceDto } from '../../models/device-models/device.dto';
import { DeviceService } from '../../services/device service/device-service';

type DeviceViewModel = DeviceDto & {
  currentStatus: string;
  dirty: boolean
};

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [FormsModule, CommonModule, Pagination],
  templateUrl: './device-list.html',
  styleUrl: './device-list.css',
})

export class DeviceList implements OnInit {
  devices : DeviceViewModel[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalRecords = 0;

  private deviceService = inject(DeviceService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.loaddevices();
  }

  loaddevices(){
    this.deviceService.getDevices(this.currentPage, this.pageSize).subscribe({
      next: (result) => {
        this.devices = result.items.map(device => ({
          ...device,
          currentStatus: device.deviceStatus,
          dirty: false
      }
    ));
    this.currentPage = result.page;
    this.pageSize = result.pageSize;
    this.totalPages = result.totalPages;
    this.totalRecords = result.totalRecords;
    this.cdr.detectChanges();
    },
    error: (err) => {
          console.error(err);
      }
    });
  }

  onPageChanged(page: number){
    this.currentPage = page;
    this.loaddevices();
  }

  onStatusChange(device: DeviceViewModel) {
    device.dirty = device.deviceStatus !== device.currentStatus;
  }

  updateDeviceStatus(device: DeviceViewModel) {
    console.log("Sending PATCH");

    this.deviceService.updateDeviceStatus(
        device.deviceID,
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