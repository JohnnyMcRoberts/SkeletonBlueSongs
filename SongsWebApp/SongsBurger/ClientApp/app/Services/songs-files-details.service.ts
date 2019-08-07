import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { SongsFilesDetailsRequest, SongsFilesDetailsResponse } from './../Models/SongsFilesDetails';

const httpOptions =
{
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable()
export class SongsFilesDetailsService 
{
    constructor(private http: HttpClient)
    {
        this.requestUrl = 'api/SongsFilesDetails/';
    }

    public requestUrl: string;
    public songsFilesDetailsResponse: any;

    async getAsyncSongsFilesDetailsData(cadDetailsRequest: SongsFilesDetailsRequest)
    {
        this.songsFilesDetailsResponse =
            await this.http.post<SongsFilesDetailsResponse>(
                this.requestUrl, cadDetailsRequest, httpOptions).toPromise();

        console.log('No issues, waiting until promise is resolved...');
    }
}