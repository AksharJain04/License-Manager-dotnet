import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Pagination } from '../../shared/pagination/pagination';
import { LicenseDto } from '../../models/license-models/license.dto';
import { LicenseService } from '../../services/license service/license-service';

type LicenseViewModel = LicenseDto & {
  originalStatus: string;
    dirty: boolean;
};

@Component({
  selector: 'app-license-list',
  standalone: true,
  imports: [CommonModule, FormsModule, Pagination],
  templateUrl: './license-list.html',
  styleUrl: './license-list.css',
})

export class LicenseList implements OnInit{
  licenses: LicenseViewModel[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalRecords = 0;

  private licenseService = inject(LicenseService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.loadlicenses();
  }

  loadlicenses(){
    this.licenseService.getLicenses(this.currentPage, this.pageSize).subscribe({
      next: (result) => {
        this.licenses = result.items.map(license => ({
            ...license,
            originalStatus: license.activationStatus,
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
    this.loadlicenses();
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
