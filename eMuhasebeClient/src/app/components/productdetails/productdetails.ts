import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { HttpService } from '../../services/http.service';
import { ActivatedRoute } from '@angular/router';
import { initialProductModel, ProductModel } from '../../models/productmodels/product.model';
import { SharedModule } from '../../modules/shared.module';
import { ProductDetailsPipe } from '../../pipes/productdetail-pipe';

@Component({
  selector: 'app-productdetails',
  imports: [
    SharedModule,
    ProductDetailsPipe
  ],
  templateUrl: './productdetails.html',
  styleUrl: './productdetails.css'
})

export default class Productdetails {

  readonly product = signal<ProductModel>({ ...initialProductModel })
  readonly productId = signal<string>("")
  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #activated = inject(ActivatedRoute)

  constructor() {

    this.#activated.params.subscribe(res => {

      this.productId.set(res["id"])

    })

    this.getAll()

  }

  getAll() {

    this.#http.post<ProductModel>("ProductDetails/GetAll",
      {
        id: this.productId()
      },
      (res) => {
        this.product.set(res)
      })

  }

}
