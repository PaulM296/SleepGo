import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReviewService } from '../../../core/services/review.service';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CreateReviewModel } from '../../../shared/models/reviewModels/createReviewModel';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { JwtService } from '../../../core/services/jwt.service';

@Component({
  selector: 'app-write-reviews',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule
  ],
  templateUrl: './write-reviews.component.html',
  styleUrl: './write-reviews.component.scss'
})
export class WriteReviewsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private reviewService = inject(ReviewService);
  private dialogRef = inject(MatDialogRef<WriteReviewsComponent>);
  private data: { hotelId: string } = inject(MAT_DIALOG_DATA);
  private snackbarService = inject(SnackbarService);
  private jwtService = inject(JwtService);

  reviewForm!: FormGroup;
  characterCount: number = 0;
  userId!: string | null;

  ngOnInit(): void {
    this.userId = this.jwtService.getUserIdFromToken();

    this.reviewForm = this.fb.group({
      reviewText:['', [Validators.required, Validators.maxLength(1500)]],
      rating: [null, [Validators.required, Validators.min(1), Validators.max(10)]]
    });
  }

  updateCharacterCount() {
    this.characterCount = this.reviewForm.get('reviewText')?.value.length || 0;
  }

  submitReview() {
    if (this.reviewForm.valid) {
      const reviewData = {
        hotelId: this.data.hotelId,
        reviewText: this.reviewForm.value.reviewText,
        rating: this.reviewForm.value.rating
      };
  
      this.reviewService.addReview(reviewData).subscribe({
        next: () => {
          this.snackbarService.success("Review added successfully!");
          this.dialogRef.close(true);
        }, 
        error: (error) => {
          console.error("Error adding review: ", error.error);
          this.snackbarService.error(error.error?.title || "Error adding review!");
        }
      });
    }
  }
  
  closeDialog() {
    this.dialogRef.close(false);
  }
}
