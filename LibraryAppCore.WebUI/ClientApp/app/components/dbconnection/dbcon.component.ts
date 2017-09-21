import { Component, OnDestroy } from '@angular/core';
import { DbConnection } from '../../models/dbconnection';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { DbConnectionService } from '../../services/dbconnection.service';

@Component({
    selector: 'chose-connection-string',
    templateUrl: 'dbcon.component.html',
    providers: [DbConnectionService]
})

export class DbConnectionComponent implements OnDestroy {

    private sub: Subscription;
    DefaultConnection: string = "DefaultConnection";
    MongoDbConnection: string = "MongoDbConnection";
    conStringDb: DbConnection;
    error: any;
    chosedDb: string;

    constructor(private serv: DbConnectionService, private activatedRoute: ActivatedRoute, private router: Router) {
        this.sub = activatedRoute.params.subscribe();
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