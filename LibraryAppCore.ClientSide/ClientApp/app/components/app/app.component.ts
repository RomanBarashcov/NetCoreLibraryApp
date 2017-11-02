import { Component } from '@angular/core';
import { NgProgress } from 'ngx-progressbar';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    constructor(private ngProgress: NgProgress) {

        this.loadProgressBar();

    }

    loadProgressBar() {

        this.ngProgress.start();
        this.ngProgress.done();
        
    }
}
