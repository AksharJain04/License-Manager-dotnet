import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Pagination } from '../../shared/pagination/pagination';
import { CustomerDto } from '../../models/customer-models/customer.dto';
import { CustomerService } from '../../services/customer service/customer-service';

type CustomerViewModel = CustomerDto & {
  customerStatus: boolean;
  dirty: boolean
};

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [FormsModule, CommonModule, Pagination],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.css',
})

export class CustomerList implements OnInit {
  customers : CustomerViewModel[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalRecords = 0;

  private customerService = inject(CustomerService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.loadcustomers();
  }

  loadcustomers(){
    this.customerService.getCustomers(this.currentPage, this.pageSize).subscribe({
      next: (result) => {
        this.customers = result.items.map(customer => ({
          ...customer,
          customerStatus: customer.isActive,
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
    this.loadcustomers();
  }

  onStatusChange(customer: CustomerViewModel) {
    customer.dirty = customer.isActive !== customer.customerStatus;
  }

  updateCustomerStatus(customer: CustomerViewModel) {
    console.log("Sending PATCH");

    this.customerService.updateCustomerStatus(
        customer.customerID,
        customer.isActive
    ).subscribe({
        next: (updatedcustomer) => {
          console.log("Customer deleted successfully.");
          customer.customerStatus = customer.isActive;
          customer.dirty = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
            console.error(err);
        }
    });
  }
}
