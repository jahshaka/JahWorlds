import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from 'app/shared/models/user.model';
import { deserialize } from 'json-typescript-mapper';
import { UserService } from 'app/shared/services/user.service';

@Component({
    selector: 'users-list',
    templateUrl: 'user-list.component.html'
})

export class UserListComponent {

    public users: Array<UserModel>;
    public loading: boolean;
    public total: boolean;
    public size: boolean;

    public constructor(private userService: UserService) {
        this.getIndividuals(1);
    }

    public getIndividuals(page: number): number {
        this.users = [];
        this.loading = true;
        this.userService.All(page).subscribe(
            (response) => {
                for (const item of response.items) {
                    this.users.push(deserialize(UserModel, item));
                }
                console.log('Users', this.users);
                this.total = response.paging.total_items;
                this.size = response.paging.page_size;
                this.loading = false;
            }, (error) => {
                // this.error = error.error_description;
                this.loading = false;
            });

        return page;
    }

}
