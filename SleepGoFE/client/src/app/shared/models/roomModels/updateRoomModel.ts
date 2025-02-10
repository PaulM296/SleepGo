import { RoomType } from "../roomType";

export interface UpdateRoomModel {
    roomType: RoomType;
    price: number;
    roomNumber: number;
    balcony: boolean;
    airConditioning: boolean;
    kitchenette: boolean;
    hairdryer: boolean;
    tv: boolean;
    isReserved: boolean;
}