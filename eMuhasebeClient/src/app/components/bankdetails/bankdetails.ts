import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { BankDetailsModel, initialBankDetailsModel } from '../../models/bankmodels/bankdetailmodel/bankdetail.model';
import { SwalService } from '../../services/swal.service';
import { HttpService } from '../../services/http.service';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TrCurrencyPipe } from 'tr-currency';
import { BankDetailsPipe } from '../../pipes/bankdetails-pipe';
import { SharedModule } from '../../modules/shared.module';
import { DatePipe } from '@angular/common';
import { BankModel, initialBankModel } from '../../models/bankmodels/bank.model';
import { CashRegisterModel } from '../../models/cashregistermodels/cashregister.model';
import { CustomerModel } from '../../models/customermodels/customer.model';

@Component({

  selector: 'app-bankdetails',
  imports: [
    SharedModule,
    BankDetailsPipe,
    FormsModule,
    TrCurrencyPipe
  ],
  providers: [
    DatePipe
  ],
  templateUrl: './bankdetails.html',
  styleUrl: './bankdetails.css'

})

export default class BankDetails {

  readonly bank = signal<BankModel>({ ...initialBankModel })
  readonly banks = signal<BankModel[]>([])
  readonly cashRegisters = signal<CashRegisterModel[]>([])
  readonly customers = signal<CustomerModel[]>([])

  readonly bankId = signal<string>("")
  readonly startDate = signal<string>("")
  readonly endDate = signal<string>("")

  readonly createModel = signal<BankDetailsModel>({ ...initialBankDetailsModel })
  readonly updateModel = signal<BankDetailsModel>({ ...initialBankDetailsModel })
  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)
  readonly #activated = inject(ActivatedRoute)
  readonly #date = inject(DatePipe)

  constructor() {

    this.#activated.params.subscribe(res => {

      this.bankId.set(res["id"])
      this.startDate.set(this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString())
      this.endDate.set(this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString())

    })

    this.getAll()
    this.getAllBanks()
    this.getAllCashRegisters()
    this.getAllCustomers()

    this.createModel.set({
      ...this.createModel(),
      openedDate: this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString(),
      bankId: this.bankId()
    })

  }

  getAll() {

    this.#http.post<BankModel>("BankDetail/GetAll",
      {
        bankId: this.bankId(),
        startDate: this.startDate(),
        endDate: this.endDate()
      },
      (res) => {
        this.bank.set(res)
      })

  }

  getAllBanks() {
    this.#http.post<BankModel[]>("Bank/GetAll", {}, (res) => {
      this.banks.set(res.filter(p => p.id != this.bankId()))
    });
  }

  getAllCashRegisters() {
    this.#http.post<CashRegisterModel[]>("CashRegister/GetAll", {}, (res) => {
      this.cashRegisters.set(res)
    });
  }

  getAllCustomers() {
    this.#http.post<CustomerModel[]>("Customer/GetAll", {}, (res) => {
      this.customers.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.createModel.set({
        ...this.createModel(),
        amount: +this.createModel().amount,
        oppositeAmount: +this.createModel().oppositeAmount,
      })

      if (this.createModel().recordType == 0) {

        this.createModel.set({
          ...this.createModel(),
          oppositeBankId: null,
          oppositeCashRegisterId: null,
          oppositeCustomerId: null
        })

      } else if (this.createModel().recordType == 1) {

        this.createModel.set({
          ...this.createModel(),
          oppositeCashRegisterId: null,
          oppositeCustomerId: null
        })

      } else if (this.createModel().recordType == 2) {

        this.createModel.set({
          ...this.createModel(),
          oppositeBankId: null
        })

      } else if (this.createModel().recordType == 3) {

        this.createModel.set({
          ...this.createModel(),
          oppositeBankId: null,
          oppositeCashRegisterId: null
        })

      }

      this.#http.post<string>("BankDetail/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)
        this.createModalCloseBtn?.nativeElement.click()

        this.createModel.set({
          ...initialBankDetailsModel,
          openedDate: this.#date.transform(new Date(), 'yyyy-MM-dd')!.toString(),
          bankId: this.bankId()
        })

        this.getAll()

      })

    }

  }

  deleteById(model: BankDetailsModel) {

    this.#swal.callSwal("Banka Hareketini Sil?", `${model.openedDate} tarihteki ${model.description} banka hareketini silmek istiyor musunuz?`, () => {

      this.#http.post<string>("BankDetail/Delete", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: BankDetailsModel) {

    model.amount = model.bankDepositAmount === 0 ? model.bankWithdrawalAmount : model.bankDepositAmount

    this.updateModel.set({ ...model })

  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("BankDetail/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.updateModel.set({ ...initialBankDetailsModel })
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

  setOppositeBank() {

    const bank = this.banks().find(p => p.id === this.createModel().oppositeBankId);

    if (bank) {

      this.createModel.set({
        ...this.createModel(),
        oppositeBank: bank
      })

    }

  }

  setOppositeCash() {

    const cash = this.cashRegisters().find(p => p.id === this.createModel().oppositeCashRegisterId);

    if (cash) {

      this.createModel.set({
        ...this.createModel(),
        oppositeCash: cash
      })

    }

  }

}