export interface ResponseReservationModel {
    id: string;
    userId: string;
    roomId: string;
    hotelName: string;
    checkIn: Date;
    checkout: Date;
    price: number;
    status: string;
}