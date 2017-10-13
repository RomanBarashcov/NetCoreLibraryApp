import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css'],
    providers: [AuthService]
})
export class NavMenuComponent implements OnInit, OnDestroy {
    constructor(public authService: AuthService) {
    }

    isAuthorized: boolean;
    private isAuthorizedSubscription: Subscription;

    ngOnInit() {
        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
                this.isAuthorized = isAuthorized;
            });
    }

    ngOnDestroy(): void {
        this.isAuthorizedSubscription.unsubscribe();
    }

    public login() {
        this.authService.login();
    }

    public refreshSession() {
        this.authService.refreshSession();
    }

    public logout() {
        this.authService.logout();
    }
    
}
