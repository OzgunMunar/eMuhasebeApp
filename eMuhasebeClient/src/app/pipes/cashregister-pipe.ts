import { Pipe, PipeTransform } from '@angular/core';
import { CashRegisterModel } from '../models/cashregistermodels/cashregister.model';

@Pipe({
  name: 'cashregisterPipe'
})
export class CashregisterPipe implements PipeTransform {

  transform(value: CashRegisterModel[], search:string): CashRegisterModel[] {
    if(!search) return value;

    return value.filter(p=> 
      p.cashRegisterName.toLocaleLowerCase().includes(search.toLocaleLowerCase()) 
    );
  }

}
