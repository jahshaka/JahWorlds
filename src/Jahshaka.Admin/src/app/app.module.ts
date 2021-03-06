import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
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
import { ApplicationService } from './shared/services/application.service';
import { AssetListComponent } from 'app/components/assets/asset-list.component';
import { AssetAddComponent } from 'app/components/assets/asset-add';
import { UserListComponent } from 'app/components/users/user-list.component';
import { StarterViewComponent } from 'app/views/appviews/starterview.component';
import { DashboardComponent } from 'app/components/dashboard/dashboard.component';
import { ApplicationComponent } from 'app/components/settings/applications/application.component';
import { ApplicationsListComponent } from 'app/components/settings/applications/applications-list.component';
// import { TagInputModule } from 'ngx-chips';
import { NgUploaderModule } from 'ngx-uploader';
import { SweetAlert2Module } from '@toverux/ngsweetalert2';
import { Ng2Bs3ModalModule } from 'ng2-bs3-modal/ng2-bs3-modal';
import { ToastrModule } from 'ngx-toastr';
import { NgxMyDatePickerModule } from 'ngx-mydatepicker';


@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        AssetListComponent,
        AssetAddComponent,
        DashboardComponent,
        UserListComponent,
        ApplicationsListComponent,
        ApplicationComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        HttpModule,
        DashboardsModule,
        LayoutsModule,
        RouterModule.forRoot(ROUTES),
        NgxPaginationModule,
        // TagInputModule,
        NgUploaderModule,
        SweetAlert2Module,
        Ng2Bs3ModalModule,
        NgxMyDatePickerModule.forRoot(),
        ToastrModule.forRoot({
          positionClass: 'toast-top-right'
        })
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
        AssetService,
        ApplicationService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
