import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface License {
  invoiceNumber: string;
  deviceModel: string;
  licenseKey: string;
  serialNumber: string;
  status: string;
}

@Component({
  selector: 'app-license-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './license-list.html',
  styleUrl: './license-list.css',
})

export class LicenseList {
  licenses: License[] = [
    {
      invoiceNumber: 'INV01001',
      deviceModel: 'Model-A',
      serialNumber: 'SN12345',
      licenseKey: 'LIC-A1B2-C3D4',
      status: 'Active'
    },
    {
      invoiceNumber: 'INV01002',
      deviceModel: 'Model-B',
      serialNumber: 'SN12346',
      licenseKey: 'LIC-E5F6-G7H8',
      status: 'Inactive'
    },
    {
      invoiceNumber: 'INV01003',
      deviceModel: 'Model-C',
      serialNumber: 'SN12347',
      licenseKey: 'LIC-I9J0-K1L2',
      status: 'Active'
    }
  ];
}
