import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { MainSidebar } from './main-sidebar/main-sidebar';
import { Footer } from './footer/footer';
import { ControlSidebar } from './control-sidebar/control-sidebar';

@Component({
  selector: 'app-layouts',
  imports: [
    RouterOutlet,
    Navbar,
    MainSidebar,
    Footer,
    ControlSidebar
  ],
  templateUrl: './layouts.html',
  styleUrl: './layouts.css'
})

export default class Layouts {

}
