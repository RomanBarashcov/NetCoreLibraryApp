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
    
    model: RegisterViewModel[] = [];
    registerUser: RegisterViewModel;
    loading: boolean = false;
    error='';
    private sub: Subscription;

    constructor(
        private router: Router,
        private  accountService: AccountService,  private activateRoute: ActivatedRoute) {
        this.sub = activateRoute.params.subscribe();
        this.registerUser = new RegisterViewModel("","","");
        this.model.push(this.registerUser);
    }

    ngOnInit() {
        this.accountService.logout();
    }

    Registration(obj: RegisterViewModel) {
        this.loading = true;
        this.accountService.Registration(obj)
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
