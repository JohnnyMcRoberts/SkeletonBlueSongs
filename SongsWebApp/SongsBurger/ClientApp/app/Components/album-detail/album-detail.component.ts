import { Component, Input, OnChanges, SimpleChange } from '@angular/core';
import { AlbumPlayed } from './../../Models/AlbumPlayed';

@Component({
    selector: 'app-album-detail',
    templateUrl: './album-detail.component.html',
    styleUrls: ['./album-detail.component.scss']
})
/** AlbumDetail component*/
export class AlbumDetailComponent implements OnChanges
{
    /** AlbumDetail ctor */
    constructor()
    {

    }

    //#region OnChanges implementation

    @Input() albumPlayed: AlbumPlayed;

    public numberLoaded: string = "None";
    private updates: number = 0;

    ngOnChanges(changes: { [propKey: string]: SimpleChange })
    {
        for (let propName in changes)
        {
            if (changes.hasOwnProperty(propName))
            {
                let changedProp = changes[propName];
                var currentValue = changedProp.currentValue;

                this.updates++;
                if (currentValue != null && propName === "albumPlayed")
                {
                    this.setAlbumPlayed((currentValue as AlbumPlayed));
                }
            }
        }
    }

    //#endregion

    //#region Local data population

    public selectedAlbumPlayed: AlbumPlayed = null;

    public setAlbumPlayed(sheet: AlbumPlayed): void
    {
        this.selectedAlbumPlayed = sheet;
    }

    //#endregion

    //#region Page Handlers

    public openDisplayLink()
    {
        window.open(this.selectedAlbumPlayed.playerLink, "_blank");
    }

    //#endregion

}