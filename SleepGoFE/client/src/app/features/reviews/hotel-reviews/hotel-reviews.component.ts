import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { HeaderComponent } from '../../../layout/header/header.component';
import { JwtService } from '../../../core/services/jwt.service';
import { ReviewService } from '../../../core/services/review.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Router } from '@angular/router';
import { ResponseReviewModel } from '../../../shared/models/reviewModels/responseReviewModel';
import { PaginationRequest, PaginationResponse } from '../../../shared/models/paginationModels/paginationResponse';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-hotel-reviews',
  standalone: true,
  imports: [
    MatCardModule,
    MatButtonModule,
    MatPaginatorModule,
    CommonModule,
    MatIconModule,
    MatListModule,
    HeaderComponent
  ],
  templateUrl: './hotel-reviews.component.html',
  styleUrl: './hotel-reviews.component.scss'
})
export class HotelReviewsComponent implements OnInit {
  private jwtService = inject(JwtService);
  private reviewService = inject(ReviewService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);
  private userService = inject(UserService);

  reviews = signal<ResponseReviewModel[]>([]);
  totalReviews = signal<number>(0);
  pageSize: number = 5;
  pageIndex: number = 0;
  
  ngOnInit(): void {
    this.loadHotelReviews();
  }

  loadHotelReviews(): void {
    const userId = this.jwtService.getUserIdFromToken();
    if(!userId) {
      console.error("Error: No user ID found!");
      return;
    }
    console.log('userId: ', userId);

    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        if(!('hotelId' in user)) {
          console.error('Error: Logeed-in user does not have a hotel profile.');
          return;
        }

        const hotelId = user.hotelId as string;

        const paginationRequest: PaginationRequest = {
          pageIndex: this.pageIndex + 1,
          pageSize: this.pageSize
        };
    
        this.reviewService.getAllHotelReviews(hotelId, paginationRequest).subscribe({
          next: (response: PaginationResponse<ResponseReviewModel>) => {
            console.log("Received reviews response: ", response);
            this.reviews.set(response.items);
    
            if(this.pageIndex === 0) {
              this.fetchTotalReviews(hotelId);
            } else {
              this.totalReviews.set(Math.max(this.totalReviews(), response.items.length + this.pageIndex * this.pageSize));
            }
          },
          error: (error) => console.log("Error fetching user reviews: ", error)
        });
      }
    });
  }

  fetchTotalReviews(hotelId: string): void {
    const paginationRequest: PaginationRequest = {
      pageIndex: 1,
      pageSize: 100
    };

    this.reviewService.getAllHotelReviews(hotelId, paginationRequest).subscribe({
      next: (response: PaginationResponse<ResponseReviewModel>) => {
        this.totalReviews.set(response.items.length);
      },
      error: (error) => console.error('Error fetching total reviews: ', error)
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadHotelReviews();
  }

  trackById(index: number, item: ResponseReviewModel): string {
    return item.id;
  }
}
