/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AlbumsReportComponent } from './albums-report.component';

let component: AlbumsReportComponent;
let fixture: ComponentFixture<AlbumsReportComponent>;

describe('AlbumsReport component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AlbumsReportComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AlbumsReportComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});