import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Author } from '../models/author';
import { Observable } from 'rxjs/Observable';
import { AccountService } from '../services/account.service';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class AuthorService {

    private url = "http://localhost:50795/AuthorApi";

    constructor(private http: Http, private accoutnService: AccountService) { }

    getAuthors(): Observable<Author[]> {

        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        headers.append('Authorization', 'Bearer ' + this.accoutnService.token );
        let options = new RequestOptions({ headers: headers });
        
        return this.http.get(this.url, options)
            .map((resp: Response) => {

                let authorList = resp.json();
                let authors: Author[] = [];
                console.log("First getAuthors() " + authorList);

                for (let index in authorList) {
                    console.log("Second getAuthors() " + authorList[index]);
                    let author = authorList[index];
                    authors.push({ id: author.id, name: author.name, surname: author.surname });
                }
                return authors;
            }).catch((error: any) => { return Observable.throw(error); });
    }

    createAuthor(obj: Author) {

        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        headers.append('Authorization', 'Bearer ' + this.accoutnService.token);
        let options = new RequestOptions({ headers: headers });

        const body = JSON.stringify(obj);
        console.log(body);

        return this.http.post(this.url, body, options)
            .map((res: Response) => {
                console.log("createAuthor() Result: " + res.status);
                return res;
            })
            .catch(this.handleError);
    }

    updateAuthor(id: string, obj: Author) {

        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        headers.append('Authorization', 'Bearer ' + this.accoutnService.token);
        let options = new RequestOptions({ headers: headers });

        const body = JSON.stringify(obj);
        return this.http.put(this.url + '/' + id, body, options)
            .map((res: Response) => {
                console.log("updateAuthor() Result: " + res.status);
                return res;
            })
            .catch(this.handleError);
    }

    deleteUser(id: string) {

        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        headers.append('Authorization', 'Bearer ' + this.accoutnService.token);
        let options = new RequestOptions({ headers: headers });
        
        return this.http.delete(this.url + '/' + id, options)
            .map((res: Response) => {
                console.log("deleteUser() Result: " + res.status);
                return res;
            })
            .catch(this.handleError);
    }

    private static json(res: Response): any {
        return res.text() === "" ? res : res.json();
    }

    private handleError(error: any) {
        console.error(error);
        return Observable.throw(error);
    }
}