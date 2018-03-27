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
export class ApplicationService {

  private resourceServerUrl: string = environment.resourceServerUrl;

  public constructor(private router: Router, private httpService: HttpService) { }

  public get(page: number) {
    const queryString = `?page=${page}`;
    return this.httpService.get(this.resourceServerUrl + `/admin/applications${queryString}`);
  }

  public getById(id: string) {
    return this.httpService.get(this.resourceServerUrl + `/admin/applications/${id}`);
  }

  public create(data: any) {
    return this.httpService.post(this.resourceServerUrl + `/admin/applications/create`, data);
  }

  public remove(id: string) {
    return this.httpService.get(this.resourceServerUrl + `/admin/applications/${id}/remove`);
  }

  public addVersion(application_id: string, version: any) {
    return this.httpService.post(this.resourceServerUrl + `/admin/applications/${application_id}/version/add`, version);
  }

  public enable(id: string, version_id) {
    return this.httpService.get(this.resourceServerUrl + `/admin/applications/${id}/versions/${version_id}/enable`);
  }

  public disable(id: string, version_id) {
    return this.httpService.get(this.resourceServerUrl + `/admin/applications/${id}/versions/${version_id}/disable`);
  }

  public removeVersion(id: string, version_id) {
    return this.httpService.get(this.resourceServerUrl + `/admin/applications/${id}/versions/${version_id}/remove`);
  }

}
