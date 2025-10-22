import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { ProductPipe } from '../../pipes/product-pipe';
import { RouterLink } from '@angular/router';
import { SharedModule } from '../../modules/shared.module';
import { initialProductModel, ProductModel } from '../../models/productmodels/product.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-products',
  imports: [
    SharedModule,
    ProductPipe,
    RouterLink
  ],
  templateUrl: './products.html',
  styleUrl: './products.css'
})

export default class Products {

  readonly products = signal<ProductModel[]>([])
  readonly createModel = signal<ProductModel>({ ...initialProductModel })
  readonly updateModel = signal<ProductModel>({ ...initialProductModel })
  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)

  constructor() {
    this.getAll()
  }

  getAll() {
    this.#http.post<ProductModel[]>("Product/GetAll", {}, (res) => {
      this.products.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Product/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)

        this.createModalCloseBtn?.nativeElement.click()
        this.createModel.set({ ...initialProductModel })
        this.getAll()

      })

    }

  }

  deleteById(model: ProductModel) {

    this.#swal.callSwal("Stoğu Sil?", `${model.name} stoğunu silmek istiyor musunuz?`, () => {

      this.#http.post<string>("Product/Delete", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: ProductModel) {

    this.updateModel.set({ ...model })

  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Product/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.updateModel.set({ ...initialProductModel })
        this.getAll()

      })

    }

  }

}
