import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { JwtService } from '../services/jwt.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const jwtService = inject(JwtService);
  const router = inject(Router);

  if(jwtService.isLoggedIn() && jwtService.getUserRole() === 'Admin') {
    return true;
  } else {
    router.navigate(['/forbidden']);
    return false;
  }
};
