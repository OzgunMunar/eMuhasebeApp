import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TrCurrencyPipe } from 'tr-currency';
import { FlexiGridModule } from 'flexi-grid';
import { FlexiSelectModule } from 'flexi-select';
import BlankComponent from '../components/blank/blank';
import { Section } from '../components/section/section';

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
    FlexiSelectModule
  ],
  exports: [
    CommonModule,
    BlankComponent, 
    Section,
    FormsModule,
    TrCurrencyPipe,
    FlexiGridModule,
    FlexiSelectModule
  ]
})
export class SharedModule { }
