import { Component, Input, OnInit } from '@angular/core';
import { detectBody } from '../../../app.helpers';
import { UserModel } from "app/shared/models/user.model";
import { SessionService } from "app/shared/services/session.service";

declare var jQuery: any;

@Component({
    selector: 'basic',
    templateUrl: 'basicLayout.template.html',
    host: {
        '(window:resize)': 'onResize()'
    }
})
export class BasicLayoutComponent implements OnInit {
    public constructor(private sessionService: SessionService) {
        
    }

    public ngOnInit(): any {
        detectBody();
    }

    public onResize() {
        detectBody();
    }

    public ngAfterViewInit() {
    }

}
