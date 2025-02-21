export interface ResponseReservationModel {
    id: string;
    userId: string;
    roomId: string;
    hotelName: string;
    checkIn: Date;
    checkOut: Date;
    price: number;
    status: string;
}