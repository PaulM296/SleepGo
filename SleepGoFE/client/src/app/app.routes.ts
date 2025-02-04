import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { UserDetailsComponent } from './features/adminPage/user-details/user-details.component';
import { HotelDetailsComponent } from './features/adminPage/hotel-details/hotel-details.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'adminPage/users', component: UserDetailsComponent, canActivate: [authGuard]},
    {path: 'adminPage/hotels', component: HotelDetailsComponent, canActivate: [authGuard]},
    {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];
