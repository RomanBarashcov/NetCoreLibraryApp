import { Component, OnDestroy, Input } from '@angular/core';
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
    
    private authorApiUrl: string;
    authors: Author[] = [];
    authorPagedResult: AuthorPagedResult;

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

    addAuthor() {

        if (this.isAuthorized) {

            this.router.navigate(['/addAuthor']);

        }
        else {

            this.statusMessage = "Please log in!";

        }
    }

    editAuthor(authorId:number) {

        if (this.isAuthorized) {

            this.router.navigate(['/editAuthor', authorId]);   

        }
        else {

            this.statusMessage = "Please log in!";

        }
    }

    deleteAuthor(authorId: number) {

        this.ngProgress.start();

        if (this.isAuthorized) {

            this.authService.delete(this.authorApiUrl + "/" + authorId).subscribe((resp: Response) => {

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

    routeToBooks(authorId: number) {

        this.router.navigate(['/bookByAuthor', authorId]);

    }

    setPage(page: number) {

        this.ngProgress.start();
        this.loadAuthors(page, this.currentOrderBy, this.currentAscending);

    }
}