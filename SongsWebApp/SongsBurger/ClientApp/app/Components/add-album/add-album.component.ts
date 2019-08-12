import { Component, OnInit, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';

import { DataModelService } from './../../Services/data-model.service';
import { CurrentLoginService } from './../../Services/current-login.service';

import { AlbumPlayedAddResponse } from './../../Models/AlbumPlayed';

import { BaseAlbumEdit } from './../base-album-edit.component';

@Component({
    selector: 'app-add-album',
    templateUrl: './add-album.component.html',
    styleUrls: ['./add-album.component.scss']
})
/** AddAlbum component*/
export class AddAlbumComponent extends BaseAlbumEdit implements OnInit, AfterViewInit
{

    /** AddAlbum ctor */
    constructor(
        public formBuilder: FormBuilder,
        public dataModelService: DataModelService,
        public currentLoginService: CurrentLoginService)
    {
        super(formBuilder, dataModelService, currentLoginService );
    }

    //#region BaseAlbumEdit Implementation

    @Output() change = new EventEmitter();

    public ngOnInitAddition() {};

    public ngAfterViewInitAddition() {};

    //#endregion

    //#region Page Control Handlers

    public async onAddNewAlbum()
    {
        this.newAlbumPlayed = this.getNewAlbumPlayed();
        console.log('onAddNewAlbum -> newUserName : ', this.newAlbumPlayed.artist);

        await this.dataModelService.addAsyncAlbumPlayed(this.newAlbumPlayed);

        var resp = AlbumPlayedAddResponse.fromData(this.dataModelService.addUserLoginResponse);

        if (resp == undefined)
        {
            console.log("Error in response");
        }
        else
        {
            console.log("Response OK");
            this.onNewAlbumReset();
        }
    }

    public onNewAlbumDisplay(): void
    {
        this.newAlbumPlayed = this.getNewAlbumPlayed();
        this.newAlbumToDisplay = true;
    }

    //#endregion
}