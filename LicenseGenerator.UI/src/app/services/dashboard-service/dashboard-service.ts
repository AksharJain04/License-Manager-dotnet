import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { DashboardDto } from '../../models/dashboard-models/dashboard-models';
import { Dashboard } from '../../components/dashboard/dashboard';

@Injectable({
    providedIn: 'root'
})

export class DashboardService {
    private readonly apiUrl = "http://localhost:5248/api/dashboard";

    constructor(private http: HttpClient) {}

    getDashboardSummary(): Observable<DashboardDto> {
        return this.http.get<DashboardDto>(this.apiUrl);
    }
}
