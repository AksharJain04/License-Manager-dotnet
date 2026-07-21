import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { DashboardModel } from '../../models/dashboard-models/dashboard-models';

@Injectable({
    providedIn: 'root'
})

export class DashboardService {
    getDashboard(): Observable<DashboardModel> {
        return of ({
            activeLicenses: 0,
            inactiveLicenses: 0,
            suspendedLicenses: 0,
            registeredDevices: 0,
            pendingMappings: 0,
            totalCustomers: 0
        });
    }
}
