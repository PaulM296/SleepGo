import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HotelProfileService {
  private apiUrl = `${environment.apiUrl}hotels`;
  private http = inject(HttpClient);

  askQuestionAboutHotels(question: string): Observable<string> {
    const headers = { 'Content-Type': 'application/json' };

    return this.http.post<string>(
      `${this.apiUrl}/hotel-recommendations/ask`,
      JSON.stringify(question),
      { headers, responseType: 'text' as 'json'}
    );
  }
}
