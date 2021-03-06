﻿import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Response, Headers } from '@angular/http';
import { DbConnection } from '../models/dbconnection';
import { Observable } from 'rxjs/Observable';
import { Config } from "../config";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class DbConnectionService {

    private url: string;

    constructor(private http: Http, private config: Config) {
        this.url = this.config.DbConnectionApiUrl;
    }

    sendConnectionString(obj: DbConnection) {
        console.log(obj);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        const body = JSON.stringify(obj);
        return this.http.post(this.url, body, { headers: headers }).catch((error: any) => { return Observable.throw(error); });
    }
}