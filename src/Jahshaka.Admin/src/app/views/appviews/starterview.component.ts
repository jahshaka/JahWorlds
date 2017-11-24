import { Component, OnDestroy, OnInit, Injectable, } from '@angular/core';

@Component({
  selector: 'starter',
  templateUrl: 'starter.template.html'
})

@Injectable()
export class StarterViewComponent implements OnDestroy, OnInit  {

  public nav: any;

  public constructor() {
    this.nav = document.querySelector('nav.navbar');
  }

  public ngOnInit(): any {
    this.nav.className += ' white-bg';
  }


  public ngOnDestroy(): any {
    this.nav.classList.remove('white-bg');
  }

}
