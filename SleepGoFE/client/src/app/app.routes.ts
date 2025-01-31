import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'users/login', component: LoginComponent},
    {path: 'users/register', component: RegisterComponent},
    {path: 'not-found', component:NotFoundComponent},
    {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];
