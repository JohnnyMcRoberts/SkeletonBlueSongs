/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { TextFileImportExportComponent } from './text-file-import-export.component';

let component: TextFileImportExportComponent;
let fixture: ComponentFixture<TextFileImportExportComponent>;

describe('TextFileImportExport component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ TextFileImportExportComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(TextFileImportExportComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});