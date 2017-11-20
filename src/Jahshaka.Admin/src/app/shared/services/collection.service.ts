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

@Injectable()
export class CollectionService {
  private resourceServerUrl: string = environment.resourceServerUrl;

  public constructor(private router: Router, private httpService: HttpService) { }

  public GetAll() {
    return this.httpService.get(this.resourceServerUrl + '/admin/collections/all');
  }

  public async Create(data: any): Promise<any> {
   const response = await this.httpService.post(this.resourceServerUrl + '/admin/collections/create', data).toPromise();
   return response;
  }

}
