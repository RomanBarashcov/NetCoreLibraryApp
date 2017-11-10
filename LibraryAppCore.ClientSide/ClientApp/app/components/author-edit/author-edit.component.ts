import { Component, OnInit ,OnDestroy, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Response } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { Author } from '../../models/author';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs/Observable';
import { Config } from '../../config';
import 'rxjs/Rx';
import * as _ from 'underscore';
import { trigger, state, style, animate, transition, group } from '@angular/animations';
import { NgProgress } from 'ngx-progressbar';

@Component({
    selector: 'author-edit-app',
    templateUrl: 'author-edit.component.html',
    styleUrls: ['author-edit.component.css'],
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
export class AuthorEditComponent implements OnInit, OnDestroy{

    private authorApiUrl: string;
    authors: Author[] = [];
    editedAuthor: Author;
    editAuthorNull: Author;
    isNewRecord: boolean;
    statusMessage: string;
    private sub: Subscription;
    error: any;
    state: string = '';

    isAuthorized: boolean;
    private isAuthorizedSubscription: Subscription;

    constructor(private route: ActivatedRoute, private authService: AuthService, private config: Config, private router: Router, private ngProgress: NgProgress) {

        this.ngProgress.start();
        this.authorApiUrl = this.config.AuthorsApiUrl;
    }

    ngOnInit() {

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe((isAuthorized: boolean) => {

            this.isAuthorized = isAuthorized;
        });

        this.sub = this.route.params.subscribe(params => {
            var id = params['id']; 

            if (id) {

                this.editAuthor(id);
            }
            else {

                this.addAuthor()
            }
        });

        this.animate();
        this.state = "in";
    }

    ngOnDestroy() {

        this.sub.unsubscribe();
    }

    animate() {

        this.state = (this.state === '' ? 'in' : '');
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

        this.ngProgress.done();
        this.animate();
        this.state = "in";
    }

    editAuthor(authorId: string) {

        this.editedAuthor = new Author("", "", "");

        if (this.isAuthorized) {

            this.authService.get(this.authorApiUrl + "/GetAuthorById/" + authorId).subscribe((resp: Response) => {

                this.editedAuthor = resp.json();

            },
                error => {

                    console.log(error);
                });

        }
        else {

            this.statusMessage = "Please log in!";
        }

        this.ngProgress.done();
        this.animate();
        this.state = "in";
    }

    saveAuthor() {

        this.ngProgress.start();

        if (this.isNewRecord) {

            this.authService.post(this.authorApiUrl, this.editedAuthor).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Saved successfully!';
                    this.router.navigate(['/author']);
                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                });

            this.isNewRecord = false;

        } else {

            this.authService.put(this.authorApiUrl + "/" + this.editedAuthor.id, this.editedAuthor).subscribe((resp: Response) => {

                if (resp.status == 200) {

                    this.statusMessage = 'Updated successfully!';
                    this.router.navigate(['/author']);
                }
            },
                error => {

                    this.statusMessage = error + ' Check all your data, and try again! ';
                    console.log(error);
                });
        }
    }

    cancel() {

        this.router.navigate(['/author']);
    }
}