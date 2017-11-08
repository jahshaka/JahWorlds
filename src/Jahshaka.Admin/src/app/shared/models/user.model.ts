import {ProfileModel} from "./profile.model";
import {AvatarModel} from "./avatar.model";
import {MobileNumberModel} from "./mobile-number.model";
import {AccountModel} from "./account.model";

export class UserModel
{
	public id: string;

	public firstName: string;

	public lastName: string;

	public createdAt: string;

	public account:AccountModel;

	public profile: ProfileModel;

	public avatar: AvatarModel;

	public mobileNumber: MobileNumberModel;
}
