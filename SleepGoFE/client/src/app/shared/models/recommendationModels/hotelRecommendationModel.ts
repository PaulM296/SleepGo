export interface HotelRecommendationData {
    userId: string;
    hotelId: string;
    hotelRating: number;
    pricePaid: number;
    label: boolean;
    city: string;
    country: string;
    roomType: number;
}

export interface HotelRecommendationResult {
    hotelName: string;
    probability: number;
    city: string;
    country: string;
    rating: number;
}

