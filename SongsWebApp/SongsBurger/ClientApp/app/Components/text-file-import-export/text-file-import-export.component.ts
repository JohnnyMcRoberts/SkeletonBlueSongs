import { Component, Inject, Output, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

import { SongsFilesDetailsRequest, SongsFilesDetailsResponse } from './../../Models/SongsFilesDetails';

import { CurrentLoginService } from './../../Services/current-login.service';
import { FileUploadService } from './../../Services/file-upload.service';
import { SongsFilesDetailsService } from './../../Services/songs-files-details.service';
import { DataModelService } from './../../Services/data-model.service';

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
        private songsFilesDetailsService: SongsFilesDetailsService,
        private dataModelService: DataModelService
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
    public onFileSubmitted(fileInfo: any)
    {
        const file: File = fileInfo._file;
        this.visibilityHash[file.name] = true;
        this.savedFilesHash[file.name] = false;
        console.log(file.name);

        this.uploadFileName = file.name;
        var fileNameGuid = this.getNewGuid();
        this.fileUploadService.upload(fileNameGuid, [file]);
        this.uploadFileNameByGuid.set(this.uploadFileName, fileNameGuid);
    }

    public uploadFileNameByGuid: Map<string, string> = new  Map<string, string>();

    // File calculate details.
    public songFileDetails: Array<any>;

    public async onGetDetails(fileInfo: any)
    {
        const file: File = fileInfo._file;

        console.log(file.name);

        if (this.uploadFileNameByGuid.has(file.name))
        {
            var fileNameGuid = this.uploadFileNameByGuid.get(file.name);

            this.dataModelService.fetchAllSongsFromFileData(fileNameGuid).then(() =>
            {
                this.updateSelectedSongDetails(this.dataModelService.albumsPlayedResponse);
            });
        }
    }

    public songsFilesDetailsResponse: SongsFilesDetailsResponse = null;
    public songsFilesDetailsResponseRxed: boolean = false;
    public updateSelectedSongDetails(songsFilesDetailsResponse: any)
    {
        this.songsFilesDetailsResponseRxed = true;
        if (songsFilesDetailsResponse == undefined)
        {
            console.log("Error in response");
        }
        else
        {
            this.songsFilesDetailsResponse = SongsFilesDetailsResponse.fromData(songsFilesDetailsResponse);
            console.log("Response OK");
        }
    }


    public getNewGuid(): string
    {
        var result: string;
        var i: string;
        var j: number;

        result = "";
        for (j = 0; j < 32; j++)
        {
            if (j === 8 || j === 12 || j === 16 || j === 20)
            {
                 result = result + '-';
            }

            i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
            result = result + i;
        }
        return result;
    }
}