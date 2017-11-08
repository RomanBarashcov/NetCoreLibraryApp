﻿import { TemplateRef, ViewChild, Component, OnDestroy, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { Author } from '../../models/author';
import { AuthorPagedResult } from '../../models/authorPagedResult';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs/Observable';
import { Config } from '../../config';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { trigger, state, style, animate, transition, group } from '@angular/animations';
import { NgProgress } from 'ngx-progressbar';

@Component({
    selector: 'authors-app',
    templateUrl: 'author.component.html',
    styleUrls: ['author.component.css'],
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
export class AuthorComponent {
    
    @ViewChild('readOnlyTemplate') readOnlyTemplate: TemplateRef<any>;
    @ViewChild('editTemplate') editTemplate: TemplateRef<any>;

    private authorApiUrl: string;
    authors: Author[] = [];
    authorPagedResult: AuthorPagedResult;
    editedAuthor: Author;
    editAuthorNull: Author;
    isNewRecord: boolean;
    statusMessage: string;
    private sub: Subscription;
    error: any;
    state: string = '';
    
    isAuthorized: boolean;
    private isAuthorizedSubscription: Subscription;

    currentPage: number;
    currentOrderBy: string;
    currentAscending: boolean;
    pageSize: number;
    rowCount: number[] = [5, 10, 20];
    selectedRowCount: number;
    totalNumberOfPages: number;
    countNumberOfPages: number[];
    totalNumberOfRecords: number;

    constructor(private authService: AuthService, private config: Config, private router: Router, private ngProgress: NgProgress) {

        this.ngProgress.start();
        this.authorApiUrl = this.config.AuthorsApiUrl;
        this.selectedRowCount = 5;
        this.loadAuthors(1, "Id", true);
    }

    animate() {

        this.state = (this.state === '' ? 'in' : '');

    }

    sort(orderBy: string, ascending: boolean) {

        this.ngProgress.start();
        ascending = ascending ? false : true;
        this.loadAuthors(this.currentPage, orderBy, ascending);

    };

    onRowCountSelected(rowCoutn: number) {

        this.ngProgress.start();
        this.selectedRowCount = rowCoutn;
        this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);

    }

    loadAuthors(page: number, orderBy: string, ascending: boolean) {

        this.currentPage = page;
        this.currentOrderBy = orderBy;
        this.currentAscending = ascending;

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe(

            (isAuthorized: boolean) => {

                this.isAuthorized = isAuthorized;

            });
        
        this.authService.get(this.authorApiUrl + "?page=" + page + "&pageSize=" + this.selectedRowCount + "&orderBy=" + orderBy + "&ascending=" + ascending).subscribe(result => {

            this.authorPagedResult = result.json();

            if (this.authorPagedResult != null) {

                this.authors = this.authorPagedResult.results;
                this.pageSize = this.authorPagedResult.pageSize;
                this.totalNumberOfRecords = this.authorPagedResult.totalNumberOfRecords;
                this.totalNumberOfPages = this.authorPagedResult.totalNumberOfPages.length;
                this.configureCountPages(this.authorPagedResult.totalNumberOfPages);

            } else {

                this.authors = [];

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

        if (this.currentPage + 1 < numberArr.length) {

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

    addAuthor() {

        if (this.isAuthorized) {

            this.editedAuthor = new Author("", "", "");
            this.authors.push(this.editedAuthor);
            this.isNewRecord = true;

        }
        else {

            this.statusMessage = "Please log in!";

        }
    }

    editAuthor(author: Author) {

        if (this.isAuthorized) {

            this.editedAuthor = new Author(author.id, author.name, author.surname);   

        }
        else {

            this.statusMessage = "Please log in!";

        }
    }

    loadTemplate(author: Author) {

        if (this.editedAuthor && this.editedAuthor.id == author.id) {

            return this.editTemplate;

        } else {

            return this.readOnlyTemplate;

        }
    }

    saveAuthor() {

        this.ngProgress.start();

        if (this.isNewRecord) {

            this.authService.post(this.authorApiUrl, this.editedAuthor).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Saved successfully!';
                    this.editedAuthor = this.editAuthorNull;
                    this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);

                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                    this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);

                });

            this.isNewRecord = false;

        } else {

            this.authService.put(this.authorApiUrl + "/" + this.editedAuthor.id, this.editedAuthor).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Updated successfully!';
                    this.editedAuthor = this.editAuthorNull;
                    this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);

                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                    this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);

                });
        }
    }

    cancel() {

        this.ngProgress.start();

        if (this.isNewRecord) {

            this.authors.pop();
            this.isNewRecord = false;

        }
        else {

            this.authors.pop();

        }

        this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);
        this.editedAuthor = this.editAuthorNull;

    }

    deleteAuthor(author: Author) {

        this.ngProgress.start();

        if (this.isAuthorized) {

            this.authService.delete(this.authorApiUrl + "/" + author.id).subscribe((resp: Response) => {

                if (resp.status == 200) {

                      this.statusMessage = 'Deleted successfully!';
                      this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);

                   }
                },
                error => {

                    this.statusMessage = error;
                    console.log(error);
                    this.loadAuthors(this.currentPage, this.currentOrderBy, this.currentAscending);

                });
        }
        else {

            this.statusMessage = "Please log in!";

        }
    }

    routeToBooks(author: Author) {

        this.router.navigate(['/bookByAuthor', author.id]);

    }

    setPage(page: number) {

        this.ngProgress.start();
        this.loadAuthors(page, this.currentOrderBy, this.currentAscending);

    }
}