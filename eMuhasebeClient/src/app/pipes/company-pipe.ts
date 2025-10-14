import { Pipe, PipeTransform } from '@angular/core';
import { CompanyModel } from '../models/companymodels/company.model';

@Pipe({
  name: 'companyPipe'
})
export class CompanyPipe implements PipeTransform {

      transform(value: CompanyModel[], search:string): CompanyModel[] {
  
        if(!search) return value;
    
        return value.filter(p=> 
          p.companyName.toLocaleLowerCase().includes(search.toLocaleLowerCase()) ||
          p.fullAddress.toLocaleLowerCase().includes(search.toLocaleLowerCase()) ||
          p.taxDepartment.toLocaleLowerCase().includes(search.toLocaleLowerCase()) ||
          p.taxNumber.toString().includes(search.toLocaleLowerCase()) 
        );
      }

}
