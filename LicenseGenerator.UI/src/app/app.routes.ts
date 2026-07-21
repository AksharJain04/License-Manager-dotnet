import { Routes } from '@angular/router';
import { LicenseGenerator } from './app-component/license-generator/license-generator';
import { Home } from './app-component/home/home';
import { LicenseList } from './app-component/license-list/license-list';
import { AppComponent } from './app-component/app-component';

export const routes: Routes = [
    { path: 'generate', component: LicenseGenerator },
    { path: '', component: Home },
    { path: 'licenses', component: LicenseList },
    { path: 'app', component: AppComponent },
    { path: '**', component: Home }  // Wildcard route for a 404 page
];


