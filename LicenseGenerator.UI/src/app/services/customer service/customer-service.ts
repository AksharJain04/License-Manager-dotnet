import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { CreateCustomerDto } from '../../models/customer-models/create-customer.dto';
import { CustomerDto } from '../../models/customer-models/customer.dto';
import { PagedResult } from '../../models/license-models/paged-result.dto';

@Injectable({
  providedIn: 'root'
})

export class CustomerService {

  private http = inject(HttpClient);

  private readonly apiUrl = 'http://localhost:5248/api/customer';
  private readonly clurl = 'http://localhost:5248/api/customerlist';

  getCustomers( page: number=1, pageSize: number=10): Observable<PagedResult<CustomerDto>> {
    return this.http.get<PagedResult<CustomerDto>>(
      `${this.clurl}?page=${page}&pageSize=${pageSize}` );
  }

  getCustomer(id: string): Observable<CustomerDto> {
    return this.http.get<CustomerDto>(`${this.apiUrl}/${id}`);
  }

  createCustomer(customer: CreateCustomerDto): Observable<CustomerDto> {
    return this.http.post<CustomerDto>(this.apiUrl, customer);
  }

  updateCustomer(id: string, customer: CustomerDto): Observable<CustomerDto> {
    return this.http.patch<CustomerDto>(
                  `${this.apiUrl}/${id}`, 
                  {customer});
  }

  updateCustomerStatus(id: string, isActive: boolean): Observable<CustomerDto> {
    return this.http.patch<CustomerDto>(
                  `${this.apiUrl}/${id}/status`, 
                  { isActive });
  }

  deleteCustomer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
