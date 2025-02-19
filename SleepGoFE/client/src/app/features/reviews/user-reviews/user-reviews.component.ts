import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ReviewService } from '../../../core/services/review.service';
import { JwtService } from '../../../core/services/jwt.service';
import { ResponseReviewModel } from '../../../shared/models/reviewModels/responseReviewModel';
import { PaginationRequest, PaginationResponse } from '../../../shared/models/paginationResponse';
import { HeaderComponent } from "../../../layout/header/header.component";

@Component({
  selector: 'app-user-reviews',
  standalone: true,
  imports: [
    MatCardModule,
    CommonModule,
    MatListModule,
    MatIconModule,
    MatPaginatorModule,
    HeaderComponent
],
  templateUrl: './user-reviews.component.html',
  styleUrl: './user-reviews.component.scss'
})
export class UserReviewsComponent implements OnInit {
  private reviewService = inject(ReviewService);
  private jwtService = inject(JwtService);

  reviews = signal<ResponseReviewModel[]>([]);
  totalReviews = signal<number>(0);
  pageSize: number = 5;
  pageIndex: number = 0;

  ngOnInit(): void {
    this.loadUserReviews();
  }

  loadUserReviews(): void {
    const userId = this.jwtService.getUserIdFromToken();

    if (!userId) {
      console.error("Error: No user ID found!");
      return;
    }

    const paginationRequest: PaginationRequest = {
      pageIndex: this.pageIndex + 1,
      pageSize: this.pageSize
    };

    this.reviewService.getAllUserReviews(userId, paginationRequest).subscribe({
      next: (response: PaginationResponse<ResponseReviewModel>) => {
        console.log("Received reviews response:", response);
        this.reviews.set(response.items);

        if (this.pageIndex === 0) {
            this.fetchTotalReviews(userId);
        } else {
            this.totalReviews.set(Math.max(this.totalReviews(), response.items.length + this.pageIndex * this.pageSize));
        }
      },
      error: (error) => console.error("Error fetching user reviews:", error)
    });
  }

  fetchTotalReviews(userId: string): void {
    const paginationRequest: PaginationRequest = {
        pageIndex: 1,
        pageSize: 100
    };

    this.reviewService.getAllUserReviews(userId, paginationRequest).subscribe({
        next: (response: PaginationResponse<ResponseReviewModel>) => {
            this.totalReviews.set(response.items.length);
        },
        error: (error) => console.error("Error fetching total reviews:", error)
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadUserReviews();
  }

  trackById(index: number, item: ResponseReviewModel): string {
    return item.id;
  }
}
