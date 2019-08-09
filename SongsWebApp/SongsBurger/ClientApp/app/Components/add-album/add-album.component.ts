import { Component, OnInit, Output, EventEmitter, AfterViewInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { DataModelService } from './../../Services/data-model.service';
import { CurrentLoginService } from './../../Services/current-login.service';

import { SongsValuesDetails } from './../../Models/SongsFilesDetails';
import { AlbumPlayed } from './../../Models/AlbumPlayed';
import { AutocompleteCategoryFormField, CategoricalValuePair, ICategoricalValuePair } from './../../Models/AutocompleteCategoryFormField';


@Component({
    selector: 'app-add-album',
    templateUrl: './add-album.component.html',
    styleUrls: ['./add-album.component.scss']
})
/** AddAlbum component*/
export class AddAlbumComponent implements OnInit, AfterViewInit
{
    @Output() change = new EventEmitter();

    /** AddAlbum ctor */
    constructor(
        public formBuilder: FormBuilder,
        private dataModelService: DataModelService,
        private currentLoginService: CurrentLoginService)
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
    }

    ngAfterViewInit()
    {
        //if (this.albumsPlayed !== undefined) {
        //    this.setupDataTable(this.albumsPlayed);
        //}
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
                album: this.albumForm.form
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
        var albumPlayed: AlbumPlayed = new AlbumPlayed();

        albumPlayed.date = new Date();
        albumPlayed.date.setHours(12, 0, 0);

        albumPlayed.location = this.locationForm.optionValue;
        albumPlayed.artist = this.artistForm.optionValue;
        albumPlayed.album = this.albumForm.optionValue;
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

    //#endregion

    //#region Page Control Handlers

    public onAddNewAlbum(): void
    {
        //this.fileInput.nativeElement.value = '';
        //this.songsFilesDetailsResponse = null;
        //this.songsFilesDetailsResponseRxed = false;
        //this.fileIsSelected = false;
        //this.fileIsUploaded = false;
    }

    public onNewAlbumReset(): void
    {
        this.resetAutocompleteForm(this.locationForm, this.songsValuesDetails.locationValues);
        this.resetAutocompleteForm(this.artistForm, this.songsValuesDetails.artistValues);
        this.resetAutocompleteForm(this.albumForm, this.songsValuesDetails.albumValues);

        this.newAlbumToDisplay = false;
    }

    public onNewAlbumDisplay(): void
    {
        this.newAlbumPlayed = this.getNewAlbumPlayed();
        this.newAlbumToDisplay = true;
    }

    //#endregion
}