import { BaseRegistrationModel } from "./baseRegistrationModel";

export interface HotelRegistrationModel extends BaseRegistrationModel {
    hotelName: string;
    address: string;
    city: string;
    Country: string;
    ZipCode: string;
    HotelDescription: string;
    Latitude: number;
    Longitude: number;
    image?: FileList | null;
}