import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})

export class Pagination {
  @Input() currentPage = 1;
  @Input() totalPages = 1;

  @Output() pageChanged = new EventEmitter<number>();

  previousPage(){
    if(this.currentPage>1){
      this.pageChanged.emit(this.currentPage-1);
    }
  }
  nextPage(){
    if(this.currentPage<this.totalPages){
      this.pageChanged.emit(this.currentPage+1);
    }
  }

  get pages(): number[] {
    return Array.from(
        { length: this.totalPages },
        (_, i) => i + 1
    );
  }
}
