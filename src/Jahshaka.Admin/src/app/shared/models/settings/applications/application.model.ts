import { JsonProperty } from 'json-typescript-mapper';
import { ApplicationVersionModel } from 'app/shared/models/settings/applications/application-version.model';

export class ApplicationModel {

  @JsonProperty('id')
  public id: string;

  @JsonProperty('client_id')
  public clientId: string;

  @JsonProperty('client_secret')
  public clientSecret: string;

  @JsonProperty('display_name')
  public displayName: string;

  @JsonProperty('post_logout_redirect_uris')
  public postLogoutRedirectUris: string;

  @JsonProperty('redirect_uris')
  public redirectUris: string;

  @JsonProperty('type')
  public type: string;

  @JsonProperty('versions')
  public versions: ApplicationVersionModel[];

  public constructor() {
    this.id = null;
    this.clientId = null;
    this.clientSecret = null;
    this.displayName = null;
    this.postLogoutRedirectUris = null;
    this.redirectUris = null;
    this.type = null;
    this.versions = [];
  }
}
