export interface ResponseUserModel {
    id: string;
    userName: string;
    email: string;
    phoneNumber: string;
    role: number;
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    imageId: string;
    isBlocked: boolean;
}