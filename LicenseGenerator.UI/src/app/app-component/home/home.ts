import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})

export class Home {
  totalLicenses: number = 120;
  activeLicenses: number = 95;
  totalDevices: number = 130;
  pendingActivations: number = 25;

  recentActivities = [
    {
      invoice: 'INV01001',
      serial: 'SN12345',
      action: 'License Generated'
    },
    {
      invoice: 'INV01002',
      serial: 'SN12346',
      action: 'Activated'
    },
    {
      invoice: 'INV01003',
      serial: 'SN12347',
      action: 'License Generated'
    }
  ];
}
