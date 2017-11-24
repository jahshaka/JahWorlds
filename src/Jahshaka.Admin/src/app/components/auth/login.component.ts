import { Component } from '@angular/core';
import { SessionService } from '../../shared/services/session.service';
import { AuthService } from '../../shared/services/auth.service';
import { LoginModel } from '../../shared/models/login.model';
import { Router } from '@angular/router';

@Component({
    selector: 'login',
    templateUrl: 'login.component.html'
})

export class LoginComponent {

    model: LoginModel;

    error: string;


    public constructor(private sessionService: SessionService, private authService: AuthService, private router: Router) {
        this.model = new LoginModel();
    }


    login() {
        this.authService.token({
            username: this.model.username,
            password: this.model.password,
            persistent: this.model.persistent
        }).subscribe(
            (tokenResponse) => {
                this.authService.setAuthData(tokenResponse);

                this.router.navigate(['home']);

            }, (tokenResponse) => {
                this.error = tokenResponse.error_description;
            });
    }
}
