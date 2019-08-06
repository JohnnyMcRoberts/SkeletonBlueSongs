
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import
{
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

import { CdkTableModule } from '@angular/cdk/table';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';

import { ToDoListComponent } from './Components/to-do-list/to-do-list.component';
import { TextFileImportExportComponent } from './Components/text-file-import-export/text-file-import-export.component';
import { LoginComponent } from './Components/login/login.component';

import { MainTablesComponent } from './Components/Tables/main-tables/main-tables.component';
import { AlbumPlayedTableComponent } from './Components/Tables/album-played-table/album-played-table.component';

import { AppRoutingModule } from './app-routing.module';

import { LayoutComponent } from './Layout/layout/layout.component';
import { SideNavigationListComponent } from './Layout/side-navigation-list/side-navigation-list.component';
import { NavigationHeaderComponent } from './Layout/navigation-header/navigation-header.component';

import { DataModelService } from './Services/data-model.service';
import { AddUserLoginService } from './Services/add-user-login.service';

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
            AddUserLoginService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
