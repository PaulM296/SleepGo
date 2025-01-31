import { BaseRegistrationModel } from "./baseRegistrationModel";

export interface UserRegistrationModel extends BaseRegistrationModel {
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    image?: FileList | null;
}