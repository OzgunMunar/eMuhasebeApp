import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { CompanyPipe } from '../../pipes/company-pipe';
import { CompanyModel, initialCompanyModel } from '../../models/companymodels/company.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-companies',
  imports: [
    SharedModule,
    CompanyPipe
  ],
  templateUrl: './companies.html',
  styleUrl: './companies.css'
})

export default class Companies {

  readonly companies = signal<CompanyModel[]>([])
  readonly createModel = signal<CompanyModel>({ ...initialCompanyModel })
  readonly updateModel = signal<CompanyModel>({ ...initialCompanyModel })
  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)

  constructor() {
    this.getAll()
  }

  getAll() {
    this.#http.post<CompanyModel[]>("Company/GetAll", {}, (res) => {
      this.companies.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Company/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)

        this.createModel.set({ ...initialCompanyModel })
        this.createModalCloseBtn?.nativeElement.click()

        this.getAll()

      })

    }

  }

  deleteById(model: CompanyModel) {

    this.#swal.callSwal("Şirketi Sil?", `${model.companyName} şirketini silmek istiyor musunuz?`, () => {

      this.#http.post<string>("Company/DeleteById", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: CompanyModel) {
    this.updateModel.set({ ...model })
  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Company/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.getAll()

      })

    }

  }

  migrateAll() {

    this.#http.post("Company/MigrateAll", {}, (res: string) => {

      this.#swal.callToast(res);

    })

  }

}
