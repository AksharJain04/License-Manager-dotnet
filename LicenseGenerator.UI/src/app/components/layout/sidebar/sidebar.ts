import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

import { AuthService } from '../../../services/auth-service/auth-service';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, MatIconModule, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  private authService = inject(AuthService);
  private router = inject(Router);

  logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
