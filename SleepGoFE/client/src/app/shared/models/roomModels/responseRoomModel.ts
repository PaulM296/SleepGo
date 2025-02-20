import { ResponseReservationModel } from "../reservationModels/responseReservationModel";
import { RoomType } from "./roomType";

export interface ResponseRoomModel {
    id: string;
    hotelId: string;
    roomType: RoomType;
    price: number;
    roomNumber: number;
    balcony: boolean;
    airConditioning: boolean;
    kitchenette: boolean;
    hairdryer: boolean;
    tv: boolean;
    isReserved: boolean;
    reservations: ResponseReservationModel[];
}