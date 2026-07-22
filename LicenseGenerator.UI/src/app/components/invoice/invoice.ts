import { Component, inject } from '@angular/core';
import { FormsModule, FormGroup, ReactiveFormsModule, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';   // UI Container
import { MatDatepickerModule } from '@angular/material/datepicker';   // UI Datepicker

import { InvoiceService } from '../../services/invoice-service/invoice-service';
import { CreateInvoiceDto } from '../../models/invoice-models/create-invoice.dto';

@Component({
  selector: 'app-invoice',
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule,
    MatInputModule, MatFormFieldModule, MatDatepickerModule],
  templateUrl: './invoice.html',
  styleUrl: './invoice.css',
})

export class Invoice {
  private invoiceservice = inject(InvoiceService);
  private fb = inject(FormBuilder);
  
  myForm = this.fb.group({
    customerid: ['', [Validators.required, Validators.pattern(/^CID-\d{8}$/)]],
    saledate: [null, [Validators.required]],
    dealerid: ['', [Validators.required, Validators.pattern(/^DLR-\d{5}$/)]],
    amount: [0.00, [Validators.required, Validators.min(0.01)]]
  });

  generateInvoice(){
    if (this.myForm.invalid) {
      this.myForm.markAllAsTouched();
      return;
    }

    const dto: CreateInvoiceDto = {
      CustomerID: this.myForm.value.customerid!,
      SaleDate: this.myForm.value.saledate!,
      DealerID: this.myForm.value.dealerid!,
      Amount: this.myForm.value.amount!
    };
    console.log(dto);

    this.invoiceservice.createInvoice(dto).subscribe({
      next: (invoice) => {
        console.log("Invoice created successfully!");
        console.log(invoice);
      },
      error: (err) => {
        console.log("Backend returned an error: ")
        console.log(err.status)
        console.log(err.error);
        console.error(err);
      }
    });
  }
}
