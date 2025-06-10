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
import { ResponseAmenityModel } from '../../shared/models/amenityModels/responseAmenityModel';
import { ResponseRoomModel } from '../../shared/models/roomModels/responseRoomModel';
import { RoomType } from '../../shared/models/roomModels/roomType';
import { animation } from '@angular/animations';
import { GoogleMap, MapMarker } from '@angular/google-maps';
import { environment } from '../../../environments/environment';
import { FormsModule } from '@angular/forms';
import { JwtService } from '../../core/services/jwt.service';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-hotel-information',
  standalone: true,
  imports: [
    HeaderComponent,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    CommonModule,
    GoogleMap,
    MapMarker,
    FormsModule,
    MatMenuModule
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
  private jwtService = inject(JwtService);

  hotel!: ResponseHotelModel;
  hotelId!: string;
  userId?: string;
  pageIndex: number = 0;
  pageSize: number = 3;
  reviews: ResponseReviewModel[] = [];
  hasMoreReviews: boolean = true;
  amenities!: ResponseAmenityModel;
  rooms: ResponseRoomModel[] = [];
  roomTypes: RoomType[] = [];
  center!: google.maps.LatLngLiteral;
  zoom = 13;
  markerPosition!: google.maps.LatLngLiteral;
  isGoogleMapsLoaded = false;
  question: string = '';
  answer: string = '';
  isAdmin = false;

  ngOnInit(): void {
    this.isAdmin = this.jwtService.getUserRole() === 'Admin';
    this.loadGoogleMaps();
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

          console.log("Hotel details loaded:", this.hotel);
          this.setMapLocation();
  
          this.loadHotelImage();
          this.fetchHotelReviews();
          this.fetchHotelAmenities();
          this.fetchHotelRooms();
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

  fetchHotelAmenities(): void {
    if(!this.hotelId) {
      console.error('Error fetching Hotel ID');
      return;
    }

    this.amenityService.getAmenitiesByHotelId(this.hotelId).subscribe({
      next: (response: ResponseAmenityModel) => {
        console.log('Amenities received before assigning: ', response);
        if(Array.isArray(response) && response.length > 0) {
          this.amenities = response[0];
        } else {
          this.amenities = response;
        }
        console.log('Amenities retrieved:', this.amenities);
      },
      error: (error) => {
        console.error('Error fecthing amenities: ', error);
      }
    });
  }

  fetchHotelRooms(): void {
    if (!this.hotelId) {
      console.error('Error fetching Hotel ID');
      return;
    }
  
    this.roomService.getAllRoomsFromHotelByHotelId(this.hotelId).subscribe({
      next: (response: ResponseRoomModel[]) => {
        console.log('Rooms retrieved:', response);
  
        if (!response || response.length === 0) {
          console.warn("No rooms found.");
          this.rooms = [];
          return;
        }

        const roomMap = new Map<number, ResponseRoomModel>();
  
        response.forEach((room) => {
          if (!roomMap.has(room.roomType)) {
            roomMap.set(room.roomType, room);
          }
        });

        this.rooms = Array.from(roomMap.values());
  
        console.log('Processed rooms:', this.rooms);
      },
      error: (error) => {
        console.error('Error fetching rooms: ', error);
      }
    });
  }
  

  extractUniqueRoomTypes(): void {
    this.roomTypes = Array.from(
      new Set(this.rooms.map(room => room.roomType))
    ).sort((a, b) => a - b);
  }

  getRoomTypeName(roomType: RoomType): string {
    return RoomType[roomType];
  }

  setMapLocation() {
    if (!this.hotel) {
      console.error("Hotel data is not available yet.");
      return;
    }
  
    if (this.hotel.latitude && this.hotel.longitude) {
      this.center = { 
        lat: this.hotel.latitude, 
        lng: this.hotel.longitude 
      };
      
      this.markerPosition = {
        lat: this.hotel.latitude,
        lng: this.hotel.longitude
      };
  
      console.log("Map location set:", this.center);
    } else {
      console.error("Latitude or longitude is missing for this hotel!");
    }
  }
  

  loadGoogleMaps(): void {
    if (typeof google === 'undefined' || !google.maps) {
      const script = document.createElement('script');
      script.src = `https://maps.googleapis.com/maps/api/js?key=${environment.googleMapsApiKey}`;
      script.async = true;
      script.defer = true;
      script.onload = () => {
        this.isGoogleMapsLoaded = true;
        this.setMapLocation();
      };
      document.head.appendChild(script);
    } else {
      this.isGoogleMapsLoaded = true;
      this.setMapLocation();
    }
  }

  askQuestion(): void {
    if(!this.question.trim()) {
      return;
    }

    this.reviewService.askQuestionAboutHotelReviews(this.hotelId, this.question).subscribe({
      next: (response) => {
        this.answer = response;
      },
      error: (error) => {
        console.error("Failed to fetch answer:", error);
      }
    });
  }

  moderateReview(review: ResponseReviewModel): void {
    this.reviewService.moderateReview(review.id).subscribe({
      next: () => {
        this.snackbarService.success("Review has been moderated.");
        review.isModerated = true;
      },
      error: (err) => {
        this.snackbarService.error("Failed to moderate review.");
        console.error(err);
      }
    });
  }

  unmoderateReview(review: ResponseReviewModel): void {
    this.reviewService.unmoderateReview(review.id).subscribe({
      next: () => {
        this.snackbarService.success("Review has been unmoderated.");
        review.isModerated = false;
      },
      error: (err) => {
        this.snackbarService.error("Failed to unmoderate review.");
        console.error(err);
      }
    });
  }
}
