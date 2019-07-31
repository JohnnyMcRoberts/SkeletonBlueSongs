import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ToDoListComponent } from './Components/to-do-list/to-do-list.component';
import { LoginComponent } from './Components/login/login.component';
import { MainTablesComponent } from './Components/Tables/main-tables/main-tables.component';

const routes: Routes =
[
    { path: '', component: ToDoListComponent, pathMatch: 'full' },

    { path: 'to-do-list', component: ToDoListComponent },
    { path: 'main-tables', component: MainTablesComponent },
    { path: 'login', component: LoginComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}