import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-license-generator',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
  ],
  templateUrl: './license-generator.html',
  styleUrl: './license-generator.css',
})

export class LicenseGenerator {
  invoiceNumber: string = '';
  deviceModel: string = '';
  generatedLicense: string = '';

  generateLicense() {
    this.generatedLicense = 'LIC-' + Math.random().toString(36).substring(2, 10).toUpperCase();
  }
}

