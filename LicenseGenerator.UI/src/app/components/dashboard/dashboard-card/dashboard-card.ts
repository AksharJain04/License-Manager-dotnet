import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dashboard-card',
  standalone: true,
  imports: [MatIconModule, CommonModule],
  templateUrl: './dashboard-card.html',
  styleUrl: './dashboard-card.css',
})

export class DashboardCard {
  @Input() title!: string;
  @Input() value!: number | string;
  @Input() subtitle: string = '';
  @Input() icon!: string;
  @Input() colour: string = '#2563EB';
}
