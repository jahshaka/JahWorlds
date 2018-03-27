import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AssetService } from 'app/shared/services/asset.service';
import { AssetModel } from 'app/shared/models/asset.model';
import { deserialize } from 'json-typescript-mapper';

@Component({
    selector: 'asset-add',
    templateUrl: 'asset-add.html'
})

export class AssetAddComponent {

    public assets: Array<AssetModel>;
    public loading: boolean;
    public total: boolean;
    public size: boolean;

    public filter = {
        type: '',
        query: null,
        is_public: ''
    };

    public data = {
        name: 'xxx',
        collection_id: null,
        description: '',
        tags: []
    }

    public constructor(private assetService: AssetService) {
    }

    public save() {
        console.log(this.data);
    }

}
