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
export class AssetService {
  private resourceServerUrl: string = environment.resourceServerUrl;

  public constructor(private router: Router, private httpService: HttpService) { }

    public Get(page: number, filter) {

        let query = '?page=' + page;

        if (filter.type != null) {
            query += '&type=' + filter.type;
        }

        if (filter.is_public != null) {
            query += '&is_public=' + filter.is_public;
        }

        if (filter.query != null) {
            query += '&query=' + filter.query;
        }

        return this.httpService.get(this.resourceServerUrl + '/admin/assets' + query);
    }

}
