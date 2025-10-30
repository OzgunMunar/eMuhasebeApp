import { Component, inject, signal } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { HttpService } from '../../services/http.service';
import { ProductProfitabilityModel } from '../../models/productprofitabilitymodel/productprofitability.model';

@Component({
  selector: 'app-product-profitability-report',
  imports: [
    SharedModule
  ],
  templateUrl: './product-profitability-report.html',
  styleUrl: './product-profitability-report.css'
})

export default class ProductProfitabilityReport {

  readonly data = signal<ProductProfitabilityModel[]>([])
  readonly #http = inject(HttpService)

  constructor(){
    this.get()
  }

  get(){
    
    this.#http.get<ProductProfitabilityModel[]>("Reports/ProductProfitabilityReport", (res)=> {

      this.data.set(res)

    })
    
  }

}
