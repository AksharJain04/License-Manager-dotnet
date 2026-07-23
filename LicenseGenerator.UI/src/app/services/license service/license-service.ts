import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { Injectable, inject } from '@angular/core';

import { PagedResult } from '../../models/license-models/paged-result.dto';
import { LicenseDto } from '../../models/license-models/license.dto';
import { CreateLicenseDto } from '../../models/license-models/create-license.dto';

@Injectable ({
    providedIn: 'root'
})

export class LicenseService {

    private http = inject(HttpClient);
    
    private readonly apiurl = 'http://localhost:5248/api/licenses';
    private readonly llurl = 'http://localhost:5248/api/licenselist';

    createLicense(dto: CreateLicenseDto): Observable<LicenseDto> {
        return this.http.post<LicenseDto>(this.apiurl, dto);
    }

    getLicenses( page: number=1, pageSize: number=10 ): Observable<PagedResult<LicenseDto>> {
        return this.http.get<PagedResult<LicenseDto>>(
            `${this.llurl}?page=${page}&pageSize=${pageSize}`);
    }

    getLicense(id: string): Observable<LicenseDto> {
        return this.http.get<LicenseDto>(`${this.apiurl}/${id}`);
    }

    updateLicenseStatus(id: string, status: string): Observable<void> {
                return this.http.patch<void>(
                        `${this.apiurl}/${id}`,
                        { activationStatus: status }
                );
    }
}

