import { RoomType } from "../roomModels/roomType";
import { ReservationStatus } from "./reservationStatus";

export interface CreateReservationModel {
    hotelId: string;
    roomType: RoomType;
    checkIn: Date;
    checkOut: Date;
    price: number;
    status: ReservationStatus;
}