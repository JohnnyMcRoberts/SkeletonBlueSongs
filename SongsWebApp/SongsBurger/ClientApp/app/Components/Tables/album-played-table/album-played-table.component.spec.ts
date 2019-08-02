/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AlbumPlayedTableComponent } from './album-played-table.component';

let component: AlbumPlayedTableComponent;
let fixture: ComponentFixture<AlbumPlayedTableComponent>;

describe('AlbumPlayedTable component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AlbumPlayedTableComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AlbumPlayedTableComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});