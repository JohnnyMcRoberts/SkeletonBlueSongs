import { Component, Inject, Output, OnInit, ElementRef, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

import { FileUploader } from 'ng2-file-upload';

import * as FileSaver from 'file-saver';
import * as Jsontoxml from 'jsontoxml';

import { SongsFilesDetailsRequest, SongsFilesDetailsResponse } from './../../Models/SongsFilesDetails';
import { AlbumPlayed } from './../../Models/AlbumPlayed';
import { ExportText } from './../../Models/ExportText';

import { CurrentLoginService } from './../../Services/current-login.service';
import { FileUploadService } from './../../Services/file-upload.service';
import { DataModelService } from './../../Services/data-model.service';

export enum ExportFileType
{
    Text = "Text",
    CSV = "CSV",
    JSON = "JSON",
    XML = "XML"
};

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

    //#region Initialise Implementation

    public albumsPlayed: AlbumPlayed[] = new Array<AlbumPlayed>();

    ngOnInit()
    {
        this.dataModelService.fetchAllAlbumPlayedData().then(() =>
        {
            this.albumsPlayed = this.dataModelService.albumsPlayed;

            this.albumsPlayed = new Array<AlbumPlayed>();
            for (let album of this.dataModelService.albumsPlayed)
            {
                const newAlbum = AlbumPlayed.fromData(album);

                if (newAlbum.userName === this.currentLoginService.name)
                {
                    this.albumsPlayed.push(newAlbum);
                }
            }
        });
    }

    //#endregion

    //#region File Upload helper methods

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

    public async setFileSongsForUser(fileInfo: any, userId:string)
    {
        const file: File = fileInfo._file;

        console.log(file.name);

        if (this.uploadFileNameByGuid.has(file.name))
        {
            var fileNameGuid = this.uploadFileNameByGuid.get(file.name);

            this.dataModelService.getAllAlbumsPlayedFromFileToUser(fileNameGuid, userId).then(() =>
            {
                this.updateSelectedSongDetails(this.dataModelService.allAlbumsPlayedFromFileToUserResponse);
            });
        }
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

    public async onFileSetForUser()
    {
        await this.setFileSongsForUser(this.fileInfoLatest, this.currentLoginService.userId);
    }

    public async onDisplayExportData()
    {
        switch (this.selectedExportType)
        {
            case ExportFileType.CSV:
                {
                    this.displayText = this.getExportDataAsCsv();
                    this.exportDataToDisplay = true;
                }
                break;

            case ExportFileType.JSON:
                {
                    this.displayText = JSON.stringify(this.albumsPlayed, null, '\t');
                    this.exportDataToDisplay = true;
                }
                break;

            case ExportFileType.XML:
                {
                    this.displayText = this.getExportDataAsXml();
                    this.exportDataToDisplay = true;
                }
                break;

            case ExportFileType.Text:
                {
                    await this.getExportDataAsText();
                    this.exportDataToDisplay = true;
                }
                break;
        }

    }

    public async onExportDataToFile()
    {
        switch (this.selectedExportType)
        {
            case ExportFileType.CSV:
                {
                    this.displayText = this.getExportDataAsCsv();
                    this.exportDataToDisplay = true;
                    let blob = new Blob([this.displayText], { type: 'text/csv' });
                    FileSaver.saveAs(blob, "SongsPlayed.csv");
                }
                break;

            case ExportFileType.JSON:
                {
                    this.displayText = JSON.stringify(this.albumsPlayed, null, '\t');
                    this.exportDataToDisplay = true;
                    let blob = new Blob([this.displayText], { type: "application/json" });
                    FileSaver.saveAs(blob, "SongsPlayed.json");
                }
                break;

            case ExportFileType.XML:
                {
                    this.displayText = this.getExportDataAsXml();
                    this.exportDataToDisplay = true;
                    let blob = new Blob([this.displayText], { type: 'text/xml' });
                    FileSaver.saveAs(blob, "SongsPlayed.xml");
                }
                break;


            case ExportFileType.Text:
                {
                    this.dataModelService.fetchExportTextData(this.currentLoginService.userId).then(() =>
                    {
                        this.exportText = ExportText.fromData(this.dataModelService.exportText);
                        this.setDisplayText(this.exportText.formattedText);
                        this.exportDataToDisplay = true;
                        let blob = new Blob([this.displayText], { type: 'text/plain' });
                        FileSaver.saveAs(blob, "SongsPlayed.txt");
                    });
                }
                break;
        }

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

    //#region Export Options

    exportTypes: string[] =
    [
        ExportFileType.Text,
        ExportFileType.CSV,
        ExportFileType.JSON
    ];

    public selectedExportType: string;
    public exportDataToDisplay: boolean = false;
    public displayText: string = '';

    get optionSelected()
    {
        return this.selectedExportType != undefined
            && this.selectedExportType != null
            && this.selectedExportType !== '';
    }

    public getExportDataAsCsv(): string
    {
        const replacer = (key, value) => value === null ? '' : value; // specify how you want to handle null values here
        const header = Object.keys(this.albumsPlayed[0]);
        let csv = this.albumsPlayed.map(row => header.map(fieldName => JSON.stringify(row[fieldName], replacer)).join(','));
        csv.unshift(header.join(','));
        let csvArray = csv.join('\r\n');

        return csvArray;
    }

    public getExportDataAsXml(): string
    {
        let xmlString: string = "<albumsPlayed>\r\n";

        for (var i = 0; i < this.albumsPlayed.length; i++)
        {
            const album: AlbumPlayed = AlbumPlayed.fromData(this.albumsPlayed[i]);
            xmlString += "<albumPlayed>\r\n";

            const xml: any = Jsontoxml(album);

            const textString: string = xml.toString();
            xmlString += textString;

            xmlString += "\r\n</albumPlayed>\r\n";
        }

        xmlString += "</albumsPlayed>\r\n";

        return xmlString;
    }

    public exportText: ExportText;

    public async getExportDataAsText()
    {
        this.displayText = "Formatting....";

        this.dataModelService.fetchExportTextData(this.currentLoginService.userId).then(() =>
        {
            this.exportText = ExportText.fromData(this.dataModelService.exportText);
            this.setDisplayText(this.exportText.formattedText);
        });
    }

    public setDisplayText(exportText: any): void
    {
        this.exportDataToDisplay = false;
        this.displayText = exportText.toString();
        this.exportDataToDisplay = true;
    }


    //#endregion

}