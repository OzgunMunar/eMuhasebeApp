import { Component, Input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-blank',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './blank.html',
  styleUrl: './blank.css'
})

export default class BlankComponent {

  @Input() pageName: string = ""
  @Input() routes: string[] = []

}
