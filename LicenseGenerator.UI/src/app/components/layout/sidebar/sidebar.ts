import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AuthService } from '../../../services/auth-service/auth-service';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, MatIconModule, RouterLinkActive, MatTooltipModule, CommonModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})

export class Sidebar {
  private authService = inject(AuthService);
  private router = inject(Router);

  isCollapsed = false;

  toggleSidebar(){
    this.isCollapsed = !this.isCollapsed;
  }

  logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
