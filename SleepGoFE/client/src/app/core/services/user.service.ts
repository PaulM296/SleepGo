import { Inject, inject, Injectable, input } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AuthResponse } from '../../shared/models/registrationModels/authResponse';
import { UserRegistrationModel } from '../../shared/models/userModels/userRegistrationMode';
import { HotelRegistrationModel } from '../../shared/models/registrationModels/hotelRegistrationModel';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModels/paginationResponse';
import { ResponseUserModel } from '../../shared/models/userModels/responseUserModel';
import { ResponseHotelModel } from '../../shared/models/userModels/responseHotelModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${ environment.apiUrl }users`;
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

  getPaginatedUsers(pageIndex: number, pageSize: number): Observable<PaginationResponse<ResponseUserModel>> {
    const params = new HttpParams().set('pageIndex', pageIndex).set('pageSize', pageSize);
    return this.http.get<PaginationResponse<ResponseUserModel>>(`${this.apiUrl}/adminPage/users`, { params } )
  }

  getPaginatedHotels(pageIndex: number, pageSize: number): Observable<PaginationResponse<ResponseHotelModel>> {
    const params = new HttpParams().set('pageIndex', pageIndex).set('pageSize', pageSize);
    return this.http.get<PaginationResponse<ResponseHotelModel>>(`${this.apiUrl}/adminPage/hotels`, { params } );
  }

  blockUser(userId: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${userId}/block`, {});
  }

  unblockUser(userId: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${userId}/unblock`, {});
  }
  
  updateUser(userId: string, userData: FormData): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${userId}`, userData);
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${userId}`);
  }

  getLoggedUser(): Observable<ResponseUserModel> {
    return this.http.get<ResponseUserModel>(`${this.apiUrl}/logged-user`);
  }

  getUserById(userId: string): Observable<ResponseUserModel | ResponseHotelModel> {
    return this.http.get<ResponseUserModel | ResponseHotelModel>(`${this.apiUrl}/${userId}`)
  }

  getImageById(imageId: string): Observable<{ imageSrc: string }> {
    return this.http.get<{ imageSrc: string }>(`${environment.apiUrl}Image/${imageId}`);
  }
  
  deleteImage(imageId: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}Image/${imageId}`);
  }
  
  searchHotels(query: string, paginationRequest: PaginationRequest): Observable<PaginationResponse<ResponseHotelModel>> {
    const params = new HttpParams()
      .set('query', query)
      .set('pageIndex', paginationRequest.pageIndex.toString())
      .set('pageSize', paginationRequest.pageSize.toString());

    return this.http.get<PaginationResponse<ResponseHotelModel>>(`${this.apiUrl}/search`, { params });
  }

  getAllPaginatedHotels(paginationRequest: PaginationRequest): Observable<PaginationResponse<ResponseHotelModel>> {
    const params = new HttpParams()
      .set('pageIndex', paginationRequest.pageIndex.toString())
      .set('pageSize', paginationRequest.pageSize.toString());

    return this.http.get<PaginationResponse<ResponseHotelModel>>(`${environment.apiUrl}hotels`, { params })
      .pipe(
        map(response => ({
          ...response,
          items: response.items.filter((hotel: ResponseHotelModel) => !hotel.isBlocked)
        }))
      );
  }
}

