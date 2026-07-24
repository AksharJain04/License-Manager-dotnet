import { Routes } from '@angular/router';
import { LicenseGenerator } from './components/license-generator/license-generator';
import { LicenseList } from './components/license-list/license-list';
import { Customer } from './components/customer/customer';
import { Device } from './components/device/device';
import { Invoice } from './components/invoice/invoice';
import { Layout } from './components/layout/layout';
import { Dashboard } from './components/dashboard/dashboard';
import { Login } from './components/login/login';
import { authGuard } from './guards/auth-guard';
import { guestGuard } from './guards/guest-guard';
import { InvoiceList } from './components/invoice-list/invoice-list';
import { DeviceList } from './components/device-list/device-list';
import { CustomerList } from './components/customer-list/customer-list';
import { Signup } from './components/signup/signup';

export const routes: Routes = [
   { path: '', component: Layout, canActivate: [authGuard], 
                children: [{ path: '', component: Dashboard },

                        // CREATE PAGES
                           { path: 'license', component: LicenseGenerator },
                           { path: 'customer', component: Customer },
                           { path: 'device', component: Device },
                           { path: 'invoice', component: Invoice },

                        // LIST PAGES
                           { path: 'alllicenses', component: LicenseList },
                           { path: 'allcustomers', component: CustomerList },
                           { path: 'alldevices', component: DeviceList },
                           { path: 'allinvoices', component: InvoiceList }
                  ]},
   { path: 'login', component: Login, canActivate: [guestGuard] },
   { path: 'signup', component: Signup, },
   { path: '**', redirectTo: ''}  // Wildcard route for a 404 page
];

