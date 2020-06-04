import { Component, Input, SimpleChange, OnChanges } from '@angular/core';

import { AlbumPlayed } from './../../Models/AlbumPlayed';
import { SearchAlbum, SearchArtist } from './../../Models/SearchResults';

import { LastFmArtistService } from './../../Services/last-fm-artist.service';

@Component({
    selector: 'app-search-detail',
    templateUrl: './search-detail.component.html',
    styleUrls: ['./search-detail.component.scss']
})
/** SearchDetail component*/
export class SearchDetailComponent implements OnChanges
{
    /** SearchDetail ctor */
    constructor(
        private lastFmArtistService: LastFmArtistService) 
    {

    }

    // #region Input Data

    @Input()
    set artist(artist: string)
    {
        this._artist = artist;
        this.selectedAlbumPlayed.artist = artist;
        console.log("searchDetail  set artist= " + artist);
    }

    get artist(): string
    {
        return this._artist;
    }

    private _artist: string = "";

    @Input()
    set album(album: string)
    {
        this._album = album;
        this.selectedAlbumPlayed.album = album;
        console.log("searchDetail  set album= " + album);
    }

    get album(): string
    {
        return this._album;
    }

    private _album: string = "";

    ngOnChanges(changes: { [propKey: string]: SimpleChange; })
    {
        for (const propName of Object.keys(changes)) 
        {
            const changedProp = changes[propName];
            if (changedProp !== undefined && changedProp !== null)
            {
                this.onUpdateSearch();
            }
        }
    }

    // #endregion

    //#region Local data population

    public selectedAlbumPlayed: AlbumPlayed = new AlbumPlayed();

    public setAlbumPlayed(sheet: AlbumPlayed): void 
    {
        this.selectedAlbumPlayed = sheet;
    }

    // #endregion

    // #region Search Methods

    async onUpdateSearch() 
    {
        console.log("searchDetail.onUpdateSearch() - Artist = " + this.artist + " album = " + this.album);

        await this.onSearchForArtistAlbum();
    }

    //#endregion

    //#region Public Data

    public searchResult: boolean = false;
    public resultAsJson: string = "";

    public searchAlbum: SearchAlbum =
    {
        summary: '',
        image: '',
        url: '',
        mbid: '',
        artist: '',
        name: ''
    };

    public searchArtist: SearchArtist =
    {
        summary: '',
        image: '',
        url: '',
        mbid: '',
        name: '',
        listeners: 0,
        playcount: 0
    };

    //#endregion

    //#region Private Methods

    private searchAlbumLastFm() 
    {
        this.searchResult = true;

        this.lastFmArtistService.searchForAlbum(this.artist, this.album).subscribe((res: any) => 
        {
            this.resultAsJson = JSON.stringify(res.album, null, 2);

            this.searchAlbum.mbid = res.album.mbid;
            this.searchAlbum.artist = res.album.artist;
            this.searchAlbum.name = res.album.name;
            this.searchAlbum.image = res.album.image[3]['#text'];
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
                this.searchAlbum.summary = '';
            }

        });

    }

    private async asyncSearchArtistLastFm() 
    {
        this.searchResult = true;

        await this.lastFmArtistService.asyncSearchForArtist(this.artist, 'getinfo');

        const res = this.lastFmArtistService.searchForArtistResponse;

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
    }

    //#endregion

    //#region Public control handlers

    public async onSearchForArtistAlbum()
    {
        console.log('onSearchForArtistAlbum button press');

        await this.asyncSearchArtistLastFm();

        this.searchAlbumLastFm();
    }

    //#endregion
}