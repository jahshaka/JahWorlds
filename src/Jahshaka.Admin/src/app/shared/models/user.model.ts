import {ProfileModel} from './profile.model';
import {AvatarModel} from './avatar.model';
import {MobileNumberModel} from './mobile-number.model';
import {AccountModel} from './account.model';
import { JsonProperty } from 'json-typescript-mapper';

export class UserModel {

	public constructor() {
	  this.id = null;
	  this.firstName = null;
	  this.lastName = null;
	  this.createdAt = null;
	}

	@JsonProperty('id')
	public id: string;

	@JsonProperty('first_name')
	public firstName: string;

	@JsonProperty('last_name')
	public lastName: string;

	@JsonProperty('created_at')
	public createdAt: string;

}
