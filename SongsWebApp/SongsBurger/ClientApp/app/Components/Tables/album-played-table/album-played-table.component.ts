import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

import { DataModelService } from './../../../Services/data-model.service';
import { CurrentLoginService } from './../../../Services/current-login.service';

import { AlbumPlayed } from './../../../Models/AlbumPlayed';

@Component({
    selector: 'app-album-played-table',
    templateUrl: './album-played-table.component.html',
    styleUrls: ['./album-played-table.component.scss']
})
/** AlbumPlayedTable component*/
export class AlbumPlayedTableComponent implements OnInit, AfterViewInit 
{
    /** AlbumPlayedTable ctor */
    constructor(
        private dataModelService: DataModelService,
        private currentLoginService: CurrentLoginService)
    {
        this.componentTitle = "Loading Albums for User from database...";
    }

    public componentTitle: string;


    ngOnInit()
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

            //this.booksDataSource = new MatTableDataSource(this.books);
            this.setupDataTable(this.albumsPlayed);
        });
    }

    ngAfterViewInit()
    {
        if (this.albumsPlayed !== undefined)
        {
            this.setupDataTable(this.albumsPlayed);
        }
    }



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


    //#endregion


}