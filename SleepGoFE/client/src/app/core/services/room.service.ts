import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ResponseRoomModel } from '../../shared/models/roomModels/responseRoomModel';
import { Observable } from 'rxjs';
import { CreateRoomDto } from '../../shared/models/roomModels/createRoomModel';
import { UpdateRoomModel } from '../../shared/models/roomModels/updateRoomModel';
import { RoomType } from '../../shared/models/roomType';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationResponse';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  private apiUrl = `${ environment.apiUrl }rooms`;
  private http = inject(HttpClient);

  addRoomToHotel(room: CreateRoomDto): Observable<ResponseRoomModel> {
    return this.http.post<ResponseRoomModel>(`${this.apiUrl}`, room);
  }

  updateRoom(roomId: string, room: UpdateRoomModel): Observable<ResponseRoomModel> {
    return this.http.put<ResponseRoomModel>(`${this.apiUrl}/${roomId}`, room);
  }

  removeRoom(roomId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${roomId}`);
  }

  getRoomsFromHotelByRoomType(hotelId: string, roomType: RoomType, paginationRequest: PaginationRequest): 
  Observable<PaginationResponse<ResponseRoomModel>> {
    const params = new HttpParams()
      .set('roomType', roomType.toString())
      .set('pageIndex', paginationRequest.pageIndex.toString())
      .set('pageSize', paginationRequest.pageSize.toString());
    return this.http.get<PaginationResponse<ResponseRoomModel>>(`${this.apiUrl}/hotel/${hotelId}/rooms`, { params });
  }

  getAvailableRoomsFromHotelByRoomType(hotelId: string, roomType: RoomType, paginationRequest: PaginationRequest): 
  Observable<PaginationResponse<ResponseRoomModel>> {
    return this.http.get<PaginationResponse<ResponseRoomModel>>(`${this.apiUrl}/hotel/${hotelId}/available`, {
      params: {
        roomType: roomType.toString(),
        pageIndex: paginationRequest.pageIndex.toString(),
        pageSize: paginationRequest.pageSize.toString(),
      }
    });
  }
}
