import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Response, Headers } from '@angular/http';
import { DbConnection } from '../models/dbconnection';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';


@Injectable()
export class DbConnectionService {

    private url = "http://localhost:50201/?=";

    constructor(private http: Http) { }

    sendConnectionString(obj: DbConnection) {
        console.log(obj);

        return this.http.get("http://localhost:50201/?=" + DbConnection).catch((error: any) => { return Observable.throw(error); });
    }
}