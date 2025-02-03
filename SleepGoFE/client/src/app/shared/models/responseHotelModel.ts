import { ResponseAmenityModel } from "./responseAmenityModel";
import { ResponseReservationModel } from "./responseReservationModel";
import { ResponseReviewModel } from "./responseReviewModel";

export interface ResponseHotel {
    id: string;
    userName: string;
    email: string;
    phoneNumber: string;
    role: number;
    hotelName: string;
    address: string;
    city: string;
    country: string;
    zipCode: string;
    hotelDescription: string;
    latitude: number;
    longitude: number;
    reservations: ResponseReservationModel[];
    reviews: ResponseReviewModel[];
    amenities: ResponseAmenityModel[];
    imageId: string;
    isBlocked: boolean;
}