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
import { UserReviewsComponent } from './features/reviews/user-reviews/user-reviews.component';
import { UserReservationComponent } from './features/reservation/user-reservation/user-reservation.component';
import { HotelReviewsComponent } from './features/reviews/hotel-reviews/hotel-reviews.component';
import { HotelReservationsComponent } from './features/reservation/hotel-reservations/hotel-reservations.component';
import { WriteReviewsComponent } from './features/reviews/write-reviews/write-reviews.component';
import { MakeReservationComponent } from './features/reservation/make-reservation/make-reservation.component';
import { hotelGuard } from './core/guards/hotel.guard';
import { userGuard } from './core/guards/user.guard';
import { CreateRoomComponent } from './features/room/create-room/create-room.component';
import { ViewHotelRoomsComponent } from './features/room/view-hotel-rooms/view-hotel-rooms.component';
import { ViewAvailableHotelRoomsComponent } from './features/room/view-available-hotel-rooms/view-available-hotel-rooms.component';

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
    {path: 'create-room', component: CreateRoomComponent, canActivate: [authGuard, hotelGuard]},
    {path: 'write-reviews', component: WriteReviewsComponent, canActivate: [authGuard]},
    {path: 'make-reservation', component: MakeReservationComponent, canActivate: [authGuard, userGuard]},
    {path: 'user-reviews', component: UserReviewsComponent, canActivate: [authGuard]},
    {path: 'user-reservations', component: UserReservationComponent, canActivate: [authGuard]},
    {path: 'hotel-reviews', component: HotelReviewsComponent},
    {path: 'hotel-reservations', component: HotelReservationsComponent, canActivate: [authGuard, hotelGuard]},
    {path: 'hotel-rooms-by-type', component: ViewHotelRoomsComponent, canActivate:[authGuard, hotelGuard]},
    {path: 'available-hotel-rooms-by-type', component: ViewAvailableHotelRoomsComponent, canActivate:[authGuard, hotelGuard]},
    {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];
