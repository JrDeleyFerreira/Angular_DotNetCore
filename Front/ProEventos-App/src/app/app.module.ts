import { NgModule } from '@angular/core';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { NgxCurrencyDirective } from 'ngx-currency';
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';

import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AppRoutingModule } from './app-routing.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { EventosComponent } from './components/eventos/eventos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NavComponent } from './shared/nav/nav.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { ContatosComponent } from './components/contatos/contatos.component';
import { PerfilComponent } from './components/users/perfil/perfil.component';
import { EventoDetalheComponent } from './components/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './components/eventos/evento-lista/evento-lista.component';
import { UsersComponent } from './components/users/users.component';
import { LoginComponent } from './components/users/login/login.component';
import { RegistrationComponent } from './components/users/registration/registration.component';
import { TituloComponent } from './shared/title/title.component';
import { HomeComponent } from './components/home/home.component';

import { EventoService } from './services/evento.service';
import { LoteService } from './services/lote.service';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { AccountService } from './services/account.service';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { PerfilDetalheComponent } from './components/users/perfil/perfil-detalhe/perfil-detalhe.component';
import { PalestranteDetalheComponent } from './components/palestrantes/palestrante-detalhe/palestrante-detalhe.component';
import { PalestranteListaComponent } from './components/palestrantes/palestrante-lista/palestrante-lista.component';
import { RedesSociaisComponent } from './components/redesSociais/redesSociais.component';
import { PalestranteService } from './services/palestrante.service';
import { RedeSocialService } from './services/redeSocial.service';

defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AppComponent,
    ContatosComponent,
    DashboardComponent,
    DateTimeFormatPipe,
    EventoDetalheComponent,
    EventoListaComponent,
    EventosComponent,
    HomeComponent,
    LoginComponent,
    NavComponent,
    PalestrantesComponent,
    PalestranteDetalheComponent,
    PalestranteListaComponent,
    PerfilComponent,
    PerfilDetalheComponent,
    RedesSociaisComponent,
    RegistrationComponent,
    TituloComponent,
    UsersComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    BsDatepickerModule.forRoot(),
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    FontAwesomeModule,
    FormsModule,
    HttpClientModule,
    ModalModule.forRoot(),
    NgxCurrencyDirective,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule,
    TabsModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    TooltipModule.forRoot(),
  ],
  providers: [
    AccountService,
    EventoService,
    LoteService,
    PalestranteService,
    RedeSocialService,
    {
      provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true
    }
  ],
  bootstrap: [AppComponent],
})

export class AppModule { }
