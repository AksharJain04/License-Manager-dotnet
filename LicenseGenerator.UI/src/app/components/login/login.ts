import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginRequest } from '../../models/auth-models/login-request';
import { Router } from '@angular/router';

import { ToastService } from '../../shared/toast.service';
import { AuthService } from '../../services/auth-service/auth-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})

export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private toast = inject(ToastService);

  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  gotosignup() {
    this.router.navigate(["/signup"]);
  }

  login() {
    if(this.loginForm.invalid) return;
    this.authService.login(
      this.loginForm.getRawValue() as LoginRequest
    ).subscribe({
      next: response => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('username', response.username);
        localStorage.setItem('email', response.email);
        localStorage.setItem('roles', JSON.stringify(response.roles));
        this.router.navigate(['/']);
        console.log('Login successful.');
      },
      error: err => {
        this.toast.error(
            "INVALID USERNAME OR PASSWORD", err.error
          );
        console.error(err);
      }
    });
  }
}
