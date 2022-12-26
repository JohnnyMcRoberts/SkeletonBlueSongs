import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { UserLoginRequest, UserLoginResponse } from './../Models/User';
import { environment } from '../../environments/environment';

const httpOptions =
{
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable(({
    providedIn: 'root',
}) as any)
export class LoginService
{
    constructor(private http: HttpClient)
    {
        this.requestUserLoginUrl = environment.baseApi + 'api/UserLogin/';
    }

    public requestUserLoginUrl: string;
    public userLoginResponse: any;

    async asyncUserLogin(request: UserLoginRequest)
    {
        this.userLoginResponse =
            await this.http.post<UserLoginResponse>(
                this.requestUserLoginUrl, request, httpOptions
                ).toPromise();

        console.log('asyncUserLogin: No issues, waiting until promise is resolved...');
    }
}