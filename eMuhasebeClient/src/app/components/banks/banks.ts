import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SwalService } from '../../services/swal.service';
import { HttpService } from '../../services/http.service';
import { NgForm } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SharedModule } from '../../modules/shared.module';
import { BankPipe } from '../../pipes/bank-pipe';
import { BankModel, initialBankModel } from '../../models/bankmodels/bank.model';
import { CurrencyTypes } from '../../models/commonmodels/smartenum-type.model';

@Component({

  selector: 'app-banks',
  imports: [
    SharedModule,
    BankPipe,
    RouterLink
  ],
  templateUrl: './banks.html',
  styleUrl: './banks.css'

})

export default class Banks {

  readonly banks = signal<BankModel[]>([])
  readonly createModel = signal<BankModel>({ ...initialBankModel })
  readonly updateModel = signal<BankModel>({ ...initialBankModel })
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
    this.#http.post<BankModel[]>("Bank/GetAll", {}, (res) => {
      this.banks.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Bank/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)

        this.createModel.set({ ...initialBankModel })
        this.createModalCloseBtn?.nativeElement.click()
        this.createModel.set({ ...initialBankModel })
        this.getAll()

      })

    }

  }

  deleteById(model: BankModel) {

    this.#swal.callSwal("Bankayı Sil?", `${model.bankName} bankasını silmek istiyor musunuz?`, () => {

      this.#http.post<string>("Bank/DeleteById", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: BankModel) {

    this.updateModel.set({ ...model })

  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Bank/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.updateModel.set({ ...initialBankModel })
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