import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Login } from "../models/login"; 
import { RegisterViewModel } from "../models/register";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class AccountService {
    
    public token: string;
    private url = "/Account";

    constructor(private http: Http) {
        
        //var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        //this.token = currentUser && currentUser.token;
    }

    login(obj: Login) : Observable<boolean> {
        const body = JSON.stringify(obj);
        console.log(body);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        
        return this.http.post(this.url + "/Login", body, { headers: headers })
            .map((res: Response) => {
                console.log("loginService() Result: " + res.status);
                
                if (res.status == 200){
                    var result = this.getToken(obj);
                    return result;
                }
                
                return res;
            }).catch(this.handleError);
    }
    
    Registration(obj: RegisterViewModel) : Observable<boolean> {
        const body = JSON.stringify(obj);
        console.log(body);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        return this.http.post(this.url + "/Register", body, { headers: headers })
            .map((res: Response) => {
            
                console.log("createUser() Result: " + res.status);
                if (res.status == 200){
                    
                    let login: Login = new Login(obj.Email, obj.Email, true, "");
                    return this.getToken(login);
                }
                return res;
                
            }).catch(this.handleError);
    }
    
    logout(): void {
        this.token = "";
        localStorage.removeItem('currentUser');
    }

    getToken(obj: Login) : Observable<boolean> {
        
        const body = JSON.stringify(obj);
        console.log(body);
        let headers = new Headers({ 'Content-Type': 'application/json;charser=utf8' });
        
        return this.http.post(this.url + "/Token", body, { headers: headers })
            .map((res: Response) => {
            
                console.log("createAuthor() Result: " + res.status);
                let token = res.json() && res.json().token;
                
                if (token) {
                    this.token = token;
                    localStorage.setItem('currentUser', JSON.stringify({ username: obj.Email, token: token }));
                    return true;
                } else {
                    return false;
                }

            }).catch(this.handleError);
    }
    
    private handleError(error: any) {
        console.error(error);
        return Observable.throw(error);
    }
}
