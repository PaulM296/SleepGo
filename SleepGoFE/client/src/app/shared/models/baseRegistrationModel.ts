export interface BaseRegistrationModel {
    userName: string;
    email: string;
    password: string;
    phoneNumber: string;
    role: 'admin' | 'user' | 'hotel';
}