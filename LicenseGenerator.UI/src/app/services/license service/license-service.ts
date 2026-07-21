import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { Injectable, inject } from '@angular/core';

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

    getAllLicenses(): Observable<LicenseDto[]> {
        return this.http.get<LicenseDto[]>(this.llurl);
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

