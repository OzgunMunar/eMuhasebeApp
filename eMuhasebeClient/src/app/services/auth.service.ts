import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { JwtPayload, jwtDecode } from 'jwt-decode';
import { initialUserModel, UserModel } from '../models/usermodels/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  token: string = "";
  readonly user = signal<UserModel>({ ...initialUserModel })

  constructor(
    private router: Router
  ) { }

  isAuthenticated() {

    this.token = localStorage.getItem("token") ?? "";

    if (this.token === "") {
      this.router.navigateByUrl("/login");
      return false;
    }

    const decode: JwtPayload | any = jwtDecode(this.token);
    const exp = decode.exp;
    const now = new Date().getTime() / 1000;

    if (now > exp) {
      this.router.navigateByUrl("/login");
      return false;
    }

    this.user.update(u => ({

      ...u,
      id: decode["Id"],
      name: decode["Name"],
      email: decode["Email"],
      userName: decode["UserName"],
      companyId: decode["CompanyId"],
      companies: JSON.parse(decode["Companies"])

    }))

    return true;

  }

  refreshUserFromToken(token: string) {
    const decode: JwtPayload | any = jwtDecode(token);
    this.user.update(u => ({

      ...u,
      id: decode["Id"],
      name: decode["Name"],
      email: decode["Email"],
      userName: decode["UserName"],
      companyId: decode["CompanyId"],
      companies: JSON.parse(decode["Companies"])

    }))
  }

}