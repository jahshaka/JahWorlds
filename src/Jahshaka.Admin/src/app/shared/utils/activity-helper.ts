import {ActivityModel} from "../models/activity.model";
import { Injectable } from "@angular/core";

@Injectable()
export class ActivityHelper
{

  public formater(source:Array<any>):Array<ActivityModel>
  {
    var destination = new Array<ActivityModel>();

    for (var x = 0; x < source.length; x++)
    {
      var activity = new ActivityModel();

      activity.id = source[x].id;
      activity.activityType = source[x].activity_type;
      activity.resourceId = source[x].resource_id;
      activity.resourceType = source[x].resource_type;

      destination.push(activity);
    }

    return destination;

  }

}
