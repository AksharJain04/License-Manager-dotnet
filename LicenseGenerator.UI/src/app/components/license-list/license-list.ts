import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { LicenseDto } from '../../models/license-models/license.dto';
import { LicenseService } from '../../services/license service/license-service';

type LicenseViewModel = LicenseDto & {
  originalStatus: string;
    dirty: boolean;
};

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

export class LicenseList implements OnInit{
  licenses: LicenseViewModel[] = [];
  private licenseService = inject(LicenseService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.licenseService.getAllLicenses().subscribe({
      next: (data) => {
        this.licenses = data.map(license => ({
            ...license,
            originalStatus: license.activationStatus,
            dirty: false
        }
      ));
      console.log(this.licenses);
      this.cdr.detectChanges();
      },
      error: (err) => {
            console.error(err);
      }
    });
  }

  onStatusChange(license: LicenseViewModel) {
    license.dirty = license.activationStatus !== license.originalStatus;
  }

  updateLicenseStatus(license: LicenseViewModel) {
    console.log("Sending PATCH");
    
    this.licenseService.updateLicenseStatus(
        license.licenseID,
        license.activationStatus
    ).subscribe({
        next: (updatedLicense) => {
          console.log("Status updated successfully.");
          license.originalStatus = license.activationStatus;
          license.dirty = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
            console.error(err);
        }
    });
  }
}
