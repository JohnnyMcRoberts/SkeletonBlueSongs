/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { SearchDetailComponent } from './search-detail.component';

let component: SearchDetailComponent;
let fixture: ComponentFixture<SearchDetailComponent>;

describe('SearchDetail component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ SearchDetailComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(SearchDetailComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});