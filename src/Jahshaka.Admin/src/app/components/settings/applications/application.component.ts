import { Component, OnDestroy, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { deserialize } from 'json-typescript-mapper';
import { NgForm, NgModel } from '@angular/forms';
import { ApplicationService } from 'app/shared/services/application.service';
import { ApplicationModel } from 'app/shared/models/settings/applications/application.model';
import { ModalComponent } from 'ng2-bs3-modal';
import swal from 'sweetalert2'
import { ToastrService } from 'ngx-toastr';
import { ApplicationVersionModel } from 'app/shared/models/settings/applications/application-version.model';

@Component({
    selector: 'application',
    templateUrl: 'application.component.html',
    styleUrls: ['application.component.css']
})
export class ApplicationComponent implements OnInit {

    @ViewChild('AddVersionModal')
    modal: ModalComponent;

    public loading = true;
    public application: ApplicationModel;
    public id: string;

    public button = {
        name : 'Save',
        disabled: false
    };

    public app = {
        id: null
    };

    public constructor(private applicationService: ApplicationService, private route: ActivatedRoute, public toastr: ToastrService) {
    }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.id = params['id'];

            this.applicationService.getById(this.id).subscribe(response => {
                this.application = deserialize(ApplicationModel, response);

                this.application.versions = [];
                for (const item of response.versions) {
                    this.application.versions.push(deserialize(ApplicationVersionModel, item));
                }

                console.log(this.application);
                this.loading = false;
            }, error => {
                this.loading = false;
            });
        });
    }

    public save() {

        this.applicationService.addVersion(this.id, this.app).subscribe(response => {
            this.application.versions.push(deserialize(ApplicationVersionModel, response));
            this.app.id = null;
            this.modal.close();
        }, error => {
        });

    }

    public enable(index) {

        this.applicationService.enable(this.id, this.application.versions[index].id).subscribe(response => {
            this.application.versions[index].supported = true;
        }, error => {
        });


    }

    public disable(index) {

        this.applicationService.disable(this.id, this.application.versions[index].id).subscribe(response => {
            this.application.versions[index].supported = false;
        }, error => {
        });

    }

    public delete(index) {
        /*swal({
            title: 'Are you sure?',
            text: 'You will not be able to recover this version!',
            type: 'warning',
            showCancelButton: true,
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, keep it'
        }).then(function() {
            console.log(this.application);
            this.applicationService.remove(this.id, this.application.versions[index].id).subscribe(response => {
                this.application.versions.splice(index, 1);
                swal(
                    'Deleted!',
                    'Application version has been deleted.',
                    'success'
                );
            }, error => {
                swal(
                    'Cancelled',
                    'Something when wrong! Please try again later.',
                    'error'
                );
            });

        }, function(dismiss) {

            if (dismiss === 'cancel') {
              swal(
                'Cancelled',
                'Delete action have been cancelled',
                'error'
              )
            }
        });*/

        this.applicationService.removeVersion(this.id, this.application.versions[index].id).subscribe(response => {
            this.application.versions.splice(index, 1);
            this.toastr.success('Application version has been deleted.', 'Success!');
        }, error => {
            console.log(error);
            this.toastr.error('Something when wrong! Please try again later.', 'Error!');
        });

    }

    public cancel(e) {
        console.log(e);
    }

}





