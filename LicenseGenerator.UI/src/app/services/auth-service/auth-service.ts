import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { LoginRequest } from '../../models/auth-models/login-request';
import { LoginResponse } from '../../models/auth-models/login-response';

@Injectable({
    providedIn: 'root'
})

export class AuthService {
    private http = inject(HttpClient);
    private readonly apiurl = 'http://localhost:5248/api/auth';

    getToken(): string | null {
        return localStorage.getItem('token');
    }
    isLoggedIn(): boolean {
        return !!this.getToken();
    }
    logout(): void {
        localStorage.clear();
    }

    login(request: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(
            `${this.apiurl}/login`, request);
    }
}
