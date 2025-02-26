import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CreateAmenityModel } from '../../shared/models/amenityModels/createAmenityModel';
import { Observable } from 'rxjs';
import { ResponseAmenityModel } from '../../shared/models/amenityModels/responseAmenityModel';
import { UpdateAmenityModel } from '../../shared/models/amenityModels/updateAmenityModel';

@Injectable({
  providedIn: 'root'
})
export class AmenityService {
  private apiUrl = `${environment.apiUrl}amenities`;
  private http = inject(HttpClient);

  addAmenities(amenities: FormData): Observable<ResponseAmenityModel> {
    return this.http.post<ResponseAmenityModel>(`${this.apiUrl}`, amenities, {
      headers: {
        'enctype': 'multipart/form-data'
      }
    });
  }

  updateAmenities(amenityId: string, updateAmenitiesModel: UpdateAmenityModel): Observable<ResponseAmenityModel> {
    return this.http.put<ResponseAmenityModel>(`${this.apiUrl}/${amenityId}`, updateAmenitiesModel);
  }

  deleteAmenities(amenityId: string): Observable<ResponseAmenityModel> {
    return this.http.delete<ResponseAmenityModel>(`${this.apiUrl}/${amenityId}`);
  }

  getAmenitiesByHotelId(hotelId: string): Observable<ResponseAmenityModel> {
    return this.http.get<ResponseAmenityModel>(`${this.apiUrl}/hotel/${hotelId}`);
  }
}
