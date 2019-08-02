import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { AlbumPlayed } from './../Models/AlbumPlayed';


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

}