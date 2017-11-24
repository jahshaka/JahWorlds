import { JsonProperty } from 'json-typescript-mapper';

export class AssetModel {

    @JsonProperty('id')
    public id: string;

    @JsonProperty('user_id')
    public userId: string;

    @JsonProperty('name')
    public name: string;

    @JsonProperty('type')
    public type: number;

    @JsonProperty('url')
    public url: string;

    @JsonProperty('icon_url')
    public iconUrl: string;

    @JsonProperty('meta_data')
    public metaData: string;

    @JsonProperty('is_public')
    public isPublic: boolean;

    @JsonProperty('tags')
    public tags: string;

    @JsonProperty('collection_id')
    public collectionId: number;

    @JsonProperty('created_at')
    public createdAt: string;

    public constructor() {
        this.id = null;
        this.userId = null;
        this.name = null;
        this.type = null;
        this.url = null;
        this.iconUrl = null;
        this.metaData = null;
        this.isPublic = null;
        this.tags = null;
        this.collectionId = null;
        this.createdAt = null;
    }

}
