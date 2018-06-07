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

  @JsonProperty('release_date')
  public releaseDate: string;

  @JsonProperty('notes')
  public notes: string;

  @JsonProperty('download_url')
  public downloadUrl: string;

  @JsonProperty('windows_url')
  public windowsUrl: string;

  @JsonProperty('mac_url')
  public macUrl: string;

  @JsonProperty('linux_url')
  public linuxUrl: string;

  public constructor() {
    this.id = null;
    this.applicationId = null;
    this.supported = null;
    this.createdAt = null;
    this.updatedAt = null;
    this.releaseDate = null;
    this.downloadUrl = null;
    this.windowsUrl = null;
    this.macUrl = null;
    this.linuxUrl = null;
    this.notes = null;
  }
}

