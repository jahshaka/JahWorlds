import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TreeModel, NodeEvent, NodeMenuItemAction, Ng2TreeSettings } from 'ng2-tree';
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
                    /*const children: Array<TreeModel> = [];
                    for (const collection of response){

                        const data = {
                            id: collection.id,
                            value: collection.name,
                            children: []
                        };

                        children.push(data);
                    };*/
                    callback(this.setChildren(response));
                }, (error) => {
                    console.log(error);
                });
        }
        /*children: [
          {
            value: 'Object-oriented programming',
            id: 5,
            children: [
              {value: 'Java', id: 2},
              {value: 'C++', id: 3},
              {value: 'C#', id: 4}
            ]
          },
          {
            value: 'Prototype-based programming',
            id: 6,
            children: [
              {value: 'JavaScript', id: 7},
              {value: 'CoffeeScript', id: 8},
              {value: 'Lua', id: 9}
            ]
          }
        ]*/
    };

    public setChildren(arg): Array<TreeModel>  {
        const children: Array<TreeModel> = [];
        if (arg) {
            for (const value of arg) {
                children.push({
                    id: value.id,
                    value: value.name,
                    children: this.setChildren(value.collections)
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

        let data = {
            name: e.node.value,
            collection_id: parent.id === 0 ? null : parent.id
        }

        this.collectionService.Create(data).then(
            (response) => {
                console.log(response);
                e.node.id = response.id;
            });

        console.log(e.node.value);
        console.log(e.node.id);

    };

}
