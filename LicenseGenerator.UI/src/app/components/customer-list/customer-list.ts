import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CustomerDto } from '../../models/customer-models/customer.dto';
import { CustomerService } from '../../services/customer service/customer-service';

type CustomerViewModel = CustomerDto & {
  customerStatus: boolean;
  dirty: boolean
};

interface Customer {
  customerId: string;
  customerEmail: string;
  customerName: string;
  customerPhone: string;
  company: string;
  status: string;
};

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.css',
})

export class CustomerList implements OnInit {
  customers : CustomerViewModel[] = [];
  private customerService = inject(CustomerService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.customerService.getAllCustomers().subscribe({
      next: data => {
        this.customers = data.map(customer => ({
          ...customer,
          customerStatus: customer.isActive,
          dirty: false
      }
    ));
    console.log(this.customers);
    this.cdr.detectChanges();
    },
    error: (err) => {
          console.error(err);
      }
    });
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
