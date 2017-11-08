import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {UserModel} from "../../shared/models/user.model";
import {UserService} from "../../shared/services/user.service";
import {forEach} from "@angular/router/src/utils/collection";

@Component({
    selector: 'individuals',
    templateUrl: 'individuals.component.html'
})

export class IndividualsComponent  implements OnDestroy, OnInit  {

  public nav:any;
  users:Array<UserModel>;
  total:number;
  size:number;
  query:string = null;
  error: string;
  loading: boolean;

  public constructor(private userService: UserService) {
    this.nav = document.querySelector('nav.navbar');
  }

  public ngOnInit():any {
    this.nav.className += " white-bg";
    this.getIndividuals(1);
  }

  public ngOnDestroy():any {
    this.nav.classList.remove("white-bg");
  }

  public getIndividuals(event):any{
    this.users = Array<UserModel>();
    this.loading = true;
    this.userService.getIndividuals(event, this.query).subscribe(
      (response) => {
        this.users = response.users;
        this.total = response.total;
        this.size = response.size;
        this.loading = false;
      }, (error) => {
        this.error = error.error_description;
        this.loading = false;
      });

    return event;
  }

}
