import { ResponseReservationModel } from "./responseReservationModel";
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
    Hairdryer: boolean;
    TV: boolean;
    IsReserved: boolean;
    reservations: ResponseReservationModel[];
}