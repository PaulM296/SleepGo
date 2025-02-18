import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { UserDetailsComponent } from './features/adminPage/user-details/user-details.component';
import { HotelDetailsComponent } from './features/adminPage/hotel-details/hotel-details.component';
import { authGuard } from './core/guards/auth.guard';
import { UserProfileComponent } from './features/account/user-profile/user-profile.component';
import { HotelComponent } from './features/hotel/hotel.component';
import { HotelInformationComponent } from './features/hotel-information/hotel-information.component';
import { adminGuard } from './core/guards/admin.guard';
import { ForbiddenComponent } from './shared/components/forbidden/forbidden.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'forbidden', component: ForbiddenComponent},
    {path: 'hotels', component: HotelComponent, canActivate: [authGuard]},
    {path: 'hotels/:id', component: HotelInformationComponent, canActivate: [authGuard]},
    {path: 'adminPage/users', component: UserDetailsComponent, canActivate: [authGuard, adminGuard]},
    {path: 'adminPage/hotels', component: HotelDetailsComponent, canActivate: [authGuard, adminGuard]},
    {path: 'user-profile', component: UserProfileComponent, canActivate: [authGuard]},
    {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];
