﻿import { Injectable } from '@angular/core';

import { UserAddRequest, UserAddResponse, UserLogin } from './../Models/User';

@Injectable(({
    providedIn: 'root',
}) as any)
export class CurrentLoginService
{
    constructor()
    {
        this.logout();
    }

    public isLoggedIn: boolean = false;
    public name: string = "";
    public description: string = "";
    public email: string = "";
    public userId: string = "";

    public logout(): void
    {
        this.isLoggedIn = false;
        this.name = "";
        this.description = "";
        this.email = "";
        this.userId = "";
    }

    public login(userLogin: UserLogin): void
    {
        this.isLoggedIn = true;
        this.name = userLogin.name;
        this.description = userLogin.description;
        this.email = userLogin.email;
        this.userId = userLogin.userId;
    }
}