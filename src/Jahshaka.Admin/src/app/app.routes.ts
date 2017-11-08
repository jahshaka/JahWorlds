
import { CanActivateChild, Routes } from '@angular/router';

import { IndividualsComponent } from "./components/users/individuals.component";
import { IndividualComponent } from "./components/users/individual.component";

import { StarterViewComponent } from "./views/appviews/starterview.component";
import { LoginComponent } from "./components/auth/login.component";

import { BlankLayoutComponent } from "./components/common/layouts/blankLayout.component";
import { BasicLayoutComponent } from "./components/common/layouts/basicLayout.component";
import { TopNavigationLayoutComponent } from "./components/common/layouts/topNavigationLayout.component";

import { AuthGuard } from './shared/services/auth-guard';

export const ROUTES: Routes = [
    // Main redirect
    { path: '', redirectTo: 'starterview', pathMatch: 'full' },

    {
        path: '',
        canActivateChild: [AuthGuard],
        children: [
            // App views
            {
                path: 'users', component: BasicLayoutComponent,
                children: [
                  { path: 'individuals', component: IndividualsComponent },
                  { path: 'individuals/:id', component: IndividualComponent }
                ]
            },
            {
                path: '', component: BasicLayoutComponent,
                children: [
                    { path: 'starterview', component: StarterViewComponent }
                ]
            },
        ]
    },

    {
        path: '', component: BlankLayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
        ]
    },

    // Handle all other routes
    { path: '**', redirectTo: 'starterview' }
];
