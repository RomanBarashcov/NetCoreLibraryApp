import { Component, OnDestroy } from '@angular/core';
import { DbConnection } from '../../models/dbconnection';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { DbConnectionService } from '../../services/dbconnection.service';
import { trigger, state, style, animate, transition, group } from '@angular/animations';

@Component({
    selector: 'chose-connection-string',
    templateUrl: 'dbcon.component.html',
    styleUrls: ['dbcon.component.css'],
    providers: [DbConnectionService],
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

export class DbConnectionComponent implements OnDestroy {
    
    private sub: Subscription;
    DefaultConnection: string = "DefaultConnection";
    MongoDbConnection: string = "MongoDbConnection";
    conStringDb: DbConnection;
    error: any;
    chosedDb: string;
    state: string = '';
    
    constructor(private serv: DbConnectionService, private activatedRoute: ActivatedRoute, private router: Router) {

        this.sub = activatedRoute.params.subscribe();

    }
    
    animateMe() {

        this.state = (this.state === '' ? 'in' : '');

    }
    
    choseDb(conString: string) {

        this.conStringDb = new DbConnection(conString);
        console.log("Chosed Db Connection: " + this.conStringDb);

        this.serv.sendConnectionString(this.conStringDb).subscribe(error => { this.error = error; console.log(error); });

        if (this.error != null) {

            this.chosedDb = " Chosed " + conString + " Db successful! You can chose any tab!";

        }
    }

    ngOnDestroy() {

        this.sub.unsubscribe();

    }
}