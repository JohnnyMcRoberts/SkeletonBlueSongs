import { Component } from '@angular/core';

import { DataModelService } from './../../Services/data-model.service';
import { CurrentLoginService } from './../../Services/current-login.service';
import { LastFmArtistService } from './../../Services/last-fm-artist.service';


export interface SearchArtist {
    mbid: string;
    name: string;
    summary: string;
    listeners: number;
    playcount: number;
    url: string;
    image: string;
}


export interface SearchAlbum {
    mbid: string;
    name: string;
    artist: string;
    summary: string;
    image: string;
    url: string;
}

export interface AlbumReleasePair 
{
    artist: string;
    album: string;
}


@Component({
    selector: 'app-find-details',
    templateUrl: './find-details.component.html',
    styleUrls: ['./find-details.component.scss']
})
/** FindDetails component*/
export class FindDetailsComponent
{
    /** FindDetails ctor */
    constructor(
        private dataModelService: DataModelService, 
        private currentLoginService: CurrentLoginService, 
        private lastFmArtistService: LastFmArtistService) 
    {
        this.searchArtistStr = this.artistName;
        this.searchAlbumStr = this.albumName;
    }

    //#region public data


    searchArtistStr: string;
    searchAlbumStr: string;


    public selectedTemplateName: string = "New Template";
    public newTemplateIndex: number = 0;
    public artistName: string = "Pharoah Sanders";
    public albumName: string = "Thembi";

    public artistNames: string[] = ["Pharoah Sanders", "Ride","Cher", "The Fall", "Tim Rose", "King Creosote"];

    public albums: AlbumReleasePair[]=
        [
            { artist: "Pharoah Sanders", album: "Thembi" },
            { artist: "Patti Smith", album: "Horses" },
            { artist: "New Order", album: "Technique" },
            { artist: "The Fall", album: "Dragnet" },
            { artist: "Townes Van Zandt", album: "Delta Momma Blues" },
            { artist: "Tinariwen", album: "Elwan" },
        ];

    public resultAsJson: string = "";

    public searchArtistResponse: any;
    
    searchArtist: SearchArtist =
    {
        summary: '',
        image: '',
        url: '',
        mbid: '',
        name: '',
        listeners: 0,
        playcount: 0
    };

    imagesBySize: Map<string, string> = null;

    searchResult : boolean = false;

    searchMusicBrainzUrl = '';

    searchAlbum: SearchAlbum =
    {
        summary: '',
        image: '',
        url: '',
        mbid: '',
        artist: '',
        name: ''
    };

    //#endregion

    //#region Private Methods

    private searchAlbumLastFm()
    {
        this.searchResult = true;

        this.lastFmArtistService.searchForAlbum(this.artistName, this.albumName).subscribe((res: any) =>
        {
            this.resultAsJson = JSON.stringify(res.album, null, 2);

            this.searchAlbum.mbid = res.album.mbid;
            this.searchAlbum.artist = res.album.artist;
            this.searchAlbum.name = res.album.name;
            this.searchAlbum.image = res.album.image[2]['#text'];
            this.searchAlbum.url = res.album.url;

            if (res.album.wiki !== undefined &&
                res.album.wiki !== null &&
                res.album.wiki.summary !== undefined &&
                res.album.wiki.summary !== null) 
            {
                this.searchAlbum.summary = res.album.wiki.summary;
            }
            else if (this.searchArtist.summary !== undefined && this.searchArtist.summary !== null)
            {
                this.searchAlbum.summary = this.searchArtist.summary;
            } 
            else 
            {
                this.searchAlbum.summary ='';
            }

        });

    }

    private async asyncSearchArtistLastFm()
    {
        this.searchResult = true;

        await this.lastFmArtistService.asyncSearchForArtist(this.artistName, 'getinfo');

        var res = this.lastFmArtistService.searchForArtistResponse;

        if (res === undefined || res === null) 
        {
            console.log("Error in response");
        } 
        else 
        {
            console.log("Response OK");
            this.searchArtist.name = res.artist.name;
            this.searchArtist.image = res.artist.image[2]['#text'];
            this.searchArtist.listeners = res.artist.stats.listeners;
            this.searchArtist.playcount = res.artist.stats.playcount;
            this.searchArtist.summary = res.artist.bio.summary;
            this.searchArtist.mbid = res.artist.mbid;
            this.searchArtist.url = res.artist.url;
        }

        //this.lastFmArtistService.searchForArtist(this.artistName, 'getinfo').subscribe((res: any) => {
        //    this.searchArtist.name = res.artist.name;
        //    this.searchArtist.image = res.artist.image[2]['#text'];
        //    this.searchArtist.listeners = res.artist.stats.listeners;
        //    this.searchArtist.playcount = res.artist.stats.playcount;
        //    this.searchArtist.summary = res.artist.bio.summary;
        //    this.searchArtist.mbid = res.artist.mbid;
        //    this.searchArtist.url = res.artist.url;
        //});

    }

    //#endregion

    //#region Public control handlers

    public async onSearchForArtistAlbum()
    {
        console.log('onSearchForArtistAlbum button press');
        //this.newTemplateIndex++;
        //this.artistName = this.albums[this.newTemplateIndex % this.artistNames.length].artist;
        //this.albumName = this.albums[this.newTemplateIndex % this.artistNames.length].album;


         this.artistName = this.searchArtistStr;
         this.albumName = this.searchAlbumStr;

        this.selectedTemplateName = "New Template #" + this.newTemplateIndex.toString();

        //this.searchArtistLastFm();

        await this.asyncSearchArtistLastFm();

        this.searchAlbumLastFm();

    }

    //#endregion


}