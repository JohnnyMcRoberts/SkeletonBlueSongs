/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AlbumDetailComponent } from './album-detail.component';

let component: AlbumDetailComponent;
let fixture: ComponentFixture<AlbumDetailComponent>;

describe('AlbumDetail component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AlbumDetailComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AlbumDetailComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});