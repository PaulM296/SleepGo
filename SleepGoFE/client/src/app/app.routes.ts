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
import { CreateAmenitiesComponent } from './features/amenity/create-amenities/create-amenities.component';
import { HotelAmenitiesComponent } from './features/amenity/hotel-amenities/hotel-amenities.component';
import { BlockedComponent } from './shared/components/blocked/blocked.component';
import { blockedGuard } from './core/guards/blocked.guard';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'forbidden', component: ForbiddenComponent},
    {path: 'blocked', component: BlockedComponent},
    {path: 'hotels', component: HotelComponent, canActivate: [authGuard, blockedGuard]},
    {path: 'hotels/:id', component: HotelInformationComponent, canActivate: [authGuard, blockedGuard]},
    {path: 'adminPage/users', component: UserDetailsComponent, canActivate: [authGuard, adminGuard, blockedGuard]},
    {path: 'adminPage/hotels', component: HotelDetailsComponent, canActivate: [authGuard, adminGuard, blockedGuard]},
    {path: 'user-profile', component: UserProfileComponent, canActivate: [authGuard, blockedGuard]},
    {path: 'create-room', component: CreateRoomComponent, canActivate: [authGuard, hotelGuard, blockedGuard]},
    {path: 'write-reviews', component: WriteReviewsComponent, canActivate: [authGuard, blockedGuard]},
    {path: 'make-reservation', component: MakeReservationComponent, canActivate: [authGuard, userGuard, blockedGuard]},
    {path: 'user-reviews', component: UserReviewsComponent, canActivate: [authGuard, blockedGuard]},
    {path: 'user-reservations', component: UserReservationComponent, canActivate: [authGuard, blockedGuard]},
    {path: 'hotel-reviews', component: HotelReviewsComponent, canActivate: [authGuard, hotelGuard, blockedGuard]},
    {path: 'hotel-reservations', component: HotelReservationsComponent, canActivate: [authGuard, hotelGuard, blockedGuard]},
    {path: 'hotel-rooms-by-type', component: ViewHotelRoomsComponent, canActivate:[authGuard, hotelGuard, blockedGuard]},
    {path: 'available-hotel-rooms-by-type', component: ViewAvailableHotelRoomsComponent, canActivate:[authGuard, hotelGuard, blockedGuard]},
    {path: 'create-amenities', component: CreateAmenitiesComponent, canActivate:[authGuard, hotelGuard, blockedGuard]},
    {path: 'hotel-amenities', component: HotelAmenitiesComponent, canActivate: [authGuard, hotelGuard, blockedGuard]},
    {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];
