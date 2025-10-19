import { Pipe, PipeTransform } from '@angular/core';
import { CashRegisterDetailsModel } from '../models/cashregistermodels/cashregisterdetailsmodel/cashregisterdetails.model';

@Pipe({
  name: 'cashRegisterDetailsPipe'
})
export class CashRegisterDetailsPipe implements PipeTransform {

    transform(value: CashRegisterDetailsModel[], search:string): CashRegisterDetailsModel[] {
      if(!search) return value;
  
      return value.filter(p=> 
        p.description.toLocaleLowerCase().includes(search.toLocaleLowerCase()) 
      );
    }

}
