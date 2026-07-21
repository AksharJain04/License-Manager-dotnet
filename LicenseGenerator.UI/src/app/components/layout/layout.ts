import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { Sidebar } from './sidebar/sidebar';

@Component({
  selector: 'app-layout',
  imports: [Navbar, Sidebar, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout {
  
}
