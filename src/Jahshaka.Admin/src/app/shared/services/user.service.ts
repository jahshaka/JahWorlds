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

  public getIndividuals(page, query) {
    var queryString = "?page="+page;
    if (query!=null&&query!=""){
      queryString += "&query="+query;
    }
    return this.httpService.get(this.resourceServerUrl + '/admin/users/individuals'+queryString);
  }

  public getBusinesses(page, query) {
    var queryString = "?page="+page;
    if (query!=null&&query!=""){
      queryString += "&query="+query;
    }
    return this.httpService.get(this.resourceServerUrl + '/admin/users/businesses'+queryString);
  }

  public getAgents(page, query) {
    var queryString = "?page="+page;
    if (query!=null&&query!=""){
      queryString += "&query="+query;
    }
    return this.httpService.get(this.resourceServerUrl + '/admin/users/agents'+queryString);
  }

  public getUser(id) {
    return this.httpService.get(this.resourceServerUrl + '/admin/users/' + id);
  }

  public getActivities(id) {
    return this.httpService.get(this.resourceServerUrl + '/admin/users/' + id + '/activities');
  }
}
