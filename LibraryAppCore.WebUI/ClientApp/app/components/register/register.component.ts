import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { RegisterViewModel } from '../../models/register';
import { Subscription } from 'rxjs/Subscription';

@Component({
    selector: 'registration',
    templateUrl: './register.component.html',
    providers: [AccountService]
})

export class RegisterComponent implements OnDestroy{
    
    model: RegisterViewModel = new RegisterViewModel("","","");
    registerUser: RegisterViewModel;
    loading: boolean = false;
    error='';
    private sub: Subscription;

    constructor(
        private router: Router,
        private  accountService: AccountService,  private activateRoute: ActivatedRoute) {
        this.sub = activateRoute.params.subscribe();
    }

    ngOnInit() {
        this.accountService.logout();
    }

    Registration() {
        this.loading = true;
        this.registerUser = new RegisterViewModel(this.model.Email, this.model.Password, this.model.PasswordConfirm);
        this.accountService.Registration(this.registerUser)
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
