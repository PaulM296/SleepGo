import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthResponse } from '../../shared/models/authResponse';
import { UserRegistrationModel } from '../../shared/models/userRegistrationMode';
import { HotelRegistrationModel } from '../../shared/models/hotelRegistrationModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${ environment .apiUrl }users`;
  private http = inject(HttpClient);

  login(email: string, password: string): Observable <AuthResponse> {
    return this.http.post<AuthResponse>(`${ this.apiUrl }/login`, { email, password });
  }

  register(user: UserRegistrationModel | HotelRegistrationModel): Observable<AuthResponse> {
    const formData = new FormData();

    formData.append('userName', user.userName);
    formData.append('email', user.email);
    formData.append('password', user.password);
    formData.append('phoneNumber', user.phoneNumber);
    formData.append('role', user.role);

    if (user.role === 'user') {
      const userModel = user as UserRegistrationModel;
      formData.append('firstName', userModel.firstName);
      formData.append('lastName', userModel.lastName);
      formData.append('dateOfBirth', userModel.dateOfBirth.toISOString());

      if (userModel.image) {
        formData.append('image', userModel.image[0]);
      }
    }

    if (user.role === 'hotel') {
      const hotelModel = user as HotelRegistrationModel;
      formData.append('hotelName', hotelModel.hotelName);
      formData.append('address', hotelModel.address);
      formData.append('city', hotelModel.city);
      formData.append('country', hotelModel.country);
      formData.append('zipCode', hotelModel.zipCode);
      formData.append('hotelDescription', hotelModel.hotelDescription);
      formData.append('latitude', hotelModel.latitude ? hotelModel.latitude.toString() : '0');
      formData.append('longitude', hotelModel.longitude ? hotelModel.longitude.toString() : '0');

      if (hotelModel.image) {
        formData.append('image', hotelModel.image[0]);
      }
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, formData);
  }

}

