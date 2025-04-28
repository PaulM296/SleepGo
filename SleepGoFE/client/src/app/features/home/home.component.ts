import { Component, inject, OnInit } from '@angular/core';
import { FooterComponent } from "../../layout/footer/footer.component";
import { HeaderComponent } from "../../layout/header/header.component";
import { MatCardModule } from '@angular/material/card';
import { RecommendationService } from '../../core/services/recommendation.service';
import { HotelRecommendationResult } from '../../shared/models/recommendationModels/hotelRecommendationModel';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { HotelProfileService } from '../../core/services/hotel-profile.service';
import { ReservationService } from '../../core/services/reservation.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    FooterComponent,
    HeaderComponent,
    MatCardModule,
    CommonModule,
    MatButtonModule,
    FormsModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  private reservationService = inject(ReservationService);
  private hotelService = inject(HotelProfileService);

  recommendationsText = '';
  currentIndex = 0;
  isLoading = true;
  errorMessage = '';
  hotelQuestion: string = '';
hotelAnswer: string = '';

  ngOnInit() {
    this.fetchRecommendationsOnLogin();
  }

  fetchRecommendationsOnLogin() {
    const userId = 'current-logged-user-id';
    this.reservationService.askRecommendationsBasedOnPastReservations().subscribe({
      next: (data: string) => {
        this.recommendationsText = data;
        this.isLoading = false;
      } ,
      error: (err) => {
        console.error('Error fetching recommendation text', err);
        this.errorMessage = 'Failed to load recommendations';
        this.isLoading = false;
      }
    });
  }

  nextRecommendation() {
    if (this.currentIndex < this.recommendationsText.length - 1) {
      this.currentIndex++;
    }
  }

  previousRecommendation() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    }
  }

  askHotelRecommendation(): void {
    if(!this.hotelQuestion.trim()) {
      return;
    }

    this.hotelService.askQuestionAboutHotels(this.hotelQuestion).subscribe({
      next: (response) => {
        this.hotelAnswer = response;
      },
      error: (error) => {
        console.error("Failed to fetch hotel recommendation answer:", error);
      }
    });
  }
}