import { Pipe, PipeTransform } from '@angular/core';
import { CustomerDetailsModel } from '../models/customermodels/customerdetailsmodel/customerdetails.model';

@Pipe({
  name: 'customerDetailsPipe'
})

export class CustomerDetailsPipe implements PipeTransform {

      transform(value: CustomerDetailsModel[], search:string): CustomerDetailsModel[] {
  
        if(!search) return value;
    
        return value.filter(p=> 
          p.description.toLocaleLowerCase().includes(search.toLocaleLowerCase()) 
        );
      }

}
