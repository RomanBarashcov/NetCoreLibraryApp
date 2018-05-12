import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { AuthorComponent } from './components/author/author.component';
import { AuthorEditComponent } from './components/author-edit/author-edit.component';
import { BookComponent } from './components/book/book.component';
import { BookEditComponent } from './components/book-edit/book-edit.component';
import { ModalWindowComponent } from './components/modal-window/modal-windows.component';
import { DbConnectionComponent } from './components/dbconnection/dbcon.component';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';

import { NgProgressModule } from 'ngx-progressbar';
import { SpinnerComponentModule } from 'ng2-component-spinner';
import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthService } from './services/auth.service';
import { Config } from './config';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        AuthorComponent,
        AuthorEditComponent,
        BookComponent,
        BookEditComponent,
        ModalWindowComponent,
        DbConnectionComponent,
        UnauthorizedComponent
    ],
    imports: [
        AuthModule.forRoot(),
        CommonModule,
        HttpModule,
        FormsModule,
        NgProgressModule,
        SpinnerComponentModule,
        RouterModule.forRoot([
            { path: 'home', component: HomeComponent },
            { path: 'author', component: AuthorComponent },
            { path: 'addAuthor', component: AuthorEditComponent },
            { path: 'editAuthor/:id', component: AuthorEditComponent },
            { path: 'book', component: BookComponent },
            { path: 'editBook/:id/:authorId', component: BookEditComponent },
            { path: 'editBook/:id', component: BookEditComponent },
            { path: 'addBook/:authorId', component: BookEditComponent },
            { path: 'addBook', component: BookEditComponent },
            { path: 'bookByAuthor/:id', component: BookComponent },
            { path: 'dbconnection', component: DbConnectionComponent },
            { path: 'unauthorized', component: UnauthorizedComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [Config, AuthService, OidcSecurityService],
    
})
export class AppModuleShared {
}
