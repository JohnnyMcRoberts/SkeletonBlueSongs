/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ToDoListComponent } from './to-do-list.component';

let component: ToDoListComponent;
let fixture: ComponentFixture<ToDoListComponent>;

describe('ToDoList component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ToDoListComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ToDoListComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});