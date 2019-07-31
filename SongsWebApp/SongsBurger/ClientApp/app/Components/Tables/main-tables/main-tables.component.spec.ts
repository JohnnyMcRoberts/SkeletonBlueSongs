/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { MainTablesComponent } from './main-tables.component';

let component: MainTablesComponent;
let fixture: ComponentFixture<MainTablesComponent>;

describe('MainTables component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ MainTablesComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(MainTablesComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});