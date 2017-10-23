import { TemplateRef, ViewChild, Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { Author } from '../../models/author';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs/Observable';
import { Config } from '../../config';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { PagerService } from '../../services/pagination.service';
import { trigger, state, style, animate, transition, group } from '@angular/animations';


@Component({
    selector: 'authors-app',
    templateUrl: 'author.component.html',
    styleUrls: ['author.component.css'],
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
export class AuthorComponent implements OnInit {
    
    @ViewChild('readOnlyTemplate') readOnlyTemplate: TemplateRef<any>;
    @ViewChild('editTemplate') editTemplate: TemplateRef<any>;

    private authorApiUrl: string;
    authors: Author[] = [];
    editedAuthor: Author;
    editAuthorNull: Author;
    isNewRecord: boolean;
    statusMessage: string;
    private sub: Subscription;
    private allItems: any[];
    pagedAuthorItems: any[];
    pager: any = {};
    error: any;
    state: string = '';
    
    isAuthorized: boolean;
    private isAuthorizedSubscription: Subscription;

    constructor(private authService: AuthService, private config: Config, private pagerService: PagerService, private router: Router) {
        this.authorApiUrl = this.config.AuthorsApiUrl;
    }

    animate() {
        this.state = (this.state === '' ? 'in' : '');
    }
    
    ngOnInit() {
        
        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
                this.isAuthorized = isAuthorized;
            });
        
        this.authService.get(this.authorApiUrl).subscribe(result => {
            this.authors = result.json();
            this.setPage(1);
            this.animate();
            this.state = "in";
        },
            error => {
                this.statusMessage = error;
                console.log(error);
            });
    }

    addAuthor() {
        if (this.isAuthorized) {
            this.editedAuthor = new Author("", "", "");
            this.authors.push(this.editedAuthor);
            this.pagedAuthorItems = this.authors;
            this.isNewRecord = true;
            if (this.pager.totalPages > 0) {
                this.setPage(this.pager.totalPages);
            }
        }
        else{
            this.statusMessage = "Please log in!";
        }
    }

    editAuthor(author: Author) {
        if (this.isAuthorized){
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
        if (this.isNewRecord) {
            this.authService.post(this.authorApiUrl, this.editedAuthor).subscribe((resp: Response) => {
                console.log("saveAuthor() resp: = " + resp);
                if (resp.status == 200) {
                    this.statusMessage = 'Saved successfully!';
                    this.editedAuthor = this.editAuthorNull;
                    this.ngOnInit();
                }
            },
                error => {
                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                    this.ngOnInit();
                });

            this.isNewRecord = false;

        } else {
            this.authService.put(this.authorApiUrl + "/" + this.editedAuthor.id, this.editedAuthor).subscribe((resp: Response) => {
                if (resp.status == 200) {
                    this.statusMessage = 'Updated successfully!';
                    this.editedAuthor = this.editAuthorNull;
                    this.ngOnInit();
                }
            },
                error => {
                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                    this.ngOnInit();
                });
        }
    }

    cancel() {
        this.editedAuthor = this.editAuthorNull;
    }

    deleteAuthor(author: Author) {
        if (this.isAuthorized) {
            this.authService.delete(this.authorApiUrl + "/" + author.id).subscribe((resp: Response) => {
                    if (resp.status == 200) {
                        this.statusMessage = 'Deleted successfully!';
                        this.ngOnInit();
                    }
                },
                error => {
                    this.statusMessage = error;
                    console.log(error);
                    this.ngOnInit();
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
        if (page < 1 || page > this.pager.totalPages) {
            return;
        }
        // get pager object from service
        this.pager = this.pagerService.getPager(this.authors.length, page);

        // get current page of items
        this.pagedAuthorItems = this.authors.slice(this.pager.startIndex, this.pager.endIndex + 1);
    }
}