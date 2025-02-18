import { ResponseAmenityModel } from "./amenityModels/responseAmenityModel";
import { ResponseReservationModel } from "./reservationModels/responseReservationModel";
import { ResponseReviewModel } from "./reviewModels/responseReviewModel";

export interface ResponseHotelModel {
    id: string;
    hotelProfileId: string;
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
    rating: number;
    reservations: ResponseReservationModel[];
    reviews: ResponseReviewModel[];
    amenities: ResponseAmenityModel[];
    imageId: string;
    imageUrl?: string;
    isBlocked: boolean;
}