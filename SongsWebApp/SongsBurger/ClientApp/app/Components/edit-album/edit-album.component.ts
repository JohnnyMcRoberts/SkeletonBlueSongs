import { Component, OnInit, Output, EventEmitter, AfterViewInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

import { DataModelService } from './../../Services/data-model.service';
import { CurrentLoginService } from './../../Services/current-login.service';

import { AlbumPlayed, AlbumPlayedAddResponse } from './../../Models/AlbumPlayed';

import { BaseAlbumEdit } from './../base-album-edit.component';

@Component({
    selector: 'app-edit-album',
    templateUrl: './edit-album.component.html',
    styleUrls: ['./edit-album.component.scss']
})
/** EditAlbum component*/
export class EditAlbumComponent extends BaseAlbumEdit  implements OnInit, AfterViewInit
{
    /** EditAlbum ctor */
    constructor(
        public formBuilder: FormBuilder,
        public dataModelService: DataModelService,
        public currentLoginService: CurrentLoginService)
    {
        super(formBuilder, dataModelService, currentLoginService);
    }


    //#region BaseAlbumEdit Implementation

    @Output() change = new EventEmitter();

    public ngOnInitAddition()
    {
        this.dataModelService.fetchAllAlbumPlayedData().then(() =>
        {
            this.albumsPlayed = this.dataModelService.albumsPlayed;

            this.albumsPlayed = new Array<AlbumPlayed>();
            for (let album of this.dataModelService.albumsPlayed)
            {
                const newAlbum = AlbumPlayed.fromData(album);

                if (newAlbum.userName === this.currentLoginService.name)
                {
                    this.albumsPlayed.push(newAlbum);
                }
            }

            this.setupDataTable(this.albumsPlayed);
        });
    };

    public ngAfterViewInitAddition()
    {
        if (this.albumsPlayed !== undefined) {
            this.setupDataTable(this.albumsPlayed);
        }
    };

    //#endregion


    //#region Data Table implementation

    @ViewChild('itemsTablePaginator') public itemsTablePaginator: MatPaginator;
    @ViewChild('itemsTableSort') public itemsTableSort: MatSort;

    public setupDataTable(albumsData: AlbumPlayed[]): void
    {
        this.items = albumsData;
        this.itemsDataSource = new MatTableDataSource(this.items);
        this.setupItemsPagingAndSorting();
    }

    public albumsPlayed: AlbumPlayed[] = new Array<AlbumPlayed>();
    public items: any[];
    public itemsDisplayedColumns: string[] = this.getItemsDisplayedColumns();
    public itemsDataSource: MatTableDataSource<any>;

    public getItemsDisplayedColumns(): string[]
    {
        var columns =
            [
                'date',
                'location',
                'artist',
                'album'
            ];

        return columns;
    }

    public setupItemsPagingAndSorting(): void
    {
        if (this.items != null)
        {
            setTimeout(() =>
            {
                this.itemsDataSource.paginator = this.itemsTablePaginator;
                this.itemsDataSource.sort = this.itemsTableSort;
                this.itemsTableSort.sortChange.subscribe(() =>
                {
                    this.itemsTablePaginator.pageIndex = 0;
                    this.itemsTablePaginator._changePageSize(this.itemsTablePaginator.pageSize);
                });
            });
        }
    }

    public applyItemsFilter(filterValue: string)
    {
        this.itemsDataSource.filter = filterValue.trim().toLowerCase();

        if (this.itemsDataSource.paginator)
        {
            this.itemsTablePaginator.pageIndex = 0;
            this.itemsTablePaginator._changePageSize(this.itemsTablePaginator.pageSize);
        }
    }

    public onItemsRowClicked(row)
    {
        var albumPlayed = AlbumPlayed.fromData(row);
        var title = albumPlayed.artist + " " + albumPlayed.album;

        //alert(title);

        this.albumToEdit = true;
        this.editAlbumPlayed = albumPlayed;
        this.newAlbumPlayed = albumPlayed;

        this.setCurrentValues();
    }

    //#endregion


    //#region Accordion State implementation

    public showAllAlbumsPanelOpenState = true;
    public editAlbumPlayed: AlbumPlayed = null;
    public albumToEdit: boolean = false;


    //#endregion


    //#region Update Details


    public selectedListeningTime = new FormControl(new Date());

    public selectedMoment = new Date();

    public setCurrentValues()
    {
        this.addAlbumGroup.setValue(
            {
                location: this.editAlbumPlayed.location,
                artist: this.editAlbumPlayed.artist,
                album: this.editAlbumPlayed.album,
                imageUrl: this.editAlbumPlayed.imagePath,
                playerUrl: this.editAlbumPlayed.playerLink,
                date: this.editAlbumPlayed.date
            });

        this.selectedListeningTime.setValue(this.editAlbumPlayed.date);

    }

    //#endregion

    //#region Page Control Handlers

    public async onUpdateAlbum()
    {
        var album = this.getNewAlbumPlayed();
        album.date = this.selectedListeningTime.value;

        await this.dataModelService.updateAsyncAlbumPlayed(album);

        var resp = AlbumPlayedAddResponse.fromData(this.dataModelService.updateAlbumPlayedResponse);
    }


    public async onDeleteAlbum()
    {
        var album = this.getNewAlbumPlayed();

        await this.dataModelService.deleteAsyncAlbumPlayed(album);

        var resp = AlbumPlayedAddResponse.fromData(this.dataModelService.deleteAlbumPlayedResponse);
    }

    //#endregion




}