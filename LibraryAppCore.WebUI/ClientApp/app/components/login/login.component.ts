import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { LoginViewModel } from '../../models/login';
import { Subscription } from 'rxjs/Subscription';



@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    providers: [AccountService]
})

export class LoginComponent implements OnDestroy {
    
    model: LoginViewModel = new LoginViewModel("","",false,"");
    loginUser: LoginViewModel;
    loading: boolean = false;
    error = '';
    private sub: Subscription;
    
    constructor(
        private router: Router,
        private accountService: AccountService, private activateRoute: ActivatedRoute) {
        this.sub = activateRoute.params.subscribe();
    }

    ngOnInit() {
        this.accountService.logout();
    }

    login() {
        this.loading = true;
        this.loginUser = new LoginViewModel(this.model.Email, this.model.Password, this.model.RememberMe, this.router.url);
        this.accountService.login(this.loginUser)
            .subscribe((resp: Response) => {
                console.log("LoginComponentResult: " + resp.status);
                if (resp.status == 200) {
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