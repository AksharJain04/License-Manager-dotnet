import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})

export class Navbar {
  username = localStorage.getItem('username') ?? '';
  roles = JSON.parse(
    localStorage.getItem('roles') ?? '[]'
  );
}
