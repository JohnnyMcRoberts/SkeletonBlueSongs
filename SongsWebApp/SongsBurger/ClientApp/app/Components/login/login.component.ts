import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';

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

    /** Login ctor */
    constructor(private formBuilder: FormBuilder)
    {
    }

    ngOnInit()
    {
        this.newUserFormGroup = this.formBuilder.group({
            newUserName: ['', [Validators.required, Validators.minLength(this.minNameLength)]],
            password: ['', [Validators.required, Validators.minLength(this.minPw)]],
            password2: ['', [Validators.required]]
        }, { validator: passwordMatchValidator });
    }

    /* Shorthands for form controls (used from within template) */
    get newUserName() { return this.newUserFormGroup.get('newUserName'); }
    get password() { return this.newUserFormGroup.get('password'); }
    get password2() { return this.newUserFormGroup.get('password2'); }

    /* Called on each input in either password field */
    onPasswordInput()
    {
        if (this.newUserFormGroup.hasError('passwordMismatch'))
            this.password2.setErrors([{ 'passwordMismatch': true }]);
        else
            this.password2.setErrors(null);
    }


    onNewUserNameInput()
    { 
        // check if unique????
    }

    onNewUserSubmitted()
    {
        console.log('onNewUserSubmitted -> newUserName : ', this.newUserFormGroup.value.newUserName);

    }

}

export const passwordMatchValidator: ValidatorFn = (formGroup: FormGroup): ValidationErrors | null =>
{
    if (formGroup.get('password').value === formGroup.get('password2').value)
        return null;
    else
        return { passwordMismatch: true };
};