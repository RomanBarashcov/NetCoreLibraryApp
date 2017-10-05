import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { Login } from '../../models/login';
import { Subscription } from 'rxjs/Subscription';


@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    providers: [AccountService]
})

export class LoginComponent implements OnDestroy {
    
    model: Login[] = [];
    loginUser: Login;
    loading: boolean = false;
    error = '';
    private sub: Subscription;
    
    constructor(
        private router: Router,
        private  accountService: AccountService,  private activateRoute: ActivatedRoute) {
        this.sub = activateRoute.params.subscribe();
        this.loginUser = new Login("","",false,"");
        this.model.push(this.loginUser);
    }

    ngOnInit() {
        this.accountService.logout();
    }

    login(obj: Login) {
        this.loading = true;
        obj.ReturnUrl = this.router.url;
        this.accountService.login(obj)
            .subscribe(result => {
                if (result === true) {
                    this.router.navigate(['/home']);
                } else {
                    this.error = 'Username or password is incorrect';
                    this.loading = false;
                }
            });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }
}