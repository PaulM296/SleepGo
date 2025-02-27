export interface ResponseReviewModel {
    id: string;
    userId: string;
    hotelId: string;
    hotelName: string;
    reviewText: string;
    createdAt: Date;
    userName: string;
    rating: number;
    isModerated: boolean;
    userImageUrl?: string | null;
}