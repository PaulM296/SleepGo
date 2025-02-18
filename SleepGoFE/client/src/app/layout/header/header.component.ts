import { ChangeDetectorRef, Component, computed, inject, OnInit } from '@angular/core';
import { JwtService } from '../../core/services/jwt.service';
import { UserService } from '../../core/services/user.service';
import { NavigationEnd, Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { filter } from 'rxjs';
import { SearchBarComponent } from "../../shared/components/search-bar/search-bar.component";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatExpansionModule,
    MatIconModule,
    CommonModule,
    MatButtonModule,
    MatMenuModule,
    MatFormFieldModule,
    FormsModule,
    MatInputModule,
    SearchBarComponent
],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  isLoggedIn = false;
  userImageUrl: string | null = null;
  userName: string = '';
  searchQuery: string = '';
  showSearchBar: boolean = false;

  private jwtService = inject(JwtService);
  private userService = inject(UserService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  isHotelsPage = computed(() => this.router.url.startsWith('/hotels'));

  ngOnInit(): void {
    this.checkUserStatus();
    
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      this.showSearchBar = event.url.startsWith('/hotels');
      this.cdr.detectChanges();
    });
  }

  checkUserStatus() {
    this.isLoggedIn = !!this.jwtService.getToken();

    if (this.isLoggedIn) {
      this.userService.getLoggedUser().subscribe(user => {
        this.userName = user.userName;

        if (user.imageId) {
          this.userService.getImageById(user.imageId).subscribe({
            next: (response) => {
              this.userImageUrl = response.imageSrc;
            },
            error: (error) => {
              console.error('Error fetching user image:', error);
              this.userImageUrl = null;
            }
          });
        } else {
          this.userImageUrl = null;
        }
      });
    }
  }

performSearch(query: string) {
  if (query.trim() === '') {
      this.router.navigate(['/hotels'], { queryParams: {} });
  } else {
      this.router.navigate(['/hotels'], { queryParams: { query } });
  }
}

  logout() {
    this.jwtService.clearToken();
    this.isLoggedIn = false;
    this.userImageUrl = null;
    this.router.navigate(['']);
  }

  navigateToProfile() {
    this.router.navigate(['/user-profile']);
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  navigateToRegister() {
    this.router.navigate(['/register']);
  }

  navigateToHome() {
    this.router.navigate(['']);
  }
}
