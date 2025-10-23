import { Pipe, PipeTransform } from '@angular/core';
import { InvoiceModel } from '../models/invoicemodels/invoice.model';

@Pipe({
  name: 'invoicePipe'
})
export class InvoicePipe implements PipeTransform {

    transform(value: InvoiceModel[], search:string): InvoiceModel[] {
      if(!search) return value;
  
      return value.filter(p=> 
        p.customer.name.toLocaleLowerCase().includes(search.toLocaleLowerCase()) ||
        p.invoiceNumber.toLocaleLowerCase().includes(search.toLocaleLowerCase()) 
      );
    }

}
