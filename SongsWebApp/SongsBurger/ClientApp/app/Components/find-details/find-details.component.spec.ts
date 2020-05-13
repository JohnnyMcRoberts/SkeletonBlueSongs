/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FindDetailsComponent } from './find-details.component';

let component: FindDetailsComponent;
let fixture: ComponentFixture<FindDetailsComponent>;

describe('FindDetails component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FindDetailsComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FindDetailsComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});