import { Pipe, PipeTransform } from '@angular/core';
import { BankDetailsModel } from '../models/bankmodels/bankdetailmodel/bankdetail.model';

@Pipe({
  name: 'bankDetailsPipe'
})
export class BankDetailsPipe implements PipeTransform {

    transform(value: BankDetailsModel[], search:string): BankDetailsModel[] {
      if(!search) return value;
  
      return value.filter(p=> 
        p.description.toLocaleLowerCase().includes(search.toLocaleLowerCase()) 
      );
    }

}
