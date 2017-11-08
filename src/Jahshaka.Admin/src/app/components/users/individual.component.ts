import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {UserModel} from "../../shared/models/user.model";
import {UserService} from "../../shared/services/user.service";
import {ActivityModel} from "../../shared/models/activity.model";
import {ActivityHelper} from "../../shared/utils/activity-helper";

@Component({
  selector: 'individual',
  templateUrl: 'individual.component.html'
})

export class IndividualComponent implements OnDestroy, OnInit  {

  public nav:any;

  private sub:any;

  id: string;

  error: string;

  user: UserModel;

  activities:Array<ActivityModel>;

  public constructor(private userService: UserService, private router: ActivatedRoute, private activityHelper: ActivityHelper) {
    this.nav = document.querySelector('nav.navbar');
  }

  public ngOnInit():any {
    this.nav.className += " white-bg";

    this.sub = this.router.params.subscribe(params => {
      this.id = params['id'];
    });

    this.getUser(this.id);
    this.getActivities(this.id);
  }

  public ngOnDestroy():any {
    this.nav.classList.remove("white-bg");
    this.sub.unsubscribe();
  }

  public getUser(id:string):any{
    this.userService.getUser(id).subscribe(
      (response) => {
        this.user = response;
        console.log(this.user);
      }, (error) => {
        this.error = error.error_description;
      });
  }

  public getActivities(id:string):any{
    this.userService.getActivities(id).subscribe(
      (response) => {
          this.activities = this.activityHelper.formater(response);
          console.log(this.activities);
      }, (error) => {
        this.error = error.error_description;
      });
  }

}
