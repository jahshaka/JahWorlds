import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TreeModel, NodeEvent, Ng2TreeSettings } from 'ng2-tree';
import { CollectionService } from 'app/shared/services/collection.service';

@Component({
    selector: 'collections',
    templateUrl: 'collections.component.html'
})

export class CollectionsComponent {

    settings: any = {
        cssClasses: {
            expanded: 'fa fa-caret-down',
            collapsed: 'fa fa-caret-right',
            empty: 'fa fa-caret-right disabled',
            leaf: 'fa'
        },
        templates: {
            node: '<i class="fa fa-folder-o"></i>',
            leaf: '<i class="fa fa-file-o"></i>'
        }
    };

    public tree: TreeModel = {
        value: '/',
        id: 0,
        settings: this.settings,
        loadChildren : (callback) => {
            this.collectionService.GetAll().subscribe(
                (response) => {
                    callback(this.setCollection(response));
                }, (error) => {
                    console.log(error);
                });
        }
    };

    public setCollection(arg): Array<TreeModel>  {
        const children: Array<TreeModel> = [];
        if (arg) {
            for (const value of arg) {
                children.push({
                    id: value.id,
                    value: value.name,
                    children: this.setCollection(value.collections).concat(this.setAsset(value.assets))
                });
            }
        }
        return children;
    };

    public setAsset(arg): Array<TreeModel>  {
        const children: Array<TreeModel> = [];
        if (arg) {
            for (const value of arg) {
                children.push({
                    id: value.id,
                    value: value.name
                });
            }
        }
        return children;
    };

    public constructor(private collectionService: CollectionService) {
    }

    public logEvent(e: NodeEvent): void {
        console.log(e);
    }

    public handleCreate(e: NodeEvent) {

        const parent = e.node.parent;

        const data = {
            name: e.node.value,
            collection_id: parent.id === 0 ? null : parent.id
        }

        this.collectionService.Create(data).then(
            (response) => {
                console.log(response);
                e.node.id = response.id;
            });

    };

    public handleRemoved(e: NodeEvent) {
        console.log(e);
        /*
        this.collectionService.Remove(e.node.id).then(
            (response) => {
                console.log(response);
                console.log('Removed successfully');
            });
        */
    }

    public handleRenamed(e: NodeEvent) {
        console.log(e);

        this.collectionService.Rename(e.node.value, e.node.id).then(
            (response) => {
                console.log(response);
                console.log('Renamed successfully');
            });

    }

    public handleMoved(e: NodeEvent) {
        console.log(e);

        const parent = e.node.parent;

        this.collectionService.Update(e.node.id, parent.id).then(
            (response) => {
                console.log(response);
                console.log('Update successfully');
            });

    }

}
