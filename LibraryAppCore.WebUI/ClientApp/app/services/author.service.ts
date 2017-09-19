import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Response, Headers } from '@angular/http';
import { Author } from '../models/author';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class AuthorService {

    private url = "http://localhost:50201/AuthorApi";

    constructor(private http: Http) { }

    getAuthors(): Observable<Author[]> {
        return this.http.get(this.url)
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
        const body = JSON.stringify(obj);
        console.log(body);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        return this.http.post(this.url, body, { headers: headers })
            .map((res: Response) => {
                console.log("createAuthor() Result: " + res.status);
                return res;
            })
            .catch(this.handleError);
    }

    updateAuthor(id: string, obj: Author) {
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        const body = JSON.stringify(obj);
        return this.http.put(this.url + '/' + id, body, { headers: headers })
            .map((res: Response) => {
                console.log("updateAuthor() Result: " + res.status);
                return res;
            })
            .catch(this.handleError);
    }

    deleteUser(id: string) {
        return this.http.delete(this.url + '/' + id)
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