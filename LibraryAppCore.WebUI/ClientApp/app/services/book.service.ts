import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Response, Headers } from '@angular/http';
import { Book } from '../models/book';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class BookService {

    private url = "http://localhost:50201/BookApi";

    constructor(private http: Http) { }

    getBooks(): Observable<Book[]> {
        return this.http.get(this.url)
            .map((resp: Response) => {

                let bookList = resp.json();
                let books: Book[] = [];
                console.log("first  getBooks() Result: " + bookList)
                for (let index in bookList) {
                    console.log("second  getBooks() Result: " + bookList[index]);
                    let book = bookList[index];
                    books.push({ id: book.id, year: book.year, name: book.name, description: book.description, authorId: book.authorId });
                }
                return books;

            }).catch((error: any) => { return Observable.throw(error); });
    }

    getBookByAuthorId(id: string): Observable<Book[]> {

        return this.http.get(this.url + '/GetBookByAuthorId/' + id).map((resp: Response) => {

            let bookList = resp.json();
            let books: Book[] = [];
            console.log("first  getBookByAuthorId() Result: " + bookList)
            for (let index in bookList) {
                let book = bookList[index];
                console.log("second getBookByAuthorId() Result: " + bookList[index])
                books.push({ id: book.id, year: book.year, name: book.name, description: book.description, authorId: book.authorId });
            }
            return books;
        }).catch((error: any) => { return Observable.throw(error); });
    }
    createBook(obj: Book) {
        const body = JSON.stringify(obj);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        return this.http.post(this.url, body, { headers: headers })
            .map((res: Response) => {
                console.log("createBook() Result: " + res.status);
                return res;
            })
            .catch(this.handleError);
    }

    updateBook(id: string, obj: Book) {
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        const body = JSON.stringify(obj);
        return this.http.put(this.url + '/' + id, body, { headers: headers })
            .map((res: Response) => {
                console.log("updateBook() Result: " + res.status);
                return res;
            })
            .catch(this.handleError);
    }

    deleteBook(id: string) {
        return this.http.delete(this.url + '/' + id)
            .map((res: Response) => {
                console.log("deleteBook() Result: " + res.status);
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