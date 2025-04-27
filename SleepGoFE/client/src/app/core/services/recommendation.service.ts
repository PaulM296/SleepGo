import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { HotelRecommendationData, HotelRecommendationResult } from '../../shared/models/recommendationModels/hotelRecommendationModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
  private apiUrl = `${environment.apiUrl}Recommendation`;
  private http = inject(HttpClient);

  predict(input: HotelRecommendationData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/predict`, input);
  }

  recommend(userId: string): Observable<HotelRecommendationResult[]> {
    return this.http.get<HotelRecommendationResult[]>(`${this.apiUrl}/recommend/${userId}`);
  }
}
