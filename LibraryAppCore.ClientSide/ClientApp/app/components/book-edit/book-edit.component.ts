import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response, RequestOptions } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../models/book';
import { BookViewModel } from '../../models/bookViewModel';
import { Author } from '../../models/author';
import { Config } from '../../config';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { trigger, state, style, animate, transition, group } from '@angular/animations';
import { NgProgress } from 'ngx-progressbar';

@Component({
    selector: 'book-edit-app',
    templateUrl: 'book-edit.component.html',
    styleUrls: ['book-edit.component.css'],
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
export class BookEditComponent implements OnInit, OnDestroy {

    books: Book[] = [];
    authors: Author[] = [];
    editedBook: BookViewModel;
    authorData: Author;
    booksViewModel: BookViewModel[] = [];

    isNewRecord: boolean;
    statusMessage: string;
    hiddenAuthorId: string;
    private sub: Subscription;
    error: any;
    state: string = '';

    private bookApiUrl: string;
    private authorApiUrl: string;

    isAuthorized: boolean;
    private isAuthorizedSubscription: Subscription;

    constructor(private authService: AuthService, private activateRoute: ActivatedRoute, private config: Config, private router: Router, private ngProgress: NgProgress) {

        this.ngProgress.start();
        this.authorApiUrl = this.config.AuthorsApiUrl;
        this.bookApiUrl = this.config.BookApiUrl;
        this.loadAuthors();
    }

    ngOnInit() {

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe((isAuthorized: boolean) => {

            this.isAuthorized = isAuthorized;
        });

        this.sub = this.activateRoute.params.subscribe((params) => {

            var id = params['id'];
            var authorId = params['authorId'];

            if (authorId != null || id != null) {

                this.editBook(params['id'], params['authorId']);
            }
            else {

                this.addBook(params['authorId']);
            }
        });
    }

    ngOnDestroy() {

        this.sub.unsubscribe();
    }

    animate() {

        this.state = (this.state === '' ? 'in' : '');
    }

    addBook(authorId: string) {

        console.log("add book");

        this.isNewRecord = true;

        if (this.isAuthorized) {

            if (authorId != undefined) {

                this.editedBook = new BookViewModel("", 0, "", "", authorId, "");
            }
            else {

                this.editedBook = new BookViewModel("", 0, "", "", "0", "");
            }

            this.booksViewModel.push(this.editedBook);
        }
        else {

            this.statusMessage = "Please log in!";
        }

        this.ngProgress.done();
    }

    editBook(bookId: string, authorId: string) {

        this.editedBook = new BookViewModel("", 0, "", "", "0", "");

        if (this.isAuthorized) {

            this.authService.get(this.bookApiUrl + "/GetBookById/" + bookId).subscribe((resp: Response) => {

                this.editedBook = resp.json();
            },
                error => {

                    console.log(error);
                });

        } else {

            this.statusMessage = "Please log in!";
        }

        this.ngProgress.done();
    }


    saveBook() {

        this.ngProgress.start();

        if (this.isNewRecord) {

            let createdBookData: Book = new Book(this.editedBook.id, this.editedBook.year, this.editedBook.name, this.editedBook.description, this.editedBook.authorId, this.editedBook.authorName);

            this.authService.post(this.bookApiUrl, createdBookData).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Saved successfully!';
                    this.router.navigate(['/book']);
                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);

                });

        } else {

            let updatedBookData: Book = new Book(this.editedBook.id, this.editedBook.year, this.editedBook.name, this.editedBook.description, this.editedBook.authorId, this.editedBook.authorName);

            this.authService.put(this.bookApiUrl + "/" + this.editedBook.id, updatedBookData).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Updated successfully!';
                    this.router.navigate(['/book']);

                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);

                });
        }
    }

    cancel() {

        this.router.navigate(['/book']);
    }

    loadAuthors() {

        this.authors = [];

        this.authService.get(this.authorApiUrl + "/GetAuthors").subscribe(result => {

            this.authors = result.json();

            if (this.authors == null) {

                this.authors = [];
            }
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
}