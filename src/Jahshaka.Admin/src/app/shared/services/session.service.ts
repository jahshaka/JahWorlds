import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/finally';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/toPromise';

import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../environments/environment';

import { HttpService } from './http.service';
import { AuthService } from './auth.service';
import { UserModel } from '../models/user.model';
import { BehaviorSubject } from "rxjs/BehaviorSubject";

@Injectable()
export class SessionService {
    private resourceServerUrl: string = environment.resourceServerUrl;

    private userSource = new BehaviorSubject<UserModel>(null);

    user = this.userSource.asObservable();

    public constructor(private router: Router, private httpService: HttpService, private authService: AuthService) { }

    public getCurrentUser()
    {
        return this.httpService.get(this.resourceServerUrl + '/users/me');
    }

    public getUser() {
        this.getCurrentUser().subscribe(response => {
            /*let user: UserModel = {
                id: response.id
            };*/

            var user = new UserModel();
            user.id = response.id;

            this.userSource.next(user);
        }, error => {
            this.userSource.next(null);
        });
    }
}
