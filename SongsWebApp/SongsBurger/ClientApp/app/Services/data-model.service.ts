import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { AlbumPlayed } from './../Models/AlbumPlayed';


import { SongsFilesDetailsRequest, SongsFilesDetailsResponse } from './../Models/SongsFilesDetails';

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
    fetchAllSongsFromFileData(key: string) {
        return this.http.get<SongsFilesDetailsResponse>(this.requestUrl + "GetAllAlbumsPlayedFromFile/" + key)
            .toPromise().then(result => {
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

}