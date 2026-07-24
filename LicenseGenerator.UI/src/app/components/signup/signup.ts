import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup,ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { ToastService } from '../../shared/toast.service';
import { AuthService } from '../../services/auth-service/auth-service';
import { RegisterRequest } from '../../models/auth-models/register-request';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})

export class Signup {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);
  private toastService = inject(ToastService);

  errorMessage = '';
  successMessage = '';

  signupForm : FormGroup = this.fb.group ({
    username: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmpassword: ['', [Validators.required]]
    },
    {
      validators: this.passwordsMatchValidator
    }
  );

  passwordsMatchValidator (control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmpassword = control.get('confirmpassword')?.value;

    if (password !== confirmpassword) return { passwordsMismatch: true };
    return null;
  }

  register(){

    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }

    const request: RegisterRequest = {
      username: this.signupForm.value.username,
      email: this.signupForm.value.email,
      password: this.signupForm.value.password
    };

    this.authService.register(request).subscribe({
      next: (request) => {
        this.toastService.success(
          "REGISTERED!",
          "Your signup was successful."
        );
      },
      error: (err) => {
        this.toastService.error(
          "UNSUCCESSFUL!",
          err.error
        );
      }
    });
  }
}
