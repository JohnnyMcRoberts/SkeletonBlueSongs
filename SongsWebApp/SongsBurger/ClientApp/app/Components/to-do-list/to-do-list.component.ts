import { Component } from '@angular/core';

export interface ISection {
    name: string;
    updated: Date;
}

@Component({
    selector: 'app-to-do-list',
    templateUrl: './to-do-list.component.html',
    styleUrls: ['./to-do-list.component.scss']
})
/** ToDoList component*/
export class ToDoListComponent
{
    /** ToDoList ctor */
    constructor()
    {

    }

    public complete: ISection[] =
    [
        {
            name: 'Get Material and hamburger menu structure going',
            updated: new Date('30/07/19')
        },
        {
            name: 'Read & display data from the Mongo DB',
            updated: new Date('02/08/19'),
        },
        {
            name: 'Create the new user login page',
            updated: new Date('07/08/19'),
        },
        {
            name: 'Read & display data from the text file',
            updated: new Date('08/08/19'),
        },
        {
            name: 'Add new recording page',
            updated: new Date('09/08/19'),
        },
        {
            name: 'Add the Edit and Delete page',
            updated: new Date('11/08/19'),
        },
        {
            name: 'Complete the Update and delete functionality',
            updated: new Date('12/08/19'),
        },
        {
            name: 'Add a report page',
            updated: new Date('13/08/19'),
        },
        {
            name: 'Include the import/export for a few formats',
            updated: new Date('23/08/19'),

        }
    ];

    public toDoItems: ISection[] =
    [
    ];

}