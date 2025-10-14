import { Component, inject, signal } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { LoginModel } from '../../models/loginmodels/login.model';
import { HttpService } from '../../services/http.service';
import { Router } from '@angular/router';
import { LoginResponseModel } from '../../models/loginmodels/login.response.model';

@Component({
  selector: 'app-login',
  imports: [
    SharedModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})

export default class Login {

  readonly model = signal<LoginModel>(new LoginModel())
  readonly isLoading = signal<boolean>(false)

  readonly #http = inject(HttpService)
  readonly #router = inject(Router)

  signIn() {

    this.isLoading.set(true)
    this.#http.post<LoginResponseModel>("Auth/Login", this.model(), (res) => {
      
      localStorage.setItem("token", res.token);
      this.#router.navigateByUrl("/");

    }, 
      () => this.isLoading.set(false)
    )

  }

}
