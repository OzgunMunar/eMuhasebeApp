import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { AuthService } from './services/auth.service';

export const routes: Routes = [

    {
        path: "login",
        loadComponent: () => import("../app/components/login/login")
    },
    {
        path: "",
        loadComponent: () => import("../app/components/layouts/layouts"),
        // canActivateChild: [()=> inject(AuthService).isAuthenticated()],
        children: [
            {
                path: "",
                loadComponent: () => import("../app/components/home/home")
            },
            {
                path: "examples",
                loadComponent: () => import("../app/components/examples/examples")
            }
        ]
    }

];
