import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { UserService } from '../services/user.service';

export const blockedGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const userService = inject(UserService);

  return userService.getLoggedUser().pipe(
    map(user => {
      if(user?.isBlocked) {
        router.navigate(['/blocked']);
        return false;
      }

      return true;
    }),
    catchError(error => {
      console.error("Error checking blocked status:", error);
      return of(true);
    })
  );
};
