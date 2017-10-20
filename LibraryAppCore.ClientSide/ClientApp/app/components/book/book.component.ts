import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../models/book';
import { Config } from '../../config';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { PagerService } from '../../services/pagination.service';
import { trigger, state, style, animate, transition, group } from '@angular/animations';

@Component({
    selector: 'books-app',
    templateUrl: 'book.component.html',
    styleUrls: ['book.component.css'],
    providers: [AuthService, Config],
    animations: [
        trigger('flyInOut', [
            state('in', style({transform: 'translateX(0)', opacity: 1})),
            transition('void => *', [
                style({transform: 'translateX(0px)', opacity: 0}),
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
export class BookComponent {    

    books: Book[] = [];
    statusMessage: string;
    hiddenAuthorId: string;
    private sub: Subscription;
    private id: number;
    private allItems: any[];
    pagedBookItems: any[];
    pager: any = {};
    error: any;
    state: string = '';
    private bookApiUrl: string;

    constructor(private authService: AuthService, private activateRoute: ActivatedRoute, private pagerService: PagerService, private config: Config, private router: Router) {
        this.sub = activateRoute.params.subscribe((params) => { params['id'] != null ? this.loadBookByAuthor(params['id']) : this.loadBooks() });
        this.bookApiUrl = this.config.BookApiUrl;
    }

    animate() {
        this.state = (this.state === '' ? 'in' : '');
    }
    
    loadBooks() {
        this.authService.get(this.bookApiUrl).subscribe(result => {
            try {
                this.books = result.json();
            }
            catch (e) {
                console.log("error: " + e);
            }

            this.setPage(1);
            this.animate();
        },
            error => {
                this.statusMessage = error;
                console.log(error);
            });
    }

    loadBookByAuthor(id: string) {
        this.authService.get(this.config.BookApiUrl + "/GetBookByAuthorId/" + id).subscribe(result => {
            this.books = result.json();
                if (this.books != null) {
                    this.pagedBookItems = this.books;
                    console.log("loadBookByAuthor() component result: " + this.books);
                        if (this.pager.totalPages > 0) {
                            this.setPage(this.pager.totalPages);
                        }
                }
                else {
                    this.books = [];
                }
            this.hiddenAuthorId = id;
        },
            error => {
                this.statusMessage = error;
                console.log(error);
            });
    }

    addBook(authorId?: string) {
        if (authorId != undefined) {
            this.router.navigate(['/AddBook', authorId]);
        }
        else {
            this.router.navigate(['/AddBook']);
        }
    }

    editBook(bookId: string, authorId: string) {
        if (bookId != undefined && authorId != undefined) {
            this.router.navigate(['/EditBook', bookId, authorId]);
        }
    }

    //editBook(book: Book) {
    //   // this.editedBook = new Book(book.id, book.year, book.name, book.description, book.authorId);
    //}

    //loadTemplate(book: Book) {
    //    if (this.editedBook && this.editedBook.id == book.id) {
    //        return this.editTemplate;
    //    } else {
    //        return this.readOnlyTemplate;
    //    }
    //}

    //saveBook() {
    //    if (this.isNewRecord) {
    //        this.authService.post(this.bookApiUrl, this.editedBook).subscribe((resp: Response) => {
    //            console.log("saveBook function");
    //            if (resp.status == 200) {
    //                this.statusMessage = 'Saved successfully!';
    //                this.loadBooks();
    //            }
    //        },
    //            error => {
    //                this.statusMessage = error + ' Check all your data, and try again! ';
    //                console.log(error);
    //                this.loadBooks();
    //            });

    //        this.isNewRecord = false;
    //        this.editedBook = this.editedBookNull;

    //    } else {
    //        this.authService.put(this.bookApiUrl + "/" + this.editedBook.id, this.editedBook).subscribe((resp: Response) => {
    //            if (resp.status == 200) {
    //                this.statusMessage = 'Updated successfully!';
    //                this.loadBooks();
    //            }
    //        },
    //            error => {
    //                this.statusMessage = error + ' Check all your data, and try again! ';
    //                console.log(error);
    //                this.loadBooks();
    //            });

    //        this.editedBook = this.editedBookNull;
    //    }
    //}

    //cancel() {
    //    this.editedBook = this.editedBookNull;
    //}

    //deleteBook(book: Book) {
    //    this.authService.delete(book.id).subscribe((resp: Response) => {
    //        if (resp.status == 200) {
    //            this.statusMessage = 'Deleted successfully!',
    //                this.loadBooks();
    //        }
    //    },
    //        error => {
    //            this.statusMessage = error;
    //            console.log(error);
    //        });
    //}

    setPage(page: number) {
        if (page < 1 || page > this.pager.totalPages) {
            return;
        }

        // get pager object from service
        this.pager = this.pagerService.getPager(this.books.length, page);

        // get current page of items
        this.pagedBookItems = this.books.slice(this.pager.startIndex, this.pager.endIndex + 1);

    }

}