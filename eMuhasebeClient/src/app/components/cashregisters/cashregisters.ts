import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { CashregisterPipe } from '../../pipes/cashregister-pipe';
import { CashRegisterModel, initialCashRegisterModel } from '../../models/cashregistermodels/cashregister.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';
import { CurrencyTypes } from '../../models/commonmodels/smartenum-type.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-cashregisters',
  imports: [
    SharedModule,
    CashregisterPipe,
    RouterLink
  ],
  templateUrl: './cashregisters.html',
  styleUrl: './cashregisters.css'
})

export default class Cashregisters {

  readonly cashRegisters = signal<CashRegisterModel[]>([])
  readonly createModel = signal<CashRegisterModel>({ ...initialCashRegisterModel })
  readonly updateModel = signal<CashRegisterModel>({ ...initialCashRegisterModel })
  search: string = ""
  currencyTypes = CurrencyTypes

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)

  constructor() {
    this.getAll()
  }

  getAll() {
    this.#http.post<CashRegisterModel[]>("CashRegister/GetAll", {}, (res) => {
      this.cashRegisters.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("CashRegister/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)

        this.createModel.set({ ...initialCashRegisterModel })
        this.createModalCloseBtn?.nativeElement.click()
        this.createModel.set({ ...initialCashRegisterModel })
        this.getAll()

      })

    }

  }

  deleteById(model: CashRegisterModel) {

    this.#swal.callSwal("Kasayı Sil?", `${model.cashRegisterName} kasasını silmek istiyor musunuz?`, () => {

      this.#http.post<string>("CashRegister/DeleteById", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: CashRegisterModel) {
    
    this.updateModel.set({ ...model })
   
  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("CashRegister/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.updateModel.set({ ...initialCashRegisterModel })
        this.getAll()

      })

    }

  }

  changeCurrencyNameToSymbol(name: string) {
    
    if(name === "TL") return "₺"
    else if(name === "USD") return "$"
    else if(name === "EUR") return "€"
    else return ""

  }

}
