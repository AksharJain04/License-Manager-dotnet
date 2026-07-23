import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PagedResult } from '../../models/license-models/paged-result.dto';
import { CreateInvoiceDto } from '../../models/invoice-models/create-invoice.dto';
import { InvoiceDto } from '../../models/invoice-models/invoice.dto';

@Injectable({
  providedIn: 'root'
})

export class InvoiceService {

    private http = inject(HttpClient);
    
    private readonly apiUrl = "http://localhost:5248/api/invoice";
    private readonly ilUrl = "http://localhost:5248/api/invoicelist"

    getInvoices( page: number=1, pageSize: number=10 ): Observable<PagedResult<InvoiceDto>> {
        return this.http.get<PagedResult<InvoiceDto>>(
            `${this.ilUrl}?page=${page}&pageSize=${pageSize}`);
    }
    
    getInvoice(id: string): Observable<InvoiceDto> {
        return this.http.get<InvoiceDto>(`${this.apiUrl}/${id}`);
    }
    
    createInvoice(invoice: CreateInvoiceDto): Observable<InvoiceDto> {
        return this.http.post<InvoiceDto>(this.apiUrl, invoice);
    }

}
