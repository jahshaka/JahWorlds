import { Injectable, EventEmitter, Output } from '@angular/core';

@Injectable()
export class ConnectivityUtil
{
	@Output()
	public isOnline: EventEmitter<boolean> = new EventEmitter<boolean>();
}