import { BaseRegistrationModel } from "./baseRegistrationModel";

export interface HotelRegistrationModel extends BaseRegistrationModel {
    hotelName: string;
    address: string;
    city: string;
    country: string;
    zipCode: string;
    hotelDescription: string;
    latitude: number;
    longitude: number;
    image?: FileList | null;
}