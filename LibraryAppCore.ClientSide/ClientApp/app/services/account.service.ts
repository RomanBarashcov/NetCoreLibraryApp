import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { LoginViewModel } from "../models/login"; 
import { RegisterViewModel } from "../models/register";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class AccountService {
    
    public token: string;
    private url = "http://localhost:52659/Account";
    lStorage: any;

    constructor(private http: Http, private router: Router) {

        try {
            this.lStorage = localStorage.getItem('currentUser');
            var currentUser = JSON.parse(this.lStorage);
            this.token = currentUser && currentUser.token;
        }
        catch (e){
            console.log("Error: " + e);
        }
     
    }

    login(obj: LoginViewModel) {

        const body = JSON.stringify(obj);
        console.log(body);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        
        return this.http.post(this.url + "/Login", body, { headers: headers })
            .map((res: Response) => {

                var result = res;
                if (res.status == 200) {
                    let token = res.json() && res.json().token;
                    if (token) {
                        this.token = token;
                        console.log("GenerateToken3 Result: " + token);
                        localStorage.setItem('currentUser', JSON.stringify({ username: obj.Email, token: token }));
                        return result;
                    } else {
                        return result.status = 400;
                    }
                }
                return result;

            }).catch(this.handleError);
    }
    
    Registration(obj: RegisterViewModel) {

        const body = JSON.stringify(obj);
        console.log(body);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });

        return this.http.post(this.url + "/Register", body, { headers: headers })
            .map((res: Response) => {

                if (res.status == 200) {
                    let token = res.json() && res.json().token;
                    if (token) {
                        this.token = token;
                        console.log("GenerateToken3 Result: " + token);
                        localStorage.setItem('currentUser', JSON.stringify({ username: obj.Email, token: token }));
                        return true;
                    } else {
                        return false;
                    }
                }
             return res;  
            }).catch(this.handleError);
    }
    
    logout(): void {
        this.token = "";
        localStorage.removeItem('currentUser');
        console.log("logout");
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        this.http.post(this.url + "/LogOff", { headers: headers }).map((res: Response) => {
                    console.log("LogOff Result:" + res);
                });
    }

    private handleError(error: any) {
        console.error(error);
        return Observable.throw(error);
    }
}
