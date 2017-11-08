import {CurrencyModel} from "./currency.model";

export class AccountModel{
  public id:string;

  public balance:number;

  public confirmedBalance:number;

  public label:string;

  public currency:CurrencyModel;
}
