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
import { ModalWindowComponent } from './components/modal-window/modal-windows.component';
import { DbConnectionComponent } from './components/dbconnection/dbcon.component';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';

import { NgProgressModule } from 'ngx-progressbar';
import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthService } from './services/auth.service';
import { Config } from './config';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        AuthorComponent,
        BookComponent,
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
        RouterModule.forRoot([
            { path: 'home', component: HomeComponent },
            { path: 'author', component: AuthorComponent },
            { path: 'book', component: BookComponent },
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
