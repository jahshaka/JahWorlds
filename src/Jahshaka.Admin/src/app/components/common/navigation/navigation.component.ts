import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import 'jquery-slimscroll';
import { UserModel } from "app/shared/models/user.model";
import { SessionService } from "app/shared/services/session.service";

declare var jQuery: any;

@Component({
    selector: 'navigation',
    templateUrl: 'navigation.template.html'
})

export class NavigationComponent implements OnInit {


    private user: UserModel = null;

    constructor(private router: Router, private sessionService: SessionService) {
    }

    ngOnInit(): void {
        this.sessionService.user.subscribe(user => this.user = user);
    }

    ngAfterViewInit() {
        jQuery('#side-menu').metisMenu();

        if (jQuery("body").hasClass('fixed-sidebar')) {
            jQuery('.sidebar-collapse').slimscroll({
                height: '100%'
            })
        }
    }

    activeRoute(routename: string): boolean {
        return this.router.url.indexOf(routename) > -1;
    }


}
