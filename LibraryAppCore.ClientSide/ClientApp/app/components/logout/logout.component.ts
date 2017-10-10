import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
    selector: "logout",
    template: ``,
    providers: [AccountService]
})

export class LogOutComponent implements OnDestroy {

    error = '';
    private sub: Subscription;

    constructor(
        private router: Router,
        private accountService: AccountService, private activateRoute: ActivatedRoute) {
        this.sub = activateRoute.params.subscribe();
    }

    ngOnInit() {
        this.accountService.logout();
        this.router.navigate(['/home']);
    }
    
    ngOnDestroy() {
        this.sub.unsubscribe();
    }
}