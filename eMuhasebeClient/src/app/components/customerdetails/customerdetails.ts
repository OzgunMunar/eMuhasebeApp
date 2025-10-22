import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { CustomerDetailsPipe } from '../../pipes/customerdetails-pipe';
import { TrCurrencyPipe } from 'tr-currency';
import { DatePipe } from '@angular/common';
import { CustomerModel, initialCustomerModel } from '../../models/customermodels/customer.model';
import { CustomerDetailsModel, initialCustomerDetailsModel } from '../../models/customermodels/customerdetailsmodel/customerdetails.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-customerdetails',
  imports: [
    SharedModule,
    CustomerDetailsPipe,
    TrCurrencyPipe
  ],
  templateUrl: './customerdetails.html',
  styleUrl: './customerdetails.css'
})

export default class Customerdetails {

  readonly customer = signal<CustomerModel>({ ...initialCustomerModel })
  readonly customerId = signal<string>("")
  search: string = ""

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #activated = inject(ActivatedRoute)

  constructor() {

    this.#activated.params.subscribe(res => {

      this.customerId.set(res["id"])

    })

    this.getAll()

  }

  getAll() {

    this.#http.post<CustomerModel>("CustomerDetails/GetAll",
      {
        customerId: this.customerId()
      },
      (res) => {
        this.customer.set(res)
        console.log(this.customer())
      })

  }

}
