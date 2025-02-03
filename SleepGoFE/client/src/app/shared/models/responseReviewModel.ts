export interface ResponseReviewModel {
    id: string;
    userId: string;
    hotelId: string;
    reviewText: string;
    createdAt: Date;
    userName: string;
    rating: number;
    isModerated: boolean;
}