﻿import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ToDoListComponent } from './Components/to-do-list/to-do-list.component';
import { FindDetailsComponent } from './Components/find-details/find-details.component';
import { LoginComponent } from './Components/login/login.component';
import { AddAlbumComponent } from './Components/add-album/add-album.component';
import { AlbumsReportComponent } from './Components/albums-report/albums-report.component';
import { EditAlbumComponent } from './Components/edit-album/edit-album.component';
import { TextFileImportExportComponent } from './Components/text-file-import-export/text-file-import-export.component';
import { MainTablesComponent } from './Components/Tables/main-tables/main-tables.component';

const routes: Routes =
[
    { path: '', component: LoginComponent, pathMatch: 'full' },

    { path: 'login', component: LoginComponent },
    { path: 'add-album', component: AddAlbumComponent },
    { path: 'edit-album', component: EditAlbumComponent },
    { path: 'albums-report', component: AlbumsReportComponent },
    { path: 'to-do-list', component: ToDoListComponent },
    { path: 'main-tables', component: MainTablesComponent },
    { path: 'import-export', component: TextFileImportExportComponent },
    { path: 'find-details', component: FindDetailsComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule
{
}