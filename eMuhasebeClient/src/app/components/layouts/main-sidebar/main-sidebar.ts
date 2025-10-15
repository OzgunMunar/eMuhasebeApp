import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MenuPipe } from '../../../pipes/menu-pipe';
import { Menus } from '../../../menu';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-main-sidebar',
  imports: [
    RouterLink,
    RouterLinkActive,
    FormsModule,
    MenuPipe
  ],
  templateUrl: './main-sidebar.html',
  styleUrl: './main-sidebar.css'
})
export class MainSidebar {

  search: string = ""
  menus = Menus

    constructor(
    public auth: AuthService
  ){
    
    if(!this.auth.user().isAdmin)
    {
      this.menus = this.menus.filter(p => !p.isAdminOnly)
    }

  }

}
