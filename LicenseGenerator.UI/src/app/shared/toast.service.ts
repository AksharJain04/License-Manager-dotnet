import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface Toast {
    title: string;
    message: string;
    type: 'success'|'error';
}

@Injectable({
    providedIn: 'root'
})

export class ToastService {
    toast$ = new Subject<Toast>();
    success(title: string, message: string){
        this.toast$.next({
            title,
            message,
            type: 'success'
        });
    }
    error(title: string, message: string){
        this.toast$.next({
            title,
            message,
            type: 'error'
        });
    }
}
