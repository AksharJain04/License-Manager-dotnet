import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { DashboardCard } from './dashboard-card/dashboard-card';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [DashboardCard, MatIconModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})

export class Dashboard {
  dashboard = {
    activeLicenses: 12,
    inactiveLicenses: 3,
    suspendedLicenses: 1,
    registeredDevices: 58,
    pendingMappings: 4,
    totalCustomers: 7
  };
}
