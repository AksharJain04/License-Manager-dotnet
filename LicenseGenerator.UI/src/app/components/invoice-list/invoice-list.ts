import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Pagination } from '../../shared/pagination/pagination';
import { InvoiceDto } from '../../models/invoice-models/invoice.dto';
import { InvoiceService } from '../../services/invoice-service/invoice-service';

type InvoiceViewModel = InvoiceDto & {}

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, FormsModule, Pagination],
  templateUrl: './invoice-list.html',
  styleUrl: './invoice-list.css',
})

export class InvoiceList implements OnInit {
  invoices : InvoiceViewModel[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalRecords = 0;
  
  private invoiceService = inject(InvoiceService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.loadinvoices();
  }

  loadinvoices(){
    this.invoiceService.getInvoices(this.currentPage, this.pageSize).subscribe({
      next: (result) => {
        this.invoices = result.items;
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
      this.loadinvoices();
    }
}
