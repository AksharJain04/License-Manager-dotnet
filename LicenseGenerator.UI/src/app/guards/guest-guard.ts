import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-service/auth-service';

export const guestGuard: CanActivateFn = (route, state) => 
{
  const authService = inject(AuthService);
  const router = inject(Router);

  if(authService.isLoggedIn()) return false;

  return true;
};
