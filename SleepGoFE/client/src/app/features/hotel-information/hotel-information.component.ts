import { Component, inject, OnInit } from '@angular/core';
import { HeaderComponent } from '../../layout/header/header.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { WriteReviewsComponent } from '../reviews/write-reviews/write-reviews.component';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { ResponseHotelModel } from '../../shared/models/userModels/responseHotelModel';
import { catchError, of, tap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { AmenityService } from '../../core/services/amenity.service';
import { RoomService } from '../../core/services/room.service';
import { ReviewService } from '../../core/services/review.service';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModels/paginationResponse';
import { SnackbarService } from '../../core/services/snackbar.service';
import { ResponseReviewModel } from '../../shared/models/reviewModels/responseReviewModel';

@Component({
  selector: 'app-hotel-information',
  standalone: true,
  imports: [
    HeaderComponent,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    CommonModule,
  ],
  templateUrl: './hotel-information.component.html',
  styleUrl: './hotel-information.component.scss'
})
export class HotelInformationComponent implements OnInit {
  private dialog = inject(MatDialog);
  private route = inject(ActivatedRoute);
  private userService = inject(UserService);
  private router = inject(Router);
  private amenityService = inject(AmenityService);
  private roomService = inject(RoomService);
  private reviewService = inject(ReviewService);
  private snackbarService = inject(SnackbarService);

  hotel!: ResponseHotelModel;
  hotelId!: string;
  userId?: string;
  pageIndex: number = 0;
  pageSize: number = 3;
  reviews: ResponseReviewModel[] = [];
  hasMoreReviews: boolean = true;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
      if (this.userId) {
        this.fetchHotelByUserId(this.userId);
      }
    });
  }

  fetchHotelByUserId(userId: string) {
    this.userService.getUserById(userId)
      .pipe(
        tap(),
        catchError(error => {
          console.error("Error fetching hotel details:", error);
          return of(null);
        })
      )
      .subscribe(response => {
        if (response && 'hotelId' in response) {
          this.hotel = response as ResponseHotelModel;
          this.hotelId = String(response.hotelId);
  
          this.loadHotelImage();
          this.fetchHotelReviews();
        } else {
          console.warn("The user is not a hotel profile.");
        }
      });
  }
  
  loadHotelImage() {
    if (!this.hotel || !this.hotel.imageId) {
      console.warn("No image ID found for this hotel.");
      return;
    }
  
    this.userService.getImageById(this.hotel.imageId).subscribe(imageResponse => {
      this.hotel.imageUrl = imageResponse.imageSrc;
    }, error => {
      console.error("Error fetching hotel image:", error);
    });
  }

  openReviewDialog() {
    if (!this.hotelId) {
      console.error("Hotel Profile ID is missing!");
      return;
    }

    const dialogRef = this.dialog.open(WriteReviewsComponent, {
      width: '900px',
      maxWidth: '95vw',
      data: { hotelId: this.hotelId } 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Review submitted successfully!');
      }
    });
  }

  navigateToCreateReservationPage() {
    if(!this.hotelId) {
      console.error('Hotel ID is missing!');
      return;
    }

    this.router.navigate(['/make-reservation'], { queryParams: { hotelId: this.hotelId }});
  }

  fetchHotelReviews(): void {
    console.log('HotelId: ', this.hotelId);
    if (!this.hotelId) {
      console.error('Error fetching hotel ID');
      return;
    }
  
    const paginationRequest: PaginationRequest = {
      pageIndex: this.pageIndex + 1,
      pageSize: this.pageSize,
    };
  
    this.reviewService.getAllHotelReviews(this.hotelId, paginationRequest).subscribe({
      next: (response: PaginationResponse<ResponseReviewModel>) => {
        console.log('Reviews successfully retrieved!');
  
        if (response.items.length > 0) {
          this.pageIndex++;
  
          const processedReviews: ResponseReviewModel[] = [];

          response.items.forEach((review) => {
            this.userService.getUserById(review.userId).subscribe((user) => {
              if (user.imageId) {
                this.userService.getImageById(user.imageId).subscribe((img) => {
                  review.userImageUrl = img.imageSrc;
                  processedReviews.push(review);
                  
                  if (processedReviews.length === response.items.length) {
                    this.reviews = [...this.reviews, ...processedReviews];
                  }
                });
              } else {
                review.userImageUrl = null;
                processedReviews.push(review);
  
                if (processedReviews.length === response.items.length) {
                  this.reviews = [...this.reviews, ...processedReviews];
                }
              }
            });
          });
  
          this.hasMoreReviews = response.items.length === this.pageSize;
        } else {
          this.hasMoreReviews = false;
        }
      },
      error: (error) => {
        console.error('Failed to retrieve reviews: ', error);
        if (error.status === 404) {
          this.hasMoreReviews = false;
        }
      },
    });
  }
  
}
