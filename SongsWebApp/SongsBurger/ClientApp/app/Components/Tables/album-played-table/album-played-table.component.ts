import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

import { DataModelService } from './../../../Services/data-model.service';

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
    constructor(private dataModelService: DataModelService)
    {
        this.componentTitle = "Loading books from database...";
    }

    public componentTitle: string;
    public albumsPlayed: AlbumPlayed[];


    //@ViewChild('booksTablePaginator') booksTablePaginator: MatPaginator;
    //@ViewChild('booksTableSort') public booksTableSort: MatSort;
    //public booksDataSource: MatTableDataSource<Book>;

    ngOnInit()
    {
        this.dataModelService.fetchAllAlbumPlayedData().then(() =>
        {
            this.albumsPlayed = this.dataModelService.albumsPlayed;
            //this.booksDataSource = new MatTableDataSource(this.books);
            //this.setupBooksPagingAndSorting();
        });
    }

    ngAfterViewInit()
    {
        //this.setupBooksPagingAndSorting();
    }


}