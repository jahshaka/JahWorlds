import { Component, OnDestroy, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { deserialize } from 'json-typescript-mapper';
import { NgForm, NgModel } from '@angular/forms';
import { ApplicationService } from 'app/shared/services/application.service';
import { ApplicationModel } from 'app/shared/models/settings/applications/application.model';
import { ModalComponent } from 'ng2-bs3-modal';
import swal from 'sweetalert2'
import { ToastrService } from 'ngx-toastr';
import { ApplicationVersionModel } from 'app/shared/models/settings/applications/application-version.model';
import { INgxMyDpOptions, IMyDateModel } from 'ngx-mydatepicker';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'application',
    templateUrl: 'application.component.html',
    styleUrls: ['application.component.css']
})

export class ApplicationComponent implements OnInit {

    @ViewChild('AddVersionModal')
    modal: ModalComponent;

    @ViewChild('EditVersionModal')
    edit_modal: ModalComponent;

    public loading = true;
    public application: ApplicationModel;
    public id: string;
    public active_version_index = -1;

    public button = {
        name : 'Save',
        disabled: false
    };

    public edit_button = {
        name : 'Update',
        disabled: false
    };

    public app = {
        id: null,
        download_url: null,
        notes: null
    };

    myOptions: INgxMyDpOptions = {
        // other options...
        dateFormat: 'dd/mm/yyyy',
    };

    model: any;

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
                );import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
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

    public edit(index) {
        this.active_version_index = index;
        if (this.application.versions[this.active_version_index].releaseDate) {
            const releaseDate  = new Date(this.application.versions[this.active_version_index].releaseDate);
            // Initialized to specific date (09.10.2018)
            this.model = { date: { year: releaseDate.getFullYear(), month: releaseDate.getMonth() + 1, day: releaseDate.getDate() } };
        }

        this.edit_modal.open();
    }

    public update() {

        if (this.model) {
            // this.application.versions[this.active_version_index].releaseDate = this.model;
            // tslint:disable-next-line:max-line-length
            this.application.versions[this.active_version_index].releaseDate = this.model.date.day + '/' + this.model.date.month + '/' + this.model.date.year;
        }
        console.log(this.application.versions[this.active_version_index]);

        this.applicationService.updateVersion(this.id, this.application.versions[this.active_version_index]).subscribe(response => {
            // this.application.versions.push(deserialize(ApplicationVersionModel, response));
            // this.app.id = null;
            this.modal.close();
        }, error => {
        });
    }

    public onDateChanged(event: IMyDateModel): void {
        // date selected
    }

}





