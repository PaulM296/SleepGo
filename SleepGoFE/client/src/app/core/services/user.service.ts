import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthResponse } from '../../shared/models/authResponse';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${ environment .apiUrl }users`;
  private http = inject(HttpClient);

  login(email: string, password: string): Observable <AuthResponse> {
    return this.http.post<AuthResponse>(`${ this.apiUrl }/login`, { email, password });
  }

}
