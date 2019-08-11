import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { AlbumPlayed, AlbumPlayedAddResponse } from './../Models/AlbumPlayed';

import { SongsValuesDetails, SongsFilesDetailsResponse } from './../Models/SongsFilesDetails';
import { ExportText } from './../Models/ExportText';

const httpOptions =
{
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class DataModelService
{
    constructor(private http: HttpClient)
    {
        this.requestUrl = 'api/DataModel/';
    }

    public requestUrl: string;

    public albumsPlayed: AlbumPlayed[];
    fetchAllAlbumPlayedData()
    {
        return this.http.get<AlbumPlayed[]>(this.requestUrl + "GetAllAlbumsPlayed")
            .toPromise().then(result => {
                this.albumsPlayed = result as AlbumPlayed[];
                },
                error => console.error(error));
    }

    public albumsPlayedResponse: SongsFilesDetailsResponse;
    fetchAllSongsFromFileData(key: string)
    {
        return this.http.get<SongsFilesDetailsResponse>(this.requestUrl + "GetAllAlbumsPlayedFromFile/" + key)
            .toPromise().then(result =>
                {
                    this.albumsPlayedResponse = result as SongsFilesDetailsResponse;
                },
                error => console.error(error));
    }

    public songsFilesDetailsResponse: any;
    async getAsyncSongsFromFile(key: string)
    {
        this.songsFilesDetailsResponse =
            await this.http.get(this.requestUrl + "GetAllAlbumsFromFile/" + key, httpOptions).toPromise();

        console.log('No issues, waiting until promise is resolved...');
    }

    public allAlbumsPlayedFromFileToUserResponse: any;
    async getAllAlbumsPlayedFromFileToUser(fileKey: string, userId: string)
    {
        return this.http.get<SongsFilesDetailsResponse>(
            this.requestUrl + "GetAllAlbumsPlayedFromFileToUser/" + fileKey + "/" + userId)
            .toPromise().then(result =>
                {
                   this.allAlbumsPlayedFromFileToUserResponse = result as SongsFilesDetailsResponse;
                },
                error => console.error(error));
    }

    public songsValuesDetails: SongsValuesDetails;
    fetchSongsValuesDetailsData()
    {
        return this.http.get<SongsValuesDetails>(this.requestUrl + "GetSongsValuesDetails")
            .toPromise().then(result =>
                {
                    this.songsValuesDetails = result as SongsValuesDetails;
                },
                error => console.error(error));
    }

    public exportText: ExportText;
    fetchExportTextData(userId: string)
    {
        return this.http.get<ExportText>(this.requestUrl + "GetExportText/" + userId)
            .toPromise().then(result =>
                {
                    this.exportText = result as ExportText;
                },
                error => console.error(error));
    }

    public addUserLoginResponse: any;
    async addAsyncAlbumPlayed(request: AlbumPlayed)
    {
        this.addUserLoginResponse =
                await this.http.post<AlbumPlayedAddResponse>(
            this.requestUrl, request, httpOptions
            ).toPromise();

        console.log('No issues, waiting until promise is resolved...');
    }


    public updateAlbumPlayedResponse: any;
    async  updateAsyncAlbumPlayed(album: AlbumPlayed)
    {
        this.updateAlbumPlayedResponse =
            await this.http.put<AlbumPlayedAddResponse>(
                this.requestUrl, album, httpOptions
            ).toPromise();

        console.log('No issues, waiting until promise is resolved...');
    }


    public deleteAlbumPlayedResponse: any;
    async  deleteAsyncAlbumPlayed(album: AlbumPlayed) {
        this.deleteAlbumPlayedResponse =
            await this.http.delete<AlbumPlayedAddResponse>(
                this.requestUrl + "/" + album.id, httpOptions
            ).toPromise();

        console.log('No issues, waiting until promise is resolved...');
    }


    private pad(number)
    {
        if (number < 10)
        {
            return '0' + number;
        }

        return number;
    }

    private dateToIso(date: Date): string
    {
        return date.getUTCFullYear() +
            this.pad(date.getUTCMonth() + 1) +
            this.pad(date.getUTCDate()) +
            'T' +
            this.pad(date.getUTCHours()) +
            this.pad(date.getUTCMinutes()) +
            'Z';
    }

    public songsReportDetails: AlbumPlayed[];
    fetchSongsReportDetailsData(startDate:Date, endDate:Date, userId: string)
    {
        var startString = this.dateToIso(startDate);
        var endString = this.dateToIso(endDate);

        return this.http.get <AlbumPlayed[]>(
            this.requestUrl + "GetSongsReportDetails"
            + "/" + startString + "/" + endString + "/" + userId)
            .toPromise().then(result =>
            {
                this.songsReportDetails = result as AlbumPlayed[];
            },
                error => console.error(error));
    }
}