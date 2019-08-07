import { Component } from '@angular/core';

import { CurrentLoginService } from './../../../Services/current-login.service';

@Component({
    selector: 'app-main-tables',
    templateUrl: './main-tables.component.html',
    styleUrls: ['./main-tables.component.scss']
})
/** MainTables component*/
export class MainTablesComponent
{
    /** MainTables ctor */
    constructor(private currentLoginService: CurrentLoginService)
    {

    }
}