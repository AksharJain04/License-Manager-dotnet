import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

import { ToastService, Toast } from '../toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.html',
  styleUrl: './toast.css',
})

export class ToastComponent implements OnInit {
  private timeoutId: any;
  private cdr = inject(ChangeDetectorRef);
  private toastservice = inject(ToastService);
  visible = false;
  toast!: Toast;
  
  ngOnInit(): void {
    this.toastservice.toast$.subscribe(data => {

      this.toast = data;
      this.visible = true;

      this.cdr.detectChanges();

      setTimeout(() => {
        this.visible = false;
        this.cdr.detectChanges();
      }, 7000);
    });
  }

  closeToast(){
    console.log("Close button clicked!");

    this.visible = false;
    this.toast = undefined as any;
    clearTimeout(this.timeoutId);
    this.cdr.detectChanges();
  }
}
