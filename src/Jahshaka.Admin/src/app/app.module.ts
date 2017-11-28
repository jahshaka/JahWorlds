import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

import { ROUTES } from './app.routes';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/auth/login.component';

import { HttpClient } from './shared/utils/http-client';
import { AuthGuard } from './shared/services/auth-guard';
import { AuthService } from './shared/services/auth.service';
import { HttpService } from './shared/services/http.service';
import { HttpErrorHandler } from './shared/services/http-error-handler';
import { SessionService } from './shared/services/session.service';
import { ConnectivityUtil } from './shared/utils/connectivity-util';
import { NgxPaginationModule } from 'ngx-pagination';

// App views
import { DashboardsModule } from './views/dashboards/dashboards.module';

// App modules/components
import { LayoutsModule } from './components/common/layouts/layouts.module';
import { UserService } from './shared/services/user.service';
import { CollectionService } from './shared/services/collection.service';
import { AssetService } from './shared/services/asset.service';
import { AssetListComponent } from 'app/components/assets/asset-list.component';
import { UserListComponent } from 'app/components/users/user-list.component';
import { StarterViewComponent } from 'app/views/appviews/starterview.component';
import { DashboardComponent } from 'app/components/dashboard/dashboard.component';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        AssetListComponent,
        DashboardComponent,
        UserListComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        DashboardsModule,
        LayoutsModule,
        RouterModule.forRoot(ROUTES),
        NgxPaginationModule,
    ],
    providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy },
        HttpClient,
        HttpService,
        HttpErrorHandler,
        AuthGuard,
        SessionService,
        ConnectivityUtil,
        AuthService,
        UserService,
        CollectionService,
        AssetService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
