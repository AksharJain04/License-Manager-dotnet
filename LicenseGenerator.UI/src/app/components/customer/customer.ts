import { Component, inject } from '@angular/core';
import { FormsModule, FormGroup, ReactiveFormsModule, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';   // UI Container
import { MatDatepickerModule } from '@angular/material/datepicker';   // UI Datepicker

import { CustomerService } from '../../services/customer service/customer-service';
import { CreateCustomerDto } from '../../models/customer-models/create-customer.dto';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule,
    MatInputModule, MatFormFieldModule, MatDatepickerModule],
  templateUrl: './customer.html',
  styleUrl: './customer.css',
})

export class Customer {
    private customerservice = inject(CustomerService);
    private fb = inject(FormBuilder);

    myForm = this.fb.group({
      customeremail: ['', [Validators.required, Validators.pattern(/^[^\s@]+@[^\s@]+\.[^\s@]+$/)]],
      customername: ['', [Validators.required]],
      customerphone: ['', [Validators.required, Validators.pattern(/^\d{4,15}$/)]],
      company: ['']
    });

    generateCustomer(){
      if (this.myForm.invalid) {
        this.myForm.markAllAsTouched();
        return;
      };

      const dto: CreateCustomerDto = {
        CustomerEmail: this.myForm.value.customeremail!,
        CustomerName: this.myForm.value.customername!,
        CustomerPhone: this.myForm.value.customerphone!,
        Company: this.myForm.value.company!
      };
      console.log(dto);

      this.customerservice.createCustomer(dto).subscribe({
        next: (customer) => {
          console.log("Customer created successfully!");
          console.log(customer);
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
