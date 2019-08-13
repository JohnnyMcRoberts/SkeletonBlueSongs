import { Component, Inject, Output, OnInit, ElementRef, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { FormBuilder, FormControl } from '@angular/forms';



import { SongsFilesDetailsRequest, SongsFilesDetailsResponse } from './../../Models/SongsFilesDetails';
import { AlbumPlayed } from './../../Models/AlbumPlayed';

import { CurrentLoginService } from './../../Services/current-login.service';
import { FileUploadService } from './../../Services/file-upload.service';
import { DataModelService } from './../../Services/data-model.service';



@Component({
    selector: 'app-albums-report',
    templateUrl: './albums-report.component.html',
    styleUrls: ['./albums-report.component.scss']
})
/** AlbumsReport component*/
export class AlbumsReportComponent
{
    /** AlbumsReport ctor */
    constructor(
        public formBuilder: FormBuilder,
        public dataModelService: DataModelService,
        public currentLoginService: CurrentLoginService)
    {
        // Finish by default at the current 
        var endTime = new Date();
        var endInMilliseconds = endTime.getTime();

        // Start by default 30 days beforehand 
        var startInMilliseconds = endInMilliseconds - (30 * 24 * 60 * 60 * 1000);
        var startTime = new Date(startInMilliseconds);

        // clear the flags
        this.reportIsGenerated = false;

        // Create the form controls
        this.startTime = new FormControl(startTime);
        this.endTime = new FormControl(endTime);
    }

    public startTime: FormControl;
    public endTime: FormControl;
    public reportIsGenerated: boolean = false;
    public albumsPlayed: AlbumPlayed[] = new Array<AlbumPlayed>();

    public onGenerateReport()
    {
        this.reportIsGenerated = true;

        var startDate = new Date(this.startTime.value);
        var endDate = new Date(this.endTime.value);
        var userId = this.currentLoginService.userId;

        this.dataModelService.fetchSongsReportDetailsData(startDate, endDate, userId).then(() =>
        {
            this.albumsPlayed = this.dataModelService.songsReportDetails;

            this.albumsPlayed = new Array<AlbumPlayed>();
            for (let album of this.dataModelService.songsReportDetails)
            {
                const newAlbum = AlbumPlayed.fromData(album);

                if (newAlbum.userName === this.currentLoginService.name)
                {
                    this.albumsPlayed.push(newAlbum);
                }
            }
        });
    }   

    public onExportToFile()
    {
        this.reportIsGenerated = false;
    }

    public onReset()
    {
        this.reportIsGenerated = false;
    }
}