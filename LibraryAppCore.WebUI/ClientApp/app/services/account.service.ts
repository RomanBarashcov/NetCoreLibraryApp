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
    private url = "/Account";

    constructor(private http: Http, private router: Router) {
        var locStorage = localStorage.getItem('currentUser');
        if (locStorage != null) {
            var currentUser = JSON.parse(locStorage);
            this.token = currentUser && currentUser.token;
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
            
                console.log("createUser() Result: " + res);

                if (res.status == 200) {
                    // let login: LoginViewModel = new LoginViewModel(obj.Email, obj.Password, true, this.router.url);
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

    GenerateToken(obj: LoginViewModel) {
        
        const body = JSON.stringify(obj);
        console.log("GenerateToken Result: " + body);
        
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });

        return this.http.post(this.url + "/Token", body, { headers: headers }).map((resp: Response) => {

            console.log("GeneratedToken Result2: " + resp);
            return resp;

        }).catch(this.handleError);

        //return this.http.post(this.url + "/Token", body, { headers: headers })
        //    .map((res: Response) => {
            
        //        console.log("GenerateToken2 Result: " + res.status);
        //        let token = res.json() && res.json().token;
                
        //        if (token) {
        //            this.token = token;
        //            console.log("GenerateToken3 Result: " + token);
        //            localStorage.setItem('currentUser', JSON.stringify({ username: obj.Email, token: token }));
        //            return true;
        //        } else {
        //            return false;
        //        }

        //    }).catch(this.handleError);

    }
    
    private handleError(error: any) {
        console.error(error);
        return Observable.throw(error);
    }
}
