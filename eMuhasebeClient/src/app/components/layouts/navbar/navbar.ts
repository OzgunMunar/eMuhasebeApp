import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { HttpService } from '../../../services/http.service';
import { LoginResponseModel } from '../../../models/loginmodels/login.response.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-navbar',
  imports: [
    FormsModule
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})

export class Navbar {

  readonly #router = inject(Router)
  readonly #http = inject(HttpService)
  readonly auth = inject(AuthService)
  readonly companyId = signal<string>("")

  logOut() {

    localStorage.clear()
    this.#router.navigateByUrl("/login")

  }

  constructor() {
    this.companyId.set(this.auth.user().companyId)
    console.table(this.auth.user())
  }

  changeCompany() {

    this.companyId.set(this.auth.user().companyId)

    this.#http.post<LoginResponseModel>(
      "Auth/ChangeCompany", 
      { companyId: this.companyId() },
      res => {
        
        localStorage.clear()
        localStorage.setItem("token", res.token)
        document.location.reload()

      })

  }

}
