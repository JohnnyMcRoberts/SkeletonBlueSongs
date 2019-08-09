import { Component, Inject, Output, OnInit, ElementRef, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

import { FileUploader } from 'ng2-file-upload';

import { SongsFilesDetailsRequest, SongsFilesDetailsResponse } from './../../Models/SongsFilesDetails';
import { AlbumPlayed } from './../../Models/AlbumPlayed';

import { CurrentLoginService } from './../../Services/current-login.service';
import { FileUploadService } from './../../Services/file-upload.service';
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
        private dataModelService: DataModelService
        )
    {
        this.uploader = new FileUploader({
            url: "URL",
            disableMultipart: true
        });

    }
    
    @ViewChild('fileInput') fileInput: any;

    //#region Local data

    public uploader: FileUploader;

    public fileInfoLatest: any;

    public fileIsSelected: boolean = false;
    public fileIsUploaded: boolean = false;
    public songsFilesDetailsResponseRxed: boolean = false;
    public isSubmitted: boolean;

    public uploadFileName: string;
    public visibilityHash: IHash = {};
    public savedFilesHash: IHash = {};
    public uploadFileNameByGuid: Map<string, string> = new Map<string, string>();

    public songFileDetails: Array<any>;
    public songsFilesDetailsResponse: SongsFilesDetailsResponse = null;

    //#endregion

    //#region File Upload helper methods

    public getGuidForSubmittedFile(fileInfo: any): string
    {
        const file: File = fileInfo._file;
        this.visibilityHash[file.name] = true;
        this.savedFilesHash[file.name] = false;
        console.log(file.name);

        this.uploadFileName = file.name;
        var fileNameGuid: string = this.getNewGuid();
        return fileNameGuid;
    }

    public onFileSubmitted(fileInfo: any)
    {
        const file: File = fileInfo._file;
        var fileNameGuid = this.getGuidForSubmittedFile(fileInfo);
        this.fileUploadService.upload(fileNameGuid, [file]);
        this.uploadFileNameByGuid.set(this.uploadFileName, fileNameGuid);
    }

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

            
            this.fileIsUploaded = true;

            this.setupDataTable(songsFilesDetailsResponse.albumsPlayed);
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

    //#endregion

    //#region Page Control Handlers

    public selectedFile()
    {
        this.uploader.uploadAll();
        var queueLength = this.uploader.queue.length;
        if (queueLength > 0)
        {
            this.fileInfoLatest = this.uploader.queue[queueLength - 1];
            this.fileIsSelected = true;
        }
    }

    public async onSelectedFileSubmitted()
    {
        var fileInfo: any = this.fileInfoLatest;
        const file: File = fileInfo._file;
        var fileNameGuid = this.getGuidForSubmittedFile(fileInfo);

        await this.fileUploadService.asyncUploadFile(fileNameGuid, [file]);

        this.uploadFileNameByGuid.set(this.uploadFileName, fileNameGuid);
        await this.onGetDetails(this.fileInfoLatest);
    }


    public onFileReset(): void
    {
        this.fileInput.nativeElement.value = '';
        this.songsFilesDetailsResponse = null;
        this.songsFilesDetailsResponseRxed = false;
        this.fileIsSelected = false;
        this.fileIsUploaded = false;
    }

    public onFileSetForUser(): void
    {

    }

    //#endregion


    //#region Data Table implementation

    @ViewChild('itemsTablePaginator') public itemsTablePaginator: MatPaginator;
    @ViewChild('itemsTableSort') public itemsTableSort: MatSort;

    public setupDataTable(albumsData: AlbumPlayed[]): void
    {
        this.items = albumsData;
        this.itemsDataSource = new MatTableDataSource(this.items);
        this.setupItemsPagingAndSorting();
    }


    public items: any[];
    public itemsDisplayedColumns: string[] = this.getItemsDisplayedColumns();
    public itemsDataSource: MatTableDataSource<any>;

    public getItemsDisplayedColumns(): string[]
    {
        var columns =
        [
            'date',
            'location',
            'artist',
            'album'
        ];

        return columns;
    }

    public setupItemsPagingAndSorting(): void
    {
        if (this.items != null)
        {
            setTimeout(() =>
            {
                this.itemsDataSource.paginator = this.itemsTablePaginator;
                this.itemsDataSource.sort = this.itemsTableSort;
                this.itemsTableSort.sortChange.subscribe(() =>
                {
                    this.itemsTablePaginator.pageIndex = 0;
                    this.itemsTablePaginator._changePageSize(this.itemsTablePaginator.pageSize);
                });
            });
        }
    }

    public applyItemsFilter(filterValue: string)
    {
        this.itemsDataSource.filter = filterValue.trim().toLowerCase();

        if (this.itemsDataSource.paginator)
        {
            this.itemsTablePaginator.pageIndex = 0;
            this.itemsTablePaginator._changePageSize(this.itemsTablePaginator.pageSize);
        }
    }


    //#endregion


}