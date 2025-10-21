import { Pipe, PipeTransform } from '@angular/core';
import { BankModel } from '../models/bankmodels/bank.model';

@Pipe({
  name: 'bankPipe'
})
export class BankPipe implements PipeTransform {

  transform(value: BankModel[], search:string): BankModel[] {
    if(!search) return value;

    return value.filter(p=> 
      p.bankName.toLocaleLowerCase().includes(search.toLocaleLowerCase()) ||
      p.iban.includes(search)
    );
  }

}
