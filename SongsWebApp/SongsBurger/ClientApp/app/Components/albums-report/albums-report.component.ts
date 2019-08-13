import { Component } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';

import * as FileSaver from 'file-saver';

import { AlbumPlayed } from './../../Models/AlbumPlayed';

import { CurrentLoginService } from './../../Services/current-login.service';
import { DataModelService } from './../../Services/data-model.service';

export class ReportDetail
{
    public artist: string;
    public album: string;
    public firstPlayed: string;

    constructor(albumPlayed: AlbumPlayed)
    {
        this.artist = albumPlayed.artist; 
        this.album = albumPlayed.album;
        let dateString = "";
        if (albumPlayed.date != null && albumPlayed.date != undefined)
            dateString = albumPlayed.date.toString();
        this.firstPlayed = dateString;
    }
}

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

    //#region Local Data

    public startTime: FormControl;
    public endTime: FormControl;
    public reportIsGenerated: boolean = false;
    public albumsPlayed: AlbumPlayed[] = new Array<AlbumPlayed>();

    //#endregion

    //#region Page Control Handlers

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
        var reportDetails = new Array<ReportDetail>();
        for (let album of this.albumsPlayed)
            reportDetails.push(new ReportDetail(AlbumPlayed.fromData(album)));

        const replacer = (key, value) => value === null ? '' : value; // specify how you want to handle null values here
        const header = Object.keys(reportDetails[0]);
        let csv = reportDetails.map(row => header.map(fieldName => JSON.stringify(row[fieldName], replacer)).join(','));
        csv.unshift(header.join(','));
        let csvArray = csv.join('\r\n');

        var blob = new Blob([csvArray], { type: 'text/csv' });
        FileSaver.saveAs(blob, "AlbumsReport.csv");
    }

    public onReset()
    {
        this.reportIsGenerated = false;
    }

    //#endregion


}