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
import { UserModel } from '../models/user.model';

@Injectable()
export class UserService {
  private resourceServerUrl: string = environment.resourceServerUrl;

  public constructor(private router: Router, private httpService: HttpService) { }

  public All(page: number) {
    return this.httpService.get(`${this.resourceServerUrl}/admin/users?page=${page}`);
  }
}
