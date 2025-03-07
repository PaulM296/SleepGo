import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-blocked',
  standalone: true,
  imports: [],
  templateUrl: './blocked.component.html',
  styleUrl: './blocked.component.scss'
})
export class BlockedComponent {
  private router = inject(Router);

  returnToLogin() {
    this.router.navigate(['/login']);
  }
}
