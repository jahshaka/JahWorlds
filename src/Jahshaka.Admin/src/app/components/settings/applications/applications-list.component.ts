import { Component, OnDestroy, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';
import { deserialize } from 'json-typescript-mapper';
import { NgForm, NgModel } from '@angular/forms';
import { ApplicationService } from 'app/shared/services/application.service';
import { ApplicationModel } from 'app/shared/models/settings/applications/application.model';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastrService } from 'ngx-toastr';
import 'bootstrap';


@Component({
    selector: 'applications-list',
    templateUrl: 'applications-list.component.html'
})
export class ApplicationsListComponent implements OnInit {

    public loading = true;
    public applications: Array<ApplicationModel>;
    public total: number;
    public size: number;
    public page: number;

    @ViewChild('AddApplicationModal')
    modal: ModalComponent;

    public app = {
        client_id: null,
        client_secret: null,
        display_name: null,
        type: 'public',
        post_logout_redirect_uris: null,
        redirect_uris: null
    };

    public button = {
        name : 'Save',
        disabled: false
    };

    public constructor(private applicationService: ApplicationService, public toastr: ToastrService, private router: Router) {
    }

    ngOnInit(): void {
        this.get(1);
    }

    public get(page) {
        this.loading = true;
        this.applications = [];
        this.applicationService.get(page).subscribe(response => {

            for (let item of response.items) {
                this.applications.push(deserialize(ApplicationModel, item));
            }

            console.log(this.applications);

            this.total = response.paging.total_items;
            this.size = response.paging.page_size;
            this.page = response.paging.current_page;
            this.loading = false;
        }, error => {
            this.loading = false;
        });
    }

    public create() {
        this.button.name = 'Saving...';
        this.button.disabled = true;
        this.applicationService.create(this.app).subscribe(response => {
            this.router.navigate(['/settings/applications', response.id]);
        }, error => {
            console.log(error);
            this.toastr.error('Something when wrong! Please try again later.', 'Error!');
            this.button.name = 'Save';
            this.button.disabled = false;
        });
    }

    public remove(index) {
        this.applicationService.remove(this.applications[index].id).subscribe(response => {
            this.applications.splice(index, 1);
            this.toastr.success('Application has been deleted.', 'Success!');
        }, error => {
            console.log(error);
            this.toastr.error('Something when wrong! Please try again later.', 'Error!');
        });
    }

    public cancel(e) {
        console.log(e);
    }

}





