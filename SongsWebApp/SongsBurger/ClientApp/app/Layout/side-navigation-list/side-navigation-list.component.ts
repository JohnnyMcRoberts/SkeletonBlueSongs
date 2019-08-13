import { Component, OnInit, Output, EventEmitter } from '@angular/core';

export class NavigationMenuItem
{
    public link: string;
    public text: string;
    public icon: string;
}

@Component({
    selector: 'app-side-navigation-list',
    templateUrl: './side-navigation-list.component.html',
    styleUrls: ['./side-navigation-list.component.scss']
})
/** SideNavigationList component*/
export class SideNavigationListComponent implements OnInit {

    @Output() sidenavClose = new EventEmitter();

    @Output() selectedMenuItem = new EventEmitter<string>();

    public navigationMenuItems: NavigationMenuItem[] =
    [
        {
            link: "/login", text: "Login", icon: "lock_open"
        },
        {
            link: "/main-tables", text: "Main Tables", icon: "vertical_split"
        },
        {
            link: "/import-export", text: "Text Import/Export", icon: "import_export"
        },
        {
            link: "/add-album", text: "Add Album Played", icon: "playlist_add"
        },
        {
            link: "/edit-album", text: "Edit Album Played", icon: "edit"
        },
        {
            link: "/albums-report", text: "Albums Report", icon: "queue_music"
        },
        //albums-report
        {
            link: "/to-do-list", text: "To Do List", icon: "list_alt"
        }
    ];

    public static defaultMenuItemText: string = "To Do List";

    /** SideNavigationList ctor */
    constructor()
    {

    }

    ngOnInit()
    {
    }

    public onSidenavClose = (param: any) =>
    {
        console.log("closed using :" + param);
        this.sidenavClose.emit();
        this.selectedMenuItem.emit(param.toString());
    }
}