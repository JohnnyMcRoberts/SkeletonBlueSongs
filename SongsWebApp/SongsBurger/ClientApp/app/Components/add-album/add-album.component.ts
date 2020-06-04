import { Component, OnInit, Output, EventEmitter, AfterViewInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';

// Components
import { SearchDetailComponent } from './../search-detail/search-detail.component';

// Services
import { DataModelService } from './../../Services/data-model.service';
import { CurrentLoginService } from './../../Services/current-login.service';

// Models
import { AlbumPlayed, AlbumPlayedAddResponse } from './../../Models/AlbumPlayed';

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
        super(formBuilder, dataModelService, currentLoginService);
        this.newAlbumPlayed = new AlbumPlayed();
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

        const resp = AlbumPlayedAddResponse.fromData(this.dataModelService.addUserLoginResponse);

        if (resp === undefined)
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

    public onFindAlbumDetails(): void
    {
        this.searchAlbum = this.getNewAlbumPlayed();
        this.newAlbumDetailsToSearch = true;
        console.log("onFindAlbumDetails -> about to display");
    }

    //#endregion

    //#region Find Details properties

    @ViewChild('searchDetail') searchDetail: SearchDetailComponent;

    newAlbumDetailsToSearch: boolean = false;
    searchAlbum: AlbumPlayed = new AlbumPlayed();

    //#endregion
}