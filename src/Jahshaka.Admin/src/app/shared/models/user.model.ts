import { JsonProperty } from 'json-typescript-mapper';

export class UserModel {

	@JsonProperty('id')
	public id: string;

	@JsonProperty('first_name')
	public firstName: string;

	@JsonProperty('last_name')
	public lastName: string;
	
	@JsonProperty('username')
	public userName: string;
		
	@JsonProperty('email')
	public email: string;

	@JsonProperty('created_at')
	public createdAt: string;

	public constructor() {
		this.id = null;
		this.firstName = null;
		this.lastName = null;
		this.userName = null;
		this.email = null;
		this.createdAt = null;
	}

}
