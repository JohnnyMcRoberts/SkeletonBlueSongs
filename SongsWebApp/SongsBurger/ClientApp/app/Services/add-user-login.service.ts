import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { UserAddRequest, UserAddResponse } from './../Models/User';

const httpOptions =
{
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable(({
    providedIn: 'root',
}) as any)
export class AddUserLoginService
{
    constructor(private http: HttpClient)
    {
        this.requestAddUserLoginUrl = 'api/Users/';
    }

    public requestAddUserLoginUrl: string;
    public addUserLoginResponse: any;
    async getAsyncUserAdd(request: UserAddRequest)
    {
        this.addUserLoginResponse =
            await this.http.post<UserAddResponse>(
                this.requestAddUserLoginUrl, request, httpOptions
                ).toPromise();

        console.log('No issues, waiting until promise is resolved...');
    }
}