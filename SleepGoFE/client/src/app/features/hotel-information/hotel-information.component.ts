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

  hotel!: ResponseHotelModel;
  hotelId!: string;
  userId?: string;

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
    this.router.navigate(['/make-reservation']);
  }
}
