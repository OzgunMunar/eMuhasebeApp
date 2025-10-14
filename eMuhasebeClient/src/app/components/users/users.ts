import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { UserPipe } from '../../pipes/user-pipe';
import { initialUserModel, UserModel } from '../../models/usermodels/user.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';
import { CompanyModel, initialCompanyModel } from '../../models/companymodels/company.model';

@Component({

  selector: 'app-users',
  imports: [
    SharedModule,
    UserPipe
  ],
  templateUrl: './users.html',
  styleUrl: './users.css'

})

export default class Users {

  readonly users = signal<UserModel[]>([{ ...initialUserModel }])
  readonly companies = signal<CompanyModel[]>([])
  readonly createModel = signal<UserModel>({ ...initialUserModel })
  readonly updateModel = signal<UserModel>({ ...initialUserModel })
  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)

  constructor() {

    this.getAll()
    this.getAllCompanies()

  }

  getAll() {
    this.#http.post<UserModel[]>("Users/GetAll", {}, (res) => {
      this.users.set(res)
    });
  }

  getAllCompanies() {
    this.#http.post<CompanyModel[]>("Company/GetAll", {}, (res) => {
      this.companies.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Users/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)

        this.createModel.set({ ...initialUserModel })
        this.createModalCloseBtn?.nativeElement.click()
        this.createModel.set({...initialUserModel})
        this.getAll()

      })

    }

  }

  deleteById(model: UserModel) {

    this.#swal.callSwal("Kullanıcıyı Sil?", `${model.fullName} isimli kullanıcıyı silmek istiyor musunuz?`, () => {

      this.#http.post<string>("Users/DeleteById", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: UserModel) {
    this.updateModel.set({ ...model })
    this.updateModel.update((val) => ({
      ...val,
      companyIds: val.companyUsers.map(value => value.companyId) 
    }))
  }

  update(form: NgForm) {

    if (form.valid) {

      if (this.updateModel().password === "") {
        this.updateModel.update((val) => ({
          ...val,
          password: null
        }))
      }

      this.#http.post<string>("Users/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.getAll()
        this.updateModel.set({...initialUserModel})

      })

    }

  }

}
