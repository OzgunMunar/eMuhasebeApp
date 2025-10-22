import { Pipe, PipeTransform } from '@angular/core';
import { ProductDetailsModel } from '../models/productmodels/productdetailsmodel/productdetails.model';

@Pipe({
  name: 'productDetailsPipe'
})
export class ProductDetailsPipe implements PipeTransform {

  transform(value: ProductDetailsModel[], search:string): ProductDetailsModel[] {
    if(!search) return value;

    return value.filter(p=> 
      p.description.toLocaleLowerCase().includes(search.toLocaleLowerCase())
    );
  }

}
