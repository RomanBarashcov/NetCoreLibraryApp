import { TemplateRef, ViewChild, Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { AuthorService } from '../../services/author.service';
import { Author } from '../../models/author';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { PagerService } from '../../services/pagination.service';
import { trigger, state, style, animate, transition, group } from '@angular/animations';


@Component({
    selector: 'authors-app',
    templateUrl: 'author.component.html',
    styleUrls: ['author.component.css'],
    providers: [AuthorService],
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
export class AuthorComponent implements OnDestroy, OnInit {

    state: string = '';

    animate() {
        this.state = (this.state === '' ? 'in' : '');
    }
    
    @ViewChild('readOnlyTemplate') readOnlyTemplate: TemplateRef<any>;
    @ViewChild('editTemplate') editTemplate: TemplateRef<any>;

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

    constructor(private serv: AuthorService, private router: Router, private activateRoute: ActivatedRoute, private pagerService: PagerService) {
        this.sub = activateRoute.params.subscribe();
    }

    ngOnInit() {
        this.serv.getAuthors().subscribe(data => {
            this.authors = data;
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
        this.editedAuthor = new Author("", "", "");
        this.authors.push(this.editedAuthor);
        this.pagedAuthorItems = this.authors;
        this.isNewRecord = true;
        if (this.pager.totalPages > 0) {
            this.setPage(this.pager.totalPages);
        }
    }

    editAuthor(author: Author) {
        this.editedAuthor = new Author(author.id, author.name, author.surname);
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
            this.serv.createAuthor(this.editedAuthor).subscribe((resp: Response) => {
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
            this.serv.updateAuthor(this.editedAuthor.id, this.editedAuthor).subscribe((resp: Response) => {
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
        this.serv.deleteUser(author.id).subscribe((resp: Response) => {
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

    routeToBooks(author: Author) {
        this.router.navigate(['/bookByAuthor', author.id]);
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
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