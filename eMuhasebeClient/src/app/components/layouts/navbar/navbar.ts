import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})

export class Navbar {

  readonly #router = inject(Router)

  logOut() {

    localStorage.clear()
    this.#router.navigateByUrl("/login")

  }

}
