import { Injectable, EventEmitter, Output } from '@angular/core';
import { Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/Rx';

import { HttpClient } from '../utils/http-client';
import { AccessTokenModel, RefreshTokenModel } from '../models/auth-model';
import { environment } from '../../../environments/environment';

@Injectable()
export class AuthService {
    @Output()
    public loggedIn: EventEmitter<any> = new EventEmitter();

    @Output()
    public loggedOut: EventEmitter<any> = new EventEmitter();

    private authServerUrl: string = environment.authServerUrl;
    private resourceServerUrl: string = environment.resourceServerUrl;
    private clientId: string = environment.clientId;
    private clientSecret: string = environment.clientSecret;

    public constructor(private httpClient: HttpClient) {
    }

    public token(model: AccessTokenModel): Observable<Response> {
        let scope = 'openid profile email';

        if (model.persistent) {
            scope += ' offline_access';
        } else {
            scope += ' offline_access';
        }

        var random = Math.floor(Math.random() * 5000 + 1);

        let data = {
            client_id: this.clientId,
            client_secret: this.clientSecret,
            grant_type: 'urn:ietf:params:oauth:grant-type:2fa',
            username: model.username,
            password: model.password,
            scope: scope,
            device_type: 'WebBrowser',
            device_id: `deviceId${random}`,
            device_name: navigator.userAgent
        };

        return this.httpClient.post(this.authServerUrl + '/connect/token', data, {}, {
            'Content-Type': 'application/x-www-form-urlencoded'
        });
    }

    public refreshToken(model: RefreshTokenModel): Observable<Response> {
        let data = {
            client_id: this.clientId,
            client_secret: this.clientSecret,
            grant_type: 'refresh_token',
            refresh_token: model.refreshToken
        };

        return this.httpClient.post(this.authServerUrl + '/connect/token', data, {}, {
            'Content-Type': 'application/x-www-form-urlencoded'
        });
    }

    public userInfo(): Observable<Response> {
        let accessToken: string = 'invalid_access_token';
        let authData = this.getAuthData();

        if (authData != null && authData.access_token) {
            accessToken = authData.access_token;
        }

        return this.httpClient.get(this.authServerUrl + '/connect/userinfo', {}, {
            'Content-Type': 'application/x-www-form-urlencoded',
            'Authorization': 'Bearer ' + accessToken
        });
    }

    public getAuthData(): any {
        return JSON.parse(localStorage.getItem('authData'));
    }

    public setAuthData(authData: any) {
        localStorage.setItem('authData', JSON.stringify(authData));

        this.loggedIn.emit(true);
    }

    public isLoggedIn(): boolean {
        return this.getAuthData() != null;
    }

    public logout(): Observable<any> {

        let authData = this.getAuthData();

        if (authData != null) {

            let token = authData.access_token;

            if (token != null) {

                this.httpClient.delete(this.resourceServerUrl + '/users/security/sessions/current', {
                    Authorization: 'Bearer ' + token
                }).subscribe(response => {
                    console.log('logged out successfully.');
                }, error => {
                    console.log('failed to log out at server.');
                });
            }
        }

        localStorage.removeItem('authData');

        this.loggedOut.emit(true);

        return Observable.of(true);
    }
}
