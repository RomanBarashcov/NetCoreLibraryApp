import { TemplateRef, ViewChild } from '@angular/core';
import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response, RequestOptions, Http } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../models/book';
import { BookViewModel } from '../../models/bookViewModel';
import { Author } from '../../models/author';
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

    @ViewChild('readOnlyTemplate') readOnlyTemplate: TemplateRef<any>;
    @ViewChild('editTemplate') editTemplate: TemplateRef<any>;
    @ViewChild("fileInput") fileInput: any;

    books: Book[] = [];
    authors: Author[] = [];
    booksNull: Book;
    editedBook: BookViewModel;
    authorData: Author;
    editedBookNull: BookViewModel;
    booksViewModel: BookViewModel[] = [];


    isNewRecord: boolean;
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
    private authorApiUrl: string;
    private documentApiUrl: string;

    isAuthorized: boolean;
    private isAuthorizedSubscription: Subscription;

    isDesc: boolean = false;
    column: string = 'id';
    direction: number;

    constructor(private authService: AuthService, private activateRoute: ActivatedRoute, private pagerService: PagerService, private config: Config, private http: Http) {

        this.authorApiUrl = this.config.AuthorsApiUrl;
        this.loadAuthors();
        this.bookApiUrl = this.config.BookApiUrl;
        this.documentApiUrl = this.config.DocumentApiUrl;

        this.sub = activateRoute.params.subscribe((params) => { params['id'] != null ? this.loadBookByAuthor(params['id']) : this.loadBooks() });
    }

    ngOnDestroy() {

        this.sub.unsubscribe();

    }

    animate() {

        this.state = (this.state === '' ? 'in' : '');

    }

    sort(property: string) {

        this.isDesc = !this.isDesc;
        this.column = property;
        this.direction = this.isDesc ? 1 : -1;

    };

    loadBooks() {

        this.booksViewModel = [];

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe((isAuthorized: boolean) => {

            this.isAuthorized = isAuthorized;

        });

        setTimeout(() => {

            this.authService.get(this.bookApiUrl).subscribe(result => {

                this.books = result.json();

                console.log(" Books Result: " + this.books);
                console.log(" Authors Result: " + this.authors);

                if (this.books.length > 0 && this.authors.length > 0) {

                    for (let b of this.books) {

                        for (let a of this.authors) {

                            if (a.id === b.authorId) {

                                this.booksViewModel.push(new BookViewModel(b.id, b.year, b.name, b.description, b.authorId, a.name, a.surname));

                            }

                        }
                    }

                    console.log("loadBooks() component result: " + this.booksViewModel);

                    this.setPage(1);

                } else {

                    this.setPage(0);

                    this.books = [];

                }

                this.animate();

            },
                error => {

                    this.statusMessage = error;
                    console.log(error);

                });

        }, 250);
    }

    loadBookByAuthor(id: string) {

        this.booksViewModel = [];

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe((isAuthorized: boolean) => {

            this.isAuthorized = isAuthorized;

        });

        setTimeout(() => {

            this.authService.get(this.config.BookApiUrl + "/GetBookByAuthorId/" + id).subscribe(result => {

                this.books = result.json();

                if (this.books.length > 0 && this.authors.length > 0) {

                    for (let b of this.books) {

                        for (let a of this.authors) {

                            if (a.id === b.authorId) {

                                this.booksViewModel.push(new BookViewModel(b.id, b.year, b.name, b.description, b.authorId, a.name, a.surname));

                            }
                        }
                    }

                    this.pagedBookItems = this.booksViewModel;
                    console.log("loadBookByAuthor() component result: " + this.booksViewModel);

                    this.setPage(1);
                }
                else {

                    this.booksViewModel = [];

                    this.setPage(0);

                }

                this.hiddenAuthorId = id;

            },
                error => {

                    this.statusMessage = error;
                    console.log(error);

                });
        }, 250);
    }

    addBook() {

        this.isNewRecord = true;

        if (this.isAuthorized) {

            if (this.hiddenAuthorId != undefined) {

                let author: Author = this.loadAuthorById(this.hiddenAuthorId);

                if (author.id != null && author.name != null && author.surname != null) {

                    this.editedBook = new BookViewModel("", 0, "", "", this.hiddenAuthorId, author.name, author.surname);

                }
            }
            else {

                this.editedBook = new BookViewModel("", 0, "", "", "0", "", "");

            }

            this.booksViewModel.push(this.editedBook);
            this.pagedBookItems = this.booksViewModel;

            if (this.pager.totalPages > 0) {

                this.setPage(this.pager.totalPages);
            }
        }
        else {

            this.statusMessage = "Please log in!";

        }
    }

    editBook(book: BookViewModel) {

        let author: Author = this.loadAuthorById(book.authorId);

        if (this.isAuthorized) {

            if (author.id != null && author.name != null && author.surname != null) {

                this.editedBook = new BookViewModel(book.id, book.year, book.name, book.description, book.authorId, book.authorName, book.authorSurname);

            }

        } else {

            this.statusMessage = "Please log in!";

        }
    }

    loadTemplate(book: BookViewModel) {

        if (this.editedBook && this.editedBook.id == book.id) {

            return this.editTemplate;

        } else {

            return this.readOnlyTemplate;
        }
    }

    saveBook() {

        if (this.isNewRecord) {

            let createdBookData: Book = new Book(this.editedBook.id, this.editedBook.year, this.editedBook.name, this.editedBook.description, this.editedBook.authorId);

            this.authService.post(this.bookApiUrl, createdBookData).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Saved successfully!';
                    this.loadBooks();

                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                    this.loadBooks();

                });

            this.isNewRecord = false;
            this.editedBook = this.editedBookNull;

        } else {

            let updatedBookData: Book = new Book(this.editedBook.id, this.editedBook.year, this.editedBook.name, this.editedBook.description, this.editedBook.authorId);

            this.authService.put(this.bookApiUrl + "/" + this.editedBook.id, updatedBookData).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Updated successfully!';
                    this.loadBooks();

                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                    this.loadBooks();

                });

            this.editedBook = this.editedBookNull;
        }
    }

    cancel() {

        if (this.isNewRecord) {

            this.booksViewModel.pop();
            this.isNewRecord = false;

        }
        else {

            this.booksViewModel.pop();

        }

        this.activateRoute.params.subscribe((params) => {

            params['id'] != null ? this.loadBookByAuthor(this.editedBook.authorId) : this.loadBooks()

        });

        this.editedBook = this.editedBookNull;
    }

    deleteBook(book: Book) {

        if (this.isAuthorized) {

            this.authService.delete(this.bookApiUrl + "/" + book.id).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Deleted successfully!';
                    this.loadBooks();

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

    loadAuthors() {

        this.authors = [];
        
        this.authService.get(this.authorApiUrl).subscribe(result => {

            this.authors = result.json();

            if (this.authors == null) {

                this.authors = [];
                console.log("Authors have null result!!!");
            }
            console.log("Authors have result!!!!!!!!!!!!!!: " + this.authors);
        },
            error => {

                this.statusMessage = error;
                console.log(error);

            });

    }

    loadAuthorById(authorId: string): Author {

        let result: Author = new Author("", "", "");

        if (this.authors.length > 0) {

            for (let a of this.authors) {

                if (a.id == authorId) {

                    result = new Author(a.id, a.name, a.surname);

                }
            }
        }

        return result;
    }

    onAuthorSelect(authorId: string) {

        this.editedBook.authorId = authorId;

    }


    addFile(): void {

        let fi = this.fileInput.nativeElement;

        if (fi.files && fi.files[0]) {

            let fileToUpload = fi.files[0];
            let data = new FormData();
            data.append("file", fileToUpload);


            this.authService.postFormData(this.documentApiUrl + "/Upload/", data)
                .subscribe(res => {

                    if (res.status == 200 && res.text() == "Data added successfully") {

                        this.loadBooks();

                    }
                    else if (res.status == 200 && res.text() == "All data is dublicating!") {

                        this.statusMessage = res.text();

                    }
                    else {

                        this.statusMessage = res.text();

                    }

                    console.log(res);

                });
        }
    } 

    setPage(page: number) {

        if (page < 1 || page > this.pager.totalPages) {

            return;

        }

        // get pager object from service
        this.pager = this.pagerService.getPager(this.booksViewModel.length, page);

        // get current page of items
        this.pagedBookItems = this.booksViewModel.slice(this.pager.startIndex, this.pager.endIndex + 1);

    }

}