import { JsonProperty } from 'json-typescript-mapper';

export class ApplicationVersionModel {

  @JsonProperty('id')
  public id: string;

  @JsonProperty('application_id')
  public applicationId: number;

  @JsonProperty('supported')
  public supported: boolean;

  @JsonProperty('created_at')
  public createdAt: string;

  @JsonProperty('updated_at')
  public updatedAt: string;

  @JsonProperty('notes')
  public notes: string;

  @JsonProperty('download_url')
  public downloadUrl: string;

  public constructor() {
    this.id = null;
    this.applicationId = null;
    this.supported = null;
    this.createdAt = null;
    this.updatedAt = null;
    this.downloadUrl = null;
    this.notes = null;
  }
}

