import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class HttpErrorHandler {

    constructor(private router: Router) { }

    handle(error: any) {
        console.log(error.status);
        if (error.status === 401) {
            this.router.navigate(['login']);
        }
    }
}
