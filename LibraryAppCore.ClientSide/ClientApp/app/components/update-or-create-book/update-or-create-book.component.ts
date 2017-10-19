import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../models/book';
import { BookSections } from '../../models/book-sections';
import { Config } from '../../config';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { PagerService } from '../../services/pagination.service';
import { trigger, state, style, animate, transition, group } from '@angular/animations';

@Component({
    selector: 'update-or-create-book-app',
    templateUrl: 'update-or-create-book.component.html',
    styleUrls: ['update-or-create-book.component.css'],
    providers: [AuthService, Config],
    animations: [
        trigger('flyInOut', [
            state('in', style({ transform: 'translateX(0)', opacity: 1 })),
            transition('void => *', [
                style({ transform: 'translateX(0px)', opacity: 0 }),
                group([
                    animate('1s 0.1s ease', style({
                        transform: 'translateX(0)',

                    })),
                    animate('1s ease', style({
                        opacity: 1
                    }))
                ])
            ]),
            transition('* => void', [
                group([
                    animate('1s ease', style({
                        transform: 'translateX(0px)',

                    })),
                    animate('1s 0.2s ease', style({
                        opacity: 0
                    }))
                ])
            ])
        ])
    ]
})
export class UpOrCrBookComponent {

    bookApiUrl: string;
    private sub: Subscription;
    editedBook: Book;
    editedBookNull: Book;
    editedBookSections: BookSections;
    createNewBook: boolean;
    statusMessage: any;
    authorId: string;

    constructor(private authService: AuthService, private activateRoute: ActivatedRoute, private pagerService: PagerService, private config: Config) {
        this.sub = activateRoute.params.subscribe((params) => {
            params['bookId'] != null && params['authorId'] != null ? this.editBook(params['bookId'], params['authorId']) : this.addBook(params['authorId'])
        });
        this.bookApiUrl = this.config.BookApiUrl;
    }

    editBook(bookId: string, authorId: string): void {
        this.authorId = authorId;
        this.createNewBook = false;
        this.editedBook = new Book("", 0, "", "", "", 0, 0, "0", "0", "", "", authorId);
        this.editedBookSections = new BookSections("", false, false, false, false, false, "");
        this.GetBookByBookId(bookId);
        this.GetSectionsByBookId(bookId);
    }

    addBook(authorId: string): void {
        this.authorId = authorId;
        this.createNewBook = true;
        this.editedBook = new Book("", 0, "", "", "", 0, 0, "0", "0", "", "", authorId);
        this.editedBookSections = new BookSections("", false, false, false, false, false, "");
    }

    private GetBookByBookId(bookId: string) {
        this.authService.get(this.bookApiUrl + "/GetBookById/" + bookId).subscribe(result => {
            this.editedBook = result.json();
        },
            error => {
                this.statusMessage = error;
                console.log(error);
            });
    }

    private GetSectionsByBookId(bookId: string) {
        this.authService.get(this.bookApiUrl + "/GetBookSectionsById/" + bookId).subscribe(result => {
            this.editedBookSections = result.json();
        },
            error => {
                this.statusMessage = error;
                console.log(error);
            });
    }

    saveBook() {
        if (this.createNewBook) {

            this.authService.post(this.bookApiUrl + "/CreateBook" , this.editedBook).subscribe((resp: Response) => {
                console.log("saveBook function");
                if (resp.status == 200) {
                    let seccess: boolean = this.saveSectionsBook(this.authorId);
                    if (seccess) {
                        this.statusMessage = 'Saved successfully!';
                    }
                    else {
                        this.statusMessage = 'Error, check all your data , and try again!';
                    }
                }
            },
                error => {
                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                });

            this.createNewBook = false;
            this.editedBook = this.editedBookNull;

        } else {
            this.authService.put(this.bookApiUrl + "/UpdateBook/" + this.editedBook.id, this.editedBook).subscribe((resp: Response) => {
                if (resp.status == 200) {
                    this.statusMessage = 'Updated successfully!';
                }
            },
                error => {
                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                });

            this.editedBook = this.editedBookNull;
        }
    }

    saveSectionsBook(authorId?: string, bookId?: string): boolean {

        if (authorId != undefined) {

            this.authService.put(this.bookApiUrl + "/BookApi/AddSectionsBook/" + authorId + "/" + this.editedBook.name, this.editedBook).subscribe((resp: Response) => {
                if (resp.status == 200) {
                    return true;
                }
            });
        }
        else if (bookId != undefined) {
            this.authService.put(this.bookApiUrl + "/BookApi/UpdateBookSections/" + bookId, this.editedBook).subscribe((resp: Response) => {
                if (resp.status == 200) {
                    return true;
                }
            });
        }

       return false;
    }
}