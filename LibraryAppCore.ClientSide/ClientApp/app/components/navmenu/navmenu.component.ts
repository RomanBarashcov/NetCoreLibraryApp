import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css'],
    providers: [AccountService]
})
export class NavMenuComponent implements OnInit {
    constructor(public accountService: AccountService, public activateRoute: ActivatedRoute) {
        this.sub = activateRoute.params.subscribe();
        
    }

    isUserLogined: boolean = false;
    private sub: Subscription;

    ngOnInit() {
        var token = this.accountService.token;
        console.log("NavMenuToken:" + token);
        if (token != undefined) {
            this.isUserLogined = true;
        }
        else {
            this.isUserLogined = false;
        }
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }
    
}
