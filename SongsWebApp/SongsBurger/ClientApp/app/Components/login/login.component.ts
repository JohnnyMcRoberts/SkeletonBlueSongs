﻿import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';

import { UserAddRequest, UserAddResponse, UserLoginRequest, UserLoginResponse, UserLogin } from './../../Models/User';

import { AddUserLoginService } from './../../Services/add-user-login.service';
import { CurrentLoginService } from './../../Services/current-login.service';
import { LoginService } from './../../Services/login.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
/** Login component*/
export class LoginComponent implements OnInit
{
    /** Login ctor */
    constructor(
        private formBuilder: FormBuilder,
        private addUserLoginService: AddUserLoginService,
        private currentLoginService: CurrentLoginService,
        private loginService: LoginService)
    {
    }

    //#region OnInit Implementation

    ngOnInit()
    {
        this.setupForGroups();
    }

    //#endregion

    //#region Form Group Data setup

    minNameLength = 4;
    minPasswordLength = 8;

    public newUserFormGroup: FormGroup;
    public existingUserFormGroup: FormGroup;

    public setupForGroups(): void
    {
        this.newUserFormGroup = this.formBuilder.group({
            newUserName: ['', [Validators.required, Validators.minLength(this.minNameLength)]],
            newUserEmail: ['', [Validators.required, Validators.email]],
            newUserDescription: [''],
            password: ['', [Validators.required, Validators.minLength(this.minPasswordLength)]],
            password2: ['', [Validators.required]]
        }, { validator: passwordMatchValidator });

        this.existingUserFormGroup = this.formBuilder.group({
            existingUserName: ['', [Validators.required]],
            existingUserPassword: ['', [Validators.required]]
        });
    }

    public clearFormGroupControl(formGroup: FormGroup, controlName: string): void
    {
        formGroup.get(controlName).reset();
        formGroup.get(controlName).markAsPristine();
        formGroup.get(controlName).clearValidators();
    }

    //#endregion

    //#region New User Form data and processing

    public addUserLoginSuccessString = '';
    public addUserLoginErrorString = '';

    /* Shorthands for form controls (used from within template) */
    get newUserName() { return this.newUserFormGroup.get('newUserName'); }
    get newUserEmail() { return this.newUserFormGroup.get('newUserEmail'); }
    get newUserDescription() { return this.newUserFormGroup.get('newUserDescription'); }
    get password() { return this.newUserFormGroup.get('password'); }
    get password2() { return this.newUserFormGroup.get('password2'); }

    /* Called on each input in either password field */
    onNewPasswordInput()
    {
        this.addUserLoginErrorString = '';
        this.addUserLoginSuccessString = '';
        if (this.newUserFormGroup.hasError('passwordMismatch'))
            this.password2.setErrors([{ 'passwordMismatch': true }]);
        else
            this.password2.setErrors(null);
    }

    onNewUserNameInput()
    {
        this.addUserLoginErrorString = '';
        this.addUserLoginSuccessString = '';
    }

    onNewUserEmailInput()
    {
        // check if unique????
        this.addUserLoginErrorString = '';
        this.addUserLoginSuccessString = '';
    }

    onNewUserDescriptionInput()
    {
        // check if unique????
        this.addUserLoginErrorString = '';
        this.addUserLoginSuccessString = '';
    }

    public async onNewUserSubmitted()
    {
        console.log('onNewUserSubmitted -> newUserName : ', this.newUserFormGroup.value.newUserName);

        var addUserReq: UserAddRequest =
            new UserAddRequest(this.newUserFormGroup.value.newUserName,
                this.newUserFormGroup.value.password,
                this.newUserFormGroup.value.newUserDescription,
                this.newUserFormGroup.value.newUserEmail);

        await this.addUserLoginService.getAsyncUserAdd(addUserReq);

        var resp = this.addUserLoginService.addUserLoginResponse;

        if (resp == undefined)
        {
            console.log("Error in response");
        }
        else
        {
            console.log("Response OK");

            var addResponse: UserAddResponse = UserAddResponse.fromData(resp);

            if (addResponse.errorCode === 0)
            {
                this.addUserLoginSuccessString = "Added New User Id: " + addResponse.userId;
                this.addUserLoginErrorString = '';

                var userLogin: UserLogin =
                    new UserLogin(
                        this.newUserFormGroup.value.newUserName,
                        this.newUserFormGroup.value.newUserDescription,
                        this.newUserFormGroup.value.newUserEmail,
                        addResponse.userId);

                this.currentLoginService.login(userLogin);

                this.newUserFormGroup.reset();
                this.newUserFormGroup.clearValidators();
                this.newUserFormGroup.markAsPristine();
                this.clearFormGroupControl(this.newUserFormGroup, 'newUserName');
                this.clearFormGroupControl(this.newUserFormGroup, 'newUserEmail');
                this.clearFormGroupControl(this.newUserFormGroup, 'newUserDescription');
                this.clearFormGroupControl(this.newUserFormGroup, 'password');
                this.clearFormGroupControl(this.newUserFormGroup, 'password2');
            }
            else
            {
                this.addUserLoginSuccessString = '';
                this.addUserLoginErrorString = addResponse.failReason;
            }
        }
    }

    //#endregion

    //#region Existing User Login Form data and processing

    public existingUserLoginSuccessString = '';
    public existingUserLoginErrorString = '';

    /* Shorthands for form controls (used from within template) */
    get existingUserName() { return this.existingUserFormGroup.get('existingUserName'); }
    get existingUserPassword() { return this.existingUserFormGroup.get('existingUserPassword'); }

    onExistingUserNameInput()
    {
        this.existingUserLoginErrorString = '';
        this.existingUserLoginSuccessString = '';
    }

    onExistingPasswordInput()
    {
        this.existingUserLoginErrorString = '';
        this.existingUserLoginSuccessString = '';
    }

    public async onExistingUserSubmitted()
    {
        console.log('onExistingUserSubmitted -> Name : ', this.existingUserFormGroup.value.existingUserName);

        var userLoginReq: UserLoginRequest =
            new UserLoginRequest(this.existingUserFormGroup.value.existingUserName, this.existingUserFormGroup.value.existingUserPassword);

        await this.loginService.asyncUserLogin(userLoginReq);

        var resp = this.loginService.userLoginResponse;

        if (resp == undefined)
        {
            console.log("Error in response");
        }
        else
        {
            console.log("Response OK");

            var loginResponse: UserLoginResponse = UserLoginResponse.fromData(resp);

            if (loginResponse.errorCode === 0)
            {
                this.existingUserLoginSuccessString = "Logged in successfully with User Id: " + loginResponse.userId;
                this.existingUserLoginErrorString = '';
                //this.existingUserFormGroup.existingUserName;

                var userLogin: UserLogin =
                    new UserLogin(
                        loginResponse.name,
                        loginResponse.description,
                        loginResponse.email,
                        loginResponse.userId);

                this.currentLoginService.login(userLogin);

                this.existingUserFormGroup.reset();
                this.existingUserFormGroup.clearValidators();
                this.existingUserFormGroup.markAsPristine();
                this.clearFormGroupControl(this.existingUserFormGroup, 'existingUserName');
                this.clearFormGroupControl(this.existingUserFormGroup, 'existingUserPassword');
            }
            else
            {
                this.existingUserLoginSuccessString = '';
                this.existingUserLoginErrorString = loginResponse.failReason;
            }
        }
    }

    public onExistingUserReset()
    {
        this.existingUserLoginSuccessString = '';
    }

    //#endregion

}

export const passwordMatchValidator: ValidatorFn = (formGroup: FormGroup): ValidationErrors | null =>
{
    if (formGroup.get('password').value === formGroup.get('password2').value)
        return null;
    else
        return { passwordMismatch: true };
};