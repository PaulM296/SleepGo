import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CreateReservationModel } from '../../shared/models/reservationModels/createReservationModel';
import { Observable } from 'rxjs';
import { ResponseReservationModel } from '../../shared/models/reservationModels/responseReservationModel';
import { UpdateReservationModel } from '../../shared/models/reservationModels/updateReservationModel';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModels/paginationResponse';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private apiUrl = `${environment.apiUrl}reservations`;
  private http = inject(HttpClient);

  addReservation(createReservationModel: CreateReservationModel): Observable<ResponseReservationModel> {
    return this.http.post<ResponseReservationModel>(`${this.apiUrl}`, createReservationModel);
  }

  updateReservation(reservationId: string, updateReservationModel: UpdateReservationModel): 
    Observable<ResponseReservationModel> {
      return this.http.put<ResponseReservationModel>(`${this.apiUrl}/${reservationId}`, updateReservationModel);
    }

  deleteReservation(reservationId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${reservationId}`);
  }

  getAllHotelReservations(hotelId: string, paginationRequest: PaginationRequest): 
    Observable<PaginationResponse<ResponseReservationModel>> {
      const params = new HttpParams()
        .set('pageIndex', paginationRequest.pageIndex.toString())
        .set('pageSize', paginationRequest.pageSize.toString());

      return this.http.get<PaginationResponse<ResponseReservationModel>>(`${this.apiUrl}/hotel/${hotelId}`, { params });
    }

  getAllUserReservations(userId: string, paginationRequest: PaginationRequest): 
    Observable<PaginationResponse<ResponseReservationModel>> {
      const params = new HttpParams()
        .set('pageIndex', paginationRequest.pageIndex.toString())
        .set('pageSize', paginationRequest.pageSize.toString());

      return this.http.get<PaginationResponse<ResponseReservationModel>>(`${this.apiUrl}/user/${userId}`, { params });
    }
}
