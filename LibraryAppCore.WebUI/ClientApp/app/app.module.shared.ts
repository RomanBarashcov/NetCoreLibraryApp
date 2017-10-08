import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { AuthorComponent } from './components/author/author.component';
import { BookComponent } from './components/book/book.component';
import { DbConnectionComponent } from './components/dbconnection/dbcon.component';
import { LoginComponent } from './components/login/login.component';
import { LogOutComponent } from './components/logout/logout.component';
import { RegisterComponent } from './components/register/register.component';

import { PagerService } from './services/pagination.service';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        AuthorComponent,
        BookComponent,
        LoginComponent,
        LogOutComponent,
        RegisterComponent,
        DbConnectionComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: 'home', component: HomeComponent },
            { path: 'author', component: AuthorComponent },
            { path: 'book', component: BookComponent },
            { path: 'bookByAuthor/:id', component: BookComponent },
            { path: 'login', component: LoginComponent },
            { path: 'logout', component: LogOutComponent },
            { path: 'registration', component: RegisterComponent },
            { path: 'dbconnection', component: DbConnectionComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [PagerService]
    
})
export class AppModuleShared {
}
