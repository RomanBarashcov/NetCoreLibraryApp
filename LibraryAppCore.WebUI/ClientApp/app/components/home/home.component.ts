import { Component } from '@angular/core';
import {
    trigger,
    state,
    style,
    animate,
    transition,
    group
} from '@angular/animations';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    animations: [
        trigger('flyInOut', [
            state('in', style({transform: 'translateX(0)', opacity: 1})),
            transition('void => *', [
                style({transform: 'translateX(0px)', opacity: 0}),
                group([
                    animate('1s 0.1s ease', style({
                        transform: 'translateX(0)',

                    })),
                    animate('1s ease', style({
                        opacity: 1
                    }))
                ])
            ]),
            transition('* => void', [
                group([
                    animate('1s ease', style({
                        transform: 'translateX(0px)',

                    })),
                    animate('1s 0.2s ease', style({
                        opacity: 0
                    }))
                ])
            ])
        ])
    ]
})
export class HomeComponent {
    state: string = '';

    animateMe() {
        this.state = (this.state === '' ? 'in' : '');
    }
}
