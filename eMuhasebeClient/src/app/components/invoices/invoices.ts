import { Component, ElementRef, inject, signal, ViewChild } from "@angular/core"
import { HttpService } from "../../services/http.service"
import { SwalService } from "../../services/swal.service"
import { NgForm } from "@angular/forms"
import { SharedModule } from "../../modules/shared.module"
import { InvoicePipe } from "../../pipes/invoice-pipe"
import { initialInvoiceModel, InvoiceModel } from "../../models/invoicemodels/invoice.model"
import { TrCurrencyPipe } from "tr-currency"
import { CustomerModel, initialCustomerModel } from "../../models/customermodels/customer.model"
import { initialProductModel, ProductModel } from "../../models/productmodels/product.model"
import { initialInvoiceDetailModel, InvoiceDetailModel } from "../../models/invoicemodels/invoicedetailmodels/invoicedetail.model"
import { DatePipe } from "@angular/common"

@Component({
  selector: 'app-invoices',
  imports: [
    SharedModule,
    InvoicePipe,
    TrCurrencyPipe
  ],
  providers: [
    DatePipe
  ],
  templateUrl: './invoices.html',
  styleUrl: './invoices.css'
})

export default class Invoices {

  readonly invoices = signal<InvoiceModel[]>([{ ...initialInvoiceModel }])
  readonly customers = signal<CustomerModel[]>([{ ...initialCustomerModel }])
  readonly products = signal<ProductModel[]>([{ ...initialProductModel }])

  readonly createModel = signal<InvoiceModel>({ ...initialInvoiceModel })
  readonly updateModel = signal<InvoiceModel>({ ...initialInvoiceModel })

  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)
  readonly date = inject(DatePipe)

  constructor() {

    this.getAll()
    this.getAllCustomers()
    this.getAllProducts()

    this.createModel.set({

      ...this.createModel(),
      date: this.date.transform(new Date(), "yyyy-MM-dd") ?? ""

    })

  }

  getAllProducts() {

    this.#http.post<ProductModel[]>("Product/GetAll", {}, (res) => {

      this.products.set(res)
      
    })

  }

  getAllCustomers() {
    this.#http.post<CustomerModel[]>("Customer/GetAll", {}, (res) => {
      this.customers.set(res)
    });
  }

  getAll() {
    this.#http.post<InvoiceModel[]>("Invoice/GetAll", {}, (res) => {
      this.invoices.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Invoice/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)

        this.createModalCloseBtn?.nativeElement.click()
        this.createModel.set({

          ...initialInvoiceModel,
          date: this.date.transform(new Date(), "yyyy-MM-dd") ?? ""

        })

        this.getAll()

      })

    }

  }

  deleteById(model: InvoiceModel) {

    this.#swal.callSwal("Faturayı Sil?", `${model.invoiceNumber} verisini silmek istiyor musunuz?`, () => {

      this.#http.post<string>("Invoice/Delete", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: InvoiceModel) {
    this.updateModel.set({ ...model })
  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Invoice/Delete", { id: this.updateModel().id }, (res) => {

        this.updateModel.set({
          ...this.updateModel(),
          id: ""
        })

        this.#http.post<string>("Invoice/Create", this.updateModel(), (res) => {

          this.#swal.callToast(res)

          this.createModalCloseBtn?.nativeElement.click()
          this.createModel.set({ ...initialInvoiceModel })

        })

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    }

  }

  addDetail() {

    const detail: InvoiceDetailModel = {

      ...initialInvoiceDetailModel,
      price: this.createModel().price,
      quantity: this.createModel().quantity,
      productId: this.createModel().productId,
      product: this.products().find(p => p.id == this.createModel().productId) ?? {...initialProductModel}

    }

    console.log("detail:", detail)
    console.log("createMode:", this.createModel())

    this.createModel.update(model => ({

      ...model,
      productId: "",
      quantity: 0,
      price: 0,
      details: [...model.details, detail]

    }))

  }

  removeItemByIndex(index: number) {

    // index'e göre filtreleyip, seçilen index numaralı kaydı dışarı alıyorum.

    this.createModel.update(model => ({
      ...model,
      details: model.details.filter((_, i) => i !== index)
    }));

  }

}