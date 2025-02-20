import { RoomType } from "./roomType";


export interface CreateRoomDto {
    hotelId: string;
    roomType: RoomType;
    roomNumber: number;
    price: number;
    balcony: boolean;
    airConditioning: boolean;
    kitchenette: boolean;
    hairdryer: boolean;
    tv: boolean;
    isReserved: boolean;
}