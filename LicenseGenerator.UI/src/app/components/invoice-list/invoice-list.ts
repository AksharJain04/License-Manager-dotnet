import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { InvoiceDto } from '../../models/invoice-models/invoice.dto';
import { InvoiceService } from '../../services/invoice-service/invoice-service';

type InvoiceViewModel = InvoiceDto & {}

interface Invoice {
  invoiceId: string;
  customerId: string;
  saleDate: string;
  dealerId: string;
  amount: number;
};

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './invoice-list.html',
  styleUrl: './invoice-list.css',
})

export class InvoiceList implements OnInit {
  invoices : InvoiceViewModel[] = [];
  private invoiceService = inject(InvoiceService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.invoiceService.getAllInvoices().subscribe({
      next: (invoices) => { 
        this.invoices = invoices;
        console.log(this.invoices);
        this.cdr.detectChanges();
      },
      error: (err) => { 
        console.error(err);
      }
    });
  }
}
