import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AssetService } from 'app/shared/services/asset.service';
import { AssetModel } from 'app/shared/models/asset.model';
import { deserialize } from 'json-typescript-mapper';

@Component({
    selector: 'asset-list',
    templateUrl: 'asset-list.component.html'
})

export class AssetListComponent {

    public assets: Array<AssetModel>;
    public loading: boolean;
    public total: boolean;
    public size: boolean;

    public constructor(private assetService: AssetService) {
        this.getResource(1);
    }

    public getResource(page: number): number {
        this.assets = [];
        this.loading = true;
        this.assetService.Get(page).subscribe(
            (response) => {
                for (const item of response.items) {
                    this.assets.push(deserialize(AssetModel, item));
                }
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
