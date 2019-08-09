import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpRequest, HttpEventType } from '@angular/common/http'

const httpOptions =
{
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class FileUploadService
{
    constructor(http: HttpClient)
    {
        this.http = http;
        this.requestUrl = 'api/FileUpload/';
    }

    public progress: number;
    public message: string;

    public http: HttpClient;
    public requestUrl: string;

    public upload(id: string, files: File[])
    {
        if (files.length === 0)
            return;

        const formData = new FormData();

        for (let file of files)
        {
            formData.append(file.name, file);
        }

        let url: string = this.requestUrl + id.toString();
        const uploadReq = new HttpRequest('POST', url, formData,
            {
                reportProgress: true,
            });

        this.http.request(uploadReq).subscribe(event =>
        {
            if (event.type === HttpEventType.UploadProgress)
                this.progress = Math.round(100 * event.loaded / event.total);
            else if (event.type === HttpEventType.Response)
                this.message = event.body.toString();
        });
    }



    public uploadFileResponse: any;
    async asyncUploadFile(id: string, files: File[])
    {
        if (files.length === 0)
            return;

        const formData = new FormData();

        for (let file of files)
        {
            formData.append(file.name, file);
        }

        let url: string = this.requestUrl + id.toString();
        const uploadReq = new HttpRequest('POST', url, formData,
            {
                reportProgress: true,
            });


        await this.http.request(uploadReq).toPromise();

        console.log('asyncUploadFile: No issues, waiting until promise is resolved...');
    }


}