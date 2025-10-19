import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { CashRegisterDetailsModel, initialCashRegisterDetailsModel } from '../../models/cashregistermodels/cashregisterdetailsmodel/cashregisterdetails.model';
import { SwalService } from '../../services/swal.service';
import { HttpService } from '../../services/http.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CashRegisterModel, initialCashRegisterModel } from '../../models/cashregistermodels/cashregister.model';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TrCurrencyPipe } from 'tr-currency';
import { CashRegisterDetailsPipe } from '../../pipes/cashregisterdetails-pipe';
import { SharedModule } from '../../modules/shared.module';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-cashregisterdetails',
  imports: [
    SharedModule,
    CashRegisterDetailsPipe,
    FormsModule,
    TrCurrencyPipe
  ],
  providers: [
    DatePipe
  ],
  templateUrl: './cashregisterdetails.html',
  styleUrl: './cashregisterdetails.css'
})

export default class Cashregisterdetails {

  readonly cashRegister = signal<CashRegisterModel>({ ...initialCashRegisterModel })
  readonly cashRegisters = signal<CashRegisterModel[]>([])
  readonly cashRegisterId = signal<string>("")
  readonly startDate = signal<string>("")
  readonly endDate = signal<string>("")

  readonly createModel = signal<CashRegisterDetailsModel>({ ...initialCashRegisterDetailsModel })
  readonly updateModel = signal<CashRegisterDetailsModel>({ ...initialCashRegisterDetailsModel })
  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)
  readonly #activated = inject(ActivatedRoute)
  readonly #date = inject(DatePipe)

  constructor() {

    this.#activated.params.subscribe(res => {

      this.cashRegisterId.set(res["id"])
      this.startDate.set(this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString())
      this.endDate.set(this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString())

    })

    this.getAll()
    this.getAllCashRegisters()

    this.createModel.set({
      ...this.createModel(),
      openedDate: this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString(),
      cashRegisterId: this.cashRegisterId()
    })

  }

  getAll() {

    this.#http.post<CashRegisterModel>("CashRegisterDetail/GetAll",
      {
        cashRegisterId: this.cashRegisterId(),
        startDate: this.startDate(),
        endDate: this.endDate()
      },
      (res) => {
        this.cashRegister.set(res)
      })

  }

  getAllCashRegisters() {
    this.#http.post<CashRegisterModel[]>("CashRegister/GetAll", {}, (res) => {
      this.cashRegisters.set(res.filter(p => p.id != this.cashRegisterId()))
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.createModel.set({
          ...this.createModel(),
          amount: +this.createModel().amount,
          oppositeAmount: +this.createModel().oppositeAmount,
      })

      if(this.createModel().recordType === 0) {

        this.createModel.set({
          ...this.createModel(),
          oppositeCashRegisterId: null
        })

      }

      this.#http.post<string>("CashRegisterDetail/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)
        this.createModalCloseBtn?.nativeElement.click()

        this.createModel.set({
          ...initialCashRegisterDetailsModel,
          openedDate: this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString(),
          cashRegisterId: this.cashRegisterId()
        })

        this.getAll()

      })

    }

  }

  deleteById(model: CashRegisterDetailsModel) {

    this.#swal.callSwal("Kasa Hareketini Sil?", `${model.openedDate} tarihteki ${model.description} kasa hareketini silmek istiyor musunuz?`, () => {

      this.#http.post<string>("CashRegisterDetail/Delete", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: CashRegisterDetailsModel) {

    model.amount = model.cashDepositAmount === 0 ? model.cashWithdrawalAmount : model.cashDepositAmount

    this.updateModel.set({ ...model })

  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("CashRegisterDetail/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.updateModel.set({ ...initialCashRegisterDetailsModel })
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

  setOppositeCashRegister() {

    const cash = this.cashRegisters().find(p => p.id === this.createModel().oppositeCashRegisterId);

    if (cash) {

      this.createModel.set({
        ...this.createModel(),
        oppositeCashRegister: cash
      })

    }

  }

}