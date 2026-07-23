import { Component, OnInit, ChangeDetectorRef, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { DashboardCard } from './dashboard-card/dashboard-card';
import { RouterLink } from '@angular/router';

import { DashboardDto } from '../../models/dashboard-models/dashboard-models';
import { DashboardService } from '../../services/dashboard-service/dashboard-service';

@Component({
  selector: 'app-dashboard',
  imports: [DashboardCard, MatIconModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})

export class Dashboard implements OnInit {
  private cdr = inject(ChangeDetectorRef);
  dashboard : DashboardDto = {
    totalCustomers: 0,
    pendingMappings: 0,
    registeredDevices: 0,
    activeLicenses: 0,
    inactiveLicenses: 0,
    suspendedLicenses: 0
  };
  
  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getDashboardSummary().subscribe({
      next: (data) => {
        console.log(data);
        this.dashboard = data;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
