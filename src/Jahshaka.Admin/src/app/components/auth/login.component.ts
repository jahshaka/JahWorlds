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

    loginButton = { name: 'Login', disabled: false};

    public constructor(private sessionService: SessionService, private authService: AuthService, private router: Router) {
        this.model = new LoginModel();
    }


    login() {
        this.loginButton.name = 'Loading...';
        this.loginButton.disabled = true;

        this.authService.token({
            username: this.model.username,
            password: this.model.password,
            persistent: this.model.persistent
        }).subscribe(
            (tokenResponse) => {
                this.authService.setAuthData(tokenResponse);

                this.loginButton.name = 'Login';
                this.loginButton.disabled = false;

                this.router.navigate(['home']);

            }, (tokenResponse) => {
                this.error = tokenResponse.error_description;

                this.loginButton.name = 'Login';
                this.loginButton.disabled = false;
            });
    }
}
