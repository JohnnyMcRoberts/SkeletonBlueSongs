//#region Import Components

import { AppComponent } from './app.component';
import { ToDoListComponent } from './Components/to-do-list/to-do-list.component';
import { TextFileImportExportComponent } from './Components/text-file-import-export/text-file-import-export.component';
import { LoginComponent } from './Components/login/login.component';

import { MainTablesComponent } from './Components/Tables/main-tables/main-tables.component';
import { AlbumPlayedTableComponent } from './Components/Tables/album-played-table/album-played-table.component';

import { LayoutComponent } from './Layout/layout/layout.component';
import { SideNavigationListComponent } from './Layout/side-navigation-list/side-navigation-list.component';
import { NavigationHeaderComponent } from './Layout/navigation-header/navigation-header.component';

//#endregion

//#region Import Services

import { DataModelService } from './Services/data-model.service';
import { AddUserLoginService } from './Services/add-user-login.service';
import { CurrentLoginService } from './Services/current-login.service';
import { LoginService } from './Services/login.service';

//#endregion

//#region Import Modules

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CdkTableModule } from '@angular/cdk/table';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';

import { AppRoutingModule } from './app-routing.module';

import {
    MatAutocompleteModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatTreeModule,

    MatFormFieldModule

} from '@angular/material';

//#endregion

@NgModule({
    declarations:
    [
        AppComponent,

        ToDoListComponent,
        LoginComponent,
        TextFileImportExportComponent,

        MainTablesComponent,
        AlbumPlayedTableComponent,

        LayoutComponent,
        SideNavigationListComponent,
        NavigationHeaderComponent
    ],
    imports:
    [
        BrowserModule,
        FormsModule,
        HttpClientModule,
        ReactiveFormsModule,

        BrowserAnimationsModule,
        LayoutModule,

        MatAutocompleteModule,
        MatBadgeModule,
        MatBottomSheetModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatCardModule,
        MatCheckboxModule,
        MatChipsModule,
        MatDatepickerModule,
        MatDialogModule,
        MatDividerModule,
        MatExpansionModule,
        MatGridListModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatNativeDateModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatRadioModule,
        MatRippleModule,
        MatSelectModule,
        MatSidenavModule,
        MatSliderModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatSortModule,
        MatStepperModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatTooltipModule,
        MatTreeModule,
        MatFormFieldModule,

        CdkTableModule,

        AppRoutingModule
    ],
    providers:
    [
        DataModelService,
        AddUserLoginService,
        LoginService,
        CurrentLoginService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
