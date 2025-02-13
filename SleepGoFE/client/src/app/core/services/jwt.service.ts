import { Injectable } from '@angular/core';
import { jwtDecode, JwtPayload } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  decodeToken(token: string): any {
    try {
      return jwtDecode<JwtPayload>(token);
    } catch (error) {
      console.log('Invalid JWT Token', error);
      return
    }
  }

  getUserIdFromToken(): string | null {
    const token =  localStorage.getItem('token');
    if(!token) return null;

    const decodeToken = this.decodeToken(token);
    return decodeToken?.sub || decodeToken?.userId || null
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  clearToken(): void {
    localStorage.removeItem('token');
  }
}
