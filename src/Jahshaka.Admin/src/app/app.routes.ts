
import { CanActivateChild, Routes } from '@angular/router';

import { StarterViewComponent } from './views/appviews/starterview.component';
import { LoginComponent } from './components/auth/login.component';

import { BlankLayoutComponent } from './components/common/layouts/blankLayout.component';
import { BasicLayoutComponent } from './components/common/layouts/basicLayout.component';
import { TopNavigationLayoutComponent } from './components/common/layouts/topNavigationLayout.component';

import { AuthGuard } from './shared/services/auth-guard';
import { AssetListComponent } from 'app/components/assets/asset-list.component';
import { DashboardComponent } from 'app/components/dashboard/dashboard.component';
import { UserListComponent } from 'app/components/users/user-list.component';
import { AssetAddComponent } from 'app/components/assets/asset-add';

export const ROUTES: Routes = [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

    {
        path: '',
        canActivateChild: [AuthGuard],
        children: [
            {
                path: 'dashboard', component: BasicLayoutComponent,
                children: [
                    { path: '', component: DashboardComponent }
                ]
            },
            {
                path: 'assets', component: BasicLayoutComponent,
                children: [
                    { path: '', component: AssetListComponent },
                    { path: 'new', component: AssetAddComponent }
                ]
            },
            {
                path: 'users', component: BasicLayoutComponent,
                children: [
                    { path: '', component: UserListComponent }
                ]
            }
        ]
    },

    {
        path: '', component: BlankLayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
        ]
    },

    // Handle all other routes
    { path: '**', redirectTo: 'dashboard' }
];
