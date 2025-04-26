import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CreateReviewModel } from '../../shared/models/reviewModels/createReviewModel';
import { Observable } from 'rxjs';
import { ResponseReviewModel } from '../../shared/models/reviewModels/responseReviewModel';
import { UpdateReviewModel } from '../../shared/models/reviewModels/updateReviewModel';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModels/paginationResponse';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = `${environment.apiUrl}reviews`;
  private http = inject(HttpClient);

  addReview(createReviewModel: CreateReviewModel): Observable<ResponseReviewModel> {
  const formData = new FormData();
  formData.append('hotelId', createReviewModel.hotelId);
  formData.append('reviewText', createReviewModel.reviewText);
  formData.append('rating', createReviewModel.rating.toString());

  return this.http.post<ResponseReviewModel>(`${this.apiUrl}`, formData);
  }

  updateReview(reviewId: string, updateReviewModel: UpdateReviewModel): Observable<ResponseReviewModel> {
    return this.http.put<ResponseReviewModel>(`${this.apiUrl}/${reviewId}`, updateReviewModel);
  }

  moderateReview(reviewId: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${reviewId}/moderate`, {});
  }

  unmoderateReview(reviewId: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${reviewId}/unmoderate`, {});
  }

  deleteReview(reviewId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${reviewId}`);
  }

  getAllHotelReviews(hotelId: string, paginationRequest: PaginationRequest):
   Observable<PaginationResponse<ResponseReviewModel>> {
    const params = new HttpParams()
      .set('pageIndex', paginationRequest.pageIndex.toString())
      .set('pageSize', paginationRequest.pageSize.toString());

    return this.http.get<PaginationResponse<ResponseReviewModel>>(`${this.apiUrl}/hotel/${hotelId}/reviews`, { params });
  }

  getAllUserReviews(userId: string, paginationRequest: PaginationRequest):
   Observable<PaginationResponse<ResponseReviewModel>> {
    const params =  new HttpParams()
      .set('pageIndex', paginationRequest.pageIndex.toString())
      .set('pageSize', paginationRequest.pageSize.toString());

    return this.http.get<PaginationResponse<ResponseReviewModel>>(`${this.apiUrl}/user/${userId}/reviews`, { params });
   }

   askQuestionAboutHotelReviews(hotelId: string, question: string): Observable<string> {
    const headers = { 'Content-Type': 'application/json' };

    return this.http.post<string>(
      `${this.apiUrl}/hotel/${hotelId}/ask`,
       JSON.stringify(question),
       { headers, responseType: 'text' as 'json'});
   }
}
