import { OnInit, EventEmitter, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { DataModelService } from './../Services/data-model.service';
import { CurrentLoginService } from './../Services/current-login.service';

import { SongsValuesDetails } from './../Models/SongsFilesDetails';
import { AlbumPlayed } from './../Models/AlbumPlayed';
import { AutocompleteCategoryFormField, CategoricalValuePair, ICategoricalValuePair } from './../Models/AutocompleteCategoryFormField';


/** AddAlbum component*/
export abstract class BaseAlbumEdit implements OnInit, AfterViewInit
{
    /** AddAlbum ctor */
    constructor(
        public formBuilder: FormBuilder,
        public dataModelService: DataModelService,
        public currentLoginService: CurrentLoginService)
    {
        this.componentTitle = "Loading albums settings from database...";
        this.setupFormGroup();
    }

    public componentTitle: string;
    public songsValuesDetails: SongsValuesDetails;
    public newAlbumPlayed: AlbumPlayed = null;
    public newAlbumToDisplay: boolean = false;

    //#region Init Implementation

    ngOnInit()
    {
        this.dataModelService.fetchSongsValuesDetailsData().then(() =>
        {
            this.songsValuesDetails = SongsValuesDetails.fromData(this.dataModelService.songsValuesDetails);

            this.setupCategoryData();
        });

        this.ngOnInitAddition();
    }

    ngAfterViewInit()
    {
        this.ngAfterViewInitAddition();
    }

    //#endregion

    //#region Form Builder Setup

    addAlbumGroup: FormGroup;

    public locationForm: AutocompleteCategoryFormField;
    public artistForm: AutocompleteCategoryFormField;
    public albumForm: AutocompleteCategoryFormField;

    public setupFormGroup(): void
    {
        this.locationForm = new AutocompleteCategoryFormField(this.change);
        this.artistForm = new AutocompleteCategoryFormField(this.change);
        this.albumForm = new AutocompleteCategoryFormField(this.change, true);

        this.addAlbumGroup = this.formBuilder.group(
            {
                location: this.locationForm.form,
                artist: this.artistForm.form,
                album: this.albumForm.form,
                imageUrl: [''],
                playerUrl: [''],
                date: ['']
            }
        );
    }

    public setupCategoryData(): void
    {
        this.locationForm.setupCategoricalOptions(
            this.getAsCategoricalValuePairs(this.songsValuesDetails.locationValues));
        this.artistForm.setupCategoricalOptions(
            this.getAsCategoricalValuePairs(this.songsValuesDetails.artistValues));
        this.albumForm.setupCategoricalOptions(
            this.getAsCategoricalValuePairs(this.songsValuesDetails.albumValues));
    }

    public getAsCategoricalValuePairs(names: string[]): ICategoricalValuePair[]
    {
        var valuePairs: ICategoricalValuePair[] = new Array<CategoricalValuePair>();

        //var i: number = 0;
        for (let name of names)
        {
            let valuePair: CategoricalValuePair = new CategoricalValuePair(name, 1, [name]);
            valuePairs.push(valuePair);
        }

        return valuePairs;
    }

    public getNewAlbumPlayed(): AlbumPlayed
    {
        var albumPlayed: AlbumPlayed =
            AlbumPlayed.fromData(this.newAlbumPlayed);

        albumPlayed.date = new Date();

        if (!this.locationForm.form.pristine)
            albumPlayed.location = this.locationForm.optionValue;

        if (!this.artistForm.form.pristine)
            albumPlayed.artist = this.artistForm.optionValue;

        if (!this.albumForm.form.pristine)
            albumPlayed.album = this.albumForm.optionValue;

        if (!this.addAlbumGroup.get('imageUrl').pristine)
            albumPlayed.imagePath = this.addAlbumGroup.value.imageUrl;

        if (!this.addAlbumGroup.get('playerUrl').pristine)
            albumPlayed.playerLink = this.addAlbumGroup.value.playerUrl;

        albumPlayed.userName = this.currentLoginService.name;

        return albumPlayed;
    }

    public resetAutocompleteForm(categoryForm: AutocompleteCategoryFormField, categoricalValues: string[])
    {
        categoryForm.form.reset();
        categoryForm.form.clearValidators();
        categoryForm.form.clearAsyncValidators();
        categoryForm.form.markAsPristine();
        categoryForm.setupCategoricalOptions(this.getAsCategoricalValuePairs(categoricalValues));
    }

    public imageUrlValueChanged(event: any)
    {
        var imageUrl = this.addAlbumGroup.value.imageUrl;
        console.log("imageUrlValueChanged: " + imageUrl.toString());
        this.change.emit(event);
    }

    public playerUrlValueChanged(event: any)
    {
        var playerUrl = this.addAlbumGroup.value.playerUrl;
        console.log("playerUrlValueChanged: " + playerUrl.toString());
        this.change.emit(event);
    }

    public currentDate = new Date();
    public dateValueChanged(event: any)
    {
        var date = this.addAlbumGroup.value.date;
        console.log("dateValueChanged: " + date.toString());
        this.change.emit(event);
    }

    //#endregion

    //#region Page Control Handlers

    public onNewAlbumReset(): void
    {
        var imageForm = this.addAlbumGroup.get('imageUrl');
        imageForm.reset();
        imageForm.clearValidators();
        imageForm.clearAsyncValidators();
        imageForm.markAsPristine();

        var playerForm = this.addAlbumGroup.get('playerUrl');
        playerForm.reset();
        playerForm.clearValidators();
        playerForm.clearAsyncValidators();
        playerForm.markAsPristine();

        this.resetAutocompleteForm(this.locationForm, this.songsValuesDetails.locationValues);
        this.resetAutocompleteForm(this.artistForm, this.songsValuesDetails.artistValues);
        this.resetAutocompleteForm(this.albumForm, this.songsValuesDetails.albumValues);

        this.newAlbumToDisplay = false;
    }
    
    //#endregion

    //#region Abstract Elements

    //@Output() change = new EventEmitter();	
    public abstract change: EventEmitter<{}>;

    public abstract ngOnInitAddition();

    public abstract ngAfterViewInitAddition();

    //#endregion 
}