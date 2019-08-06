import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';


import { UserAddRequest, UserAddResponse } from './../../Models/User';

import { AddUserLoginService } from './../../Services/add-user-login.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
/** Login component*/
export class LoginComponent implements OnInit
{
    minNameLength = 4;
    minPw = 8;
    newUserFormGroup: FormGroup;

    public successString = '';
    public errorString = '';

    /** Login ctor */
    constructor(private formBuilder: FormBuilder, private addUserLoginService: AddUserLoginService)
    {
    }

    ngOnInit()
    {
        this.newUserFormGroup = this.formBuilder.group({
            newUserName: ['', [Validators.required, Validators.minLength(this.minNameLength)]],
            newUserEmail: ['', [Validators.required, Validators.email]],
            newUserDescription: [''],
            password: ['', [Validators.required, Validators.minLength(this.minPw)]],
            password2: ['', [Validators.required]]
        }, { validator: passwordMatchValidator });
    }

    /* Shorthands for form controls (used from within template) */
    get newUserName() { return this.newUserFormGroup.get('newUserName'); }
    get newUserEmail() { return this.newUserFormGroup.get('newUserEmail'); }
    get newUserDescription() { return this.newUserFormGroup.get('newUserDescription'); }
    get password() { return this.newUserFormGroup.get('password'); }
    get password2() { return this.newUserFormGroup.get('password2'); }

    /* Called on each input in either password field */
    onPasswordInput()
    {
        this.errorString = '';
        this.successString = '';
        if (this.newUserFormGroup.hasError('passwordMismatch'))
            this.password2.setErrors([{ 'passwordMismatch': true }]);
        else
            this.password2.setErrors(null);
    }


    onNewUserNameInput()
    { 
        this.errorString = '';
        this.successString = '';
    }

    onNewUserEmailInput()
    {
        // check if unique????
        this.errorString = '';
        this.successString = '';
    }

    onNewUserDescriptionInput()
    {
        // check if unique????
        this.errorString = '';
        this.successString = '';
    }

    public async onNewUserSubmitted()
    {
        console.log('onNewUserSubmitted -> newUserName : ', this.newUserFormGroup.value.newUserName);
        console.log('onNewUserSubmitted -> newUserEmail : ', this.newUserFormGroup.value.newUserEmail);
        console.log('onNewUserSubmitted -> newUserDescription : ', this.newUserFormGroup.value.newUserDescription);
        console.log('onNewUserSubmitted -> password : ', this.newUserFormGroup.value.password);

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
                this.successString = "Added New User Id: " + addResponse.userId;
                this.errorString = '';
                this.newUserFormGroup.reset();
                this.newUserFormGroup.clearValidators();
            }
            else
            {
                this.successString = '';
                this.errorString = addResponse.failReason;
            }

        }
    }

}

export const passwordMatchValidator: ValidatorFn = (formGroup: FormGroup): ValidationErrors | null =>
{
    if (formGroup.get('password').value === formGroup.get('password2').value)
        return null;
    else
        return { passwordMismatch: true };
};