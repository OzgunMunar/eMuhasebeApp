import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TrCurrencyPipe } from 'tr-currency';
import { FlexiGridModule } from 'flexi-grid';
import { FlexiSelectModule } from 'flexi-select';
import BlankComponent from '../components/blank/blank';
import { Section } from '../components/section/section';
import { FormValidateDirective } from 'form-validate-angular';

@NgModule({
  declarations: [    
  ],
  imports: [
    CommonModule,
    BlankComponent, 
    Section,
    FormsModule,
    TrCurrencyPipe,
    FlexiGridModule,
    FlexiSelectModule,
    FormValidateDirective
  ],
  exports: [
    CommonModule,
    BlankComponent, 
    Section,
    FormsModule,
    TrCurrencyPipe,
    FlexiGridModule,
    FlexiSelectModule,
    FormValidateDirective
  ]
})
export class SharedModule { }
