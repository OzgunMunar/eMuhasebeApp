import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { CustomerPipe } from '../../pipes/customer-pipe';
import { CustomerModel, initialCustomerModel } from '../../models/customermodels/customer.model';
import { CustomerTypes } from '../../models/commonmodels/smartenum-type.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-customers',
  imports: [
    SharedModule,
    CustomerPipe,
    RouterLink
  ],
  templateUrl: './customers.html',
  styleUrl: './customers.css'
})

export default class Customers {


  readonly customers = signal<CustomerModel[]>([])
  readonly createModel = signal<CustomerModel>({ ...initialCustomerModel })
  readonly updateModel = signal<CustomerModel>({ ...initialCustomerModel })
  search: string = ""
  customerTypes = CustomerTypes
  p: number = 1

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)

  constructor() {
    this.getAll()
  }

  getAll() {
    this.#http.post<CustomerModel[]>("Customer/GetAll", {}, (res) => {
      this.customers.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Customer/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)

        this.createModel.set({ ...initialCustomerModel })
        this.createModalCloseBtn?.nativeElement.click()
        this.createModel.set({ ...initialCustomerModel })
        this.getAll()

      })

    }

  }

  deleteById(model: CustomerModel) {

    this.#swal.callSwal("Cariyi Sil?", `${model.name} isimli cariyi silmek istiyor musunuz?`, () => {

      this.#http.post<string>("Customer/Delete", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: CustomerModel) {

    this.updateModel.set({ 
      ...model,
      typeValue: this.updateModel().type.value
    })

  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Customer/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.updateModel.set({ ...initialCustomerModel })
        this.getAll()

      })

    }

  }

  changeCurrencyNameToSymbol(name: string) {

    if (name === "TL") return "₺"
    else if (name === "USD") return "$"
    else if (name === "EUR") return "€"
    else return ""

  }


}
