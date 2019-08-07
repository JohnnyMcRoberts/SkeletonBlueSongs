import { Component, Inject, Output, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

import { SongsFilesDetailsRequest, SongsFilesDetailsResponse } from './../../Models/SongsFilesDetails';

import { CurrentLoginService } from './../../Services/current-login.service';
import { FileUploadService } from './../../Services/file-upload.service';
import { SongsFilesDetailsService } from './../../Services/songs-files-details.service';

export interface IHash
{
    [details: string]: boolean;
} 

@Component({
    selector: 'app-text-file-import-export',
    templateUrl: './text-file-import-export.component.html',
    styleUrls: ['./text-file-import-export.component.scss']
})
/** TextFileImportExport component*/
export class TextFileImportExportComponent
{
    /** TextFileImportExport ctor */
    constructor(
        private currentLoginService: CurrentLoginService,
        private fileUploadService: FileUploadService,
        private songsFilesDetailsService: SongsFilesDetailsService
        )
    {
        this.uploader = new FileUploader({
            url: "URL",
            disableMultipart: true
        });

    }

    public tmpFolder: string = "temp";

    @ViewChild('fileInput') fileInput: any;
    public uploader: FileUploader;

    public uploadFile()
    {
        //this.uploader.clearQueue();
        this.uploader.uploadAll();
        this.fileInput.nativeElement.value = '';
    }

    // File upload
    public uploadFileName: string;
    public isSubmitted: boolean;
    public visibilityHash: IHash = {};
    public isCheckThumbnail: IHash = {};
    public savedFilesHash: IHash = {};
    public onFileSelected(fileInfo: any)
    {
        const file: File = fileInfo._file;
        this.visibilityHash[file.name] = true;
        this.savedFilesHash[file.name] = false;
        console.log(file.name);

        this.uploadFileName = file.name;
        this.fileUploadService.upload(this.tmpFolder, [file]);
    }


    // File calculate details.
    private temporaryPath: string = "temp";
    public songFileDetails: Array<any>;

    public async onGetDetails(fileInfo: any)
    {
        const file: File = fileInfo._file;

        console.log(file.name);
        var detailsRequest = new SongsFilesDetailsRequest();
        detailsRequest.fileName = file.name;
        detailsRequest.filePath = this.temporaryPath;

        await this.songsFilesDetailsService.getAsyncSongsFilesDetailsData(detailsRequest);

        var tempDetails: any[] = Object.assign([], this.songFileDetails);
        this.songFileDetails = null;
        if (this.songsFilesDetailsService.songsFilesDetailsResponse == undefined)
        {
            console.log("Error in response");
        }
        else
        {
            tempDetails.push(this.songsFilesDetailsService.songsFilesDetailsResponse);
            console.log("Response OK");
        }

        this.songFileDetails = tempDetails;
    }
}