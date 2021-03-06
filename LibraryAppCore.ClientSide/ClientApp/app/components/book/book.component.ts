﻿import { TemplateRef, ViewChild } from '@angular/core';
import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response, RequestOptions, Http } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../models/book';
import { BookViewModel } from '../../models/bookViewModel';
import { BookPagedResult } from '../../models/bookPagedResult';
import { AuthorPagedResult } from '../../models/authorPagedResult';
import { Author } from '../../models/author';
import { Config } from '../../config';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { trigger, state, style, animate, transition, group } from '@angular/animations';
import { NgProgress } from 'ngx-progressbar';

@Component({
    selector: 'books-app',
    templateUrl: 'book.component.html',
    styleUrls: ['book.component.css'],
    providers: [AuthService, Config],
    animations: [
        trigger('flyInOut', [
            state('in', style({ transform: 'translateX(0)', opacity: 1 })),
            transition('void => *', [
                style({ transform: 'translateX(0px)', opacity: 0 }),
                group([
                    animate('1s  ease', style({
                        transform: 'translateX(0)',

                    })),
                    animate('1s ease', style({
                        opacity: 1
                    }))
                ])
            ]),
            transition('* => void', [
                group([
                    animate('0s ease', style({
                        transform: 'translateX(0px)',

                    })),
                    animate('0s  ease', style({
                        opacity: 0
                    }))
                ])
            ])
        ])
    ]
})
export class BookComponent implements OnDestroy {

    @ViewChild("fileInput") fileInput: any;

    books: Book[] = [];
    authors: Author[] = [];
    bookPagedResult: BookPagedResult;
    authorData: Author;
    booksViewModel: BookViewModel[] = [];

    isNewRecord: boolean;
    statusMessage: string;
    hiddenAuthorId: string;
    private sub: Subscription;
    private id: number;
    error: any;
    state: string = '';

    private bookApiUrl: string;
    private authorApiUrl: string;
    private documentApiUrl: string;

    isAuthorized: boolean;
    private isAuthorizedSubscription: Subscription;

    currentPage: number;
    currentOrderBy: string;
    currentAscending: boolean;
    pageSize: number;
    rowCount: number[] = [5, 10, 20];
    selectedRowCount: number;
    countNumberOfPages: number[];
    totalNumberOfPages: number;
    totalNumberOfRecords: number;

    showDialog: boolean = false;

    constructor(private authService: AuthService, private activateRoute: ActivatedRoute, private config: Config, private router: Router, private http: Http, private ngProgress: NgProgress) {

        this.ngProgress.start();
        this.authorApiUrl = this.config.AuthorsApiUrl;
        this.bookApiUrl = this.config.BookApiUrl;
        this.documentApiUrl = this.config.DocumentApiUrl;
        this.selectedRowCount = 5;
        this.sub = activateRoute.params.subscribe((params) => { params['id'] != null ? this.loadBookByAuthor(params['id'], 1, "Id", true) : this.loadBooks(1, "Id", true) });
    }

    ngOnDestroy() {

        this.sub.unsubscribe();

    }

    animate() {

        this.state = (this.state === '' ? 'in' : '');

    }

    sort(orderBy: string, ascending: boolean) {

        this.ngProgress.start();
        ascending = ascending ? false : true;
        this.loadBooks(this.currentPage, orderBy, ascending);

    };

    onRowCountSelected(rowCoutn: number) {

        this.ngProgress.start();
        this.selectedRowCount = rowCoutn;
        this.loadBooks(this.currentPage, this.currentOrderBy, this.currentAscending);
    }

    loadBooks(page: number, orderBy: string, ascending: boolean) {

        this.currentPage = page;
        this.currentOrderBy = orderBy;
        this.currentAscending = ascending;
        this.booksViewModel = [];

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe((isAuthorized: boolean) => {

            this.isAuthorized = isAuthorized;

        });

        this.authService.get(this.bookApiUrl + "?page=" + page + "&pageSize=" + this.selectedRowCount + "&orderBy=" + orderBy + "&ascending=" + ascending).subscribe(result => {

                this.bookPagedResult = result.json();
                this.pageSize = this.bookPagedResult.pageSize;
                this.totalNumberOfRecords = this.bookPagedResult.totalNumberOfRecords;
                this.totalNumberOfPages = this.bookPagedResult.totalNumberOfPages.length;
                this.configureCountPages(this.bookPagedResult.totalNumberOfPages);

                if (this.bookPagedResult.results != null) {

                    for (let b of this.bookPagedResult.results) {

                         this.booksViewModel.push(new BookViewModel(b.id, b.year, b.name, b.description, b.authorId, b.authorName));

                    }

                } else {

                    this.books = [];

                }

                this.ngProgress.done();
                this.animate();
                this.state = "in";

            },
            error => {

                    this.statusMessage = error;
                    console.log(error);

            });
    }

    configureCountPages(numberArr: number[]) {

        this.countNumberOfPages = [];

        if (this.currentPage + 1 <= numberArr.length && numberArr.length > 4) {

            let index: number = this.currentPage;
            let countPages: number;

            for (let p: number = index; p <= index + 4; p++) {

                if (this.currentPage == 1) {
                    countPages = numberArr[p - 1];
                }
                else if (this.currentPage == 2) {
                    countPages = numberArr[p - 2];
                }
                else {
                    countPages = numberArr[p - 3];
                }

                this.countNumberOfPages.push(countPages);
            }
        }
        else if (this.currentPage == numberArr.length) {

            this.countNumberOfPages.push(numberArr.length);

        }
        else {

            this.countNumberOfPages = numberArr;
        }
    }

    loadBookByAuthor(id: string, page: number, orderBy: string, ascending: boolean) {

        this.currentPage = page;
        this.currentOrderBy = orderBy;
        this.currentAscending = ascending;
        this.booksViewModel = [];

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe((isAuthorized: boolean) => {

            this.isAuthorized = isAuthorized;
        });

        this.authService.get(this.config.BookApiUrl + "/GetBookByAuthorId/" + id + "?page=" + page + "&pageSize=" + this.selectedRowCount + "&orderBy=" + orderBy + "&ascending=" + ascending).subscribe(result => {

                this.bookPagedResult = result.json();

                if (this.bookPagedResult.results != null) {

                    for (let b of this.bookPagedResult.results) {

                        this.booksViewModel.push(new BookViewModel(b.id, b.year, b.name, b.description, b.authorId, b.authorName));
                    }
                }
                else {

                    this.booksViewModel = [];
                }

                this.ngProgress.done();
                this.hiddenAuthorId = id;
                this.animate();
                this.state = "in";

            },
                error => {

                    this.statusMessage = error;
                    console.log(error);

                });
    }

    addBook() {

        this.isNewRecord = true;

        if (this.isAuthorized) {

            if (this.hiddenAuthorId != undefined) {

                this.router.navigate(['/addBook', this.hiddenAuthorId]);   
            }
            else {

                this.router.navigate(['/addBook']);   
            }
        }
        else {

            this.statusMessage = "Please log in!";
        }
    }

    editBook(bookId: number) {

        if (this.hiddenAuthorId != undefined) {

            this.router.navigate(['/editBook', this.hiddenAuthorId, bookId]);

        } else {

            this.router.navigate(['/editBook', bookId]);
        }
    }

    deleteBook(book: Book) {

        this.ngProgress.start();

        if (this.isAuthorized) {

            this.authService.delete(this.bookApiUrl + "/" + book.id).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Deleted successfully!';
                    this.loadBooks(this.currentPage, this.currentOrderBy, this.currentAscending);
                }
            },
                error => {

                    this.statusMessage = error;
                    console.log(error);

                });
        }
        else {

            this.statusMessage = "Please log in!";
        }
    }

    addFile(): void {

        this.ngProgress.start();
        this.showDialog = true;
        let fi = this.fileInput.nativeElement;

        if (fi.files && fi.files[0]) {

            let fileToUpload = fi.files[0];
            let data = new FormData();
            data.append("file", fileToUpload);

            this.authService.postFormData(this.documentApiUrl + "/Upload/", data)
                .subscribe(res => {

                    if (res.status == 200 && res.text() == "Data added successfully") {

                        this.loadBooks(this.currentPage, this.currentOrderBy, this.currentAscending);
                    }
                    else if (res.status == 200 && res.text() == "All data is dublicating!") {

                        this.statusMessage = res.text();
                    }
                    else {

                        this.statusMessage = res.text();
                    }

                    this.ngProgress.done();
                    this.showDialog = false;
                    console.log(res);
                });
        }
    } 

    setPage(page: number) {

        this.ngProgress.start();
        this.loadBooks(page, this.currentOrderBy, this.currentAscending);
    }

}