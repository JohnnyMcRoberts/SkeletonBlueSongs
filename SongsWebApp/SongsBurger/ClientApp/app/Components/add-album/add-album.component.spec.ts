/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AddAlbumComponent } from './add-album.component';

let component: AddAlbumComponent;
let fixture: ComponentFixture<AddAlbumComponent>;

describe('AddAlbum component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AddAlbumComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AddAlbumComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});