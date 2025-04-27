import { Component, OnInit } from '@angular/core';
import { FooterComponent } from "../../layout/footer/footer.component";
import { HeaderComponent } from "../../layout/header/header.component";
import { MatCardModule } from '@angular/material/card';
import { RecommendationService } from '../../core/services/recommendation.service';
import { HotelRecommendationResult } from '../../shared/models/recommendationModels/hotelRecommendationModel';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    FooterComponent,
    HeaderComponent,
    MatCardModule,
    CommonModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  recommendations: HotelRecommendationResult[] = [];
  currentIndex = 0;

  constructor(private recommendationService: RecommendationService) {}

  ngOnInit() {
    this.fetchRecommendations();
  }

  fetchRecommendations() {
    const userId = 'current-logged-user-id';
    this.recommendationService.recommend(userId).subscribe({
      next: (data) => this.recommendations = data,
      error: (err) => console.error('Error fetching recommendations', err)
    });
  }

  get currentRecommendation(): HotelRecommendationResult {
    return this.recommendations[this.currentIndex];
  }

  nextRecommendation() {
    if (this.currentIndex < this.recommendations.length - 1) {
      this.currentIndex++;
    }
  }

  previousRecommendation() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    }
  }
}