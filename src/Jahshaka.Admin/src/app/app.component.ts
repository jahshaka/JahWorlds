import { Component, OnInit, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs/Subject';

import { HttpClient } from './shared/utils/http-client';
import { AuthGuard } from './shared/services/auth-guard';
import { AuthService } from './shared/services/auth.service';
import { HttpService } from './shared/services/http.service';
import { HttpErrorHandler } from './shared/services/http-error-handler';
import { SessionService } from './shared/services/session.service';
import { ConnectivityUtil } from './shared/utils/connectivity-util';
import { UserModel } from './shared/models/user.model';
import { Observable } from 'rxjs/Observable';
import { SessionStateModel } from 'app/shared/models/session-state.model';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
    user: UserModel = null;

    constructor(private router: Router, private authService: AuthService, private sessionService: SessionService, private connectivityUtil: ConnectivityUtil) {
    }

    ngOnInit() {
        this.connectivityUtil.isOnline.subscribe(isOnline => {
            if (!isOnline) {
                console.log('Offline');
            }

        });

        this.authService.loggedIn.subscribe(response => {
            this.getUser();
        });

        this.authService.loggedOut.subscribe(response => {
            this.user = null;
            this.router.navigate(['login']);
        });

        if (this.authService.isLoggedIn()) {
            this.getUser();
        }
    }

    getUser() {
        this.sessionService.getUser();
    }
}
