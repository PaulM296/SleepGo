import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { ReviewService } from '../../../core/services/review.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UpdateReviewModel } from '../../models/reviewModels/updateReviewModel';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-review-dialog',
  standalone: true,
  imports: [
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './edit-review-dialog.component.html',
  styleUrl: './edit-review-dialog.component.scss'
})
export class EditReviewDialogComponent implements OnInit {
  private reviewService = inject(ReviewService);
  private dialogRef = inject(MatDialogRef<EditReviewDialogComponent>);
  private snackbarService = inject(SnackbarService);
  private data = inject(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);

  reviewForm!: FormGroup;
  characterCount: number = 0;

  ngOnInit(): void {
    this.reviewForm = this.fb.group({
      reviewText: [this.data.review.reviewText, [Validators.required, Validators.maxLength(1500)]],
      rating: [this.data.review.rating, [Validators.required, Validators.min(1), Validators.max(10)]]
    });

    this.characterCount = this.reviewForm.get('reviewText')?.value.length || 0;
  }

  updateCharacterCount() {
    this.characterCount = this.reviewForm.get('reviewText')?.value.length || 0;
  }

  onSave(): void {
    if (this.reviewForm.valid) {
      const updateModel: UpdateReviewModel = {
        reviewText: this.reviewForm.value.reviewText,
        rating: this.reviewForm.value.rating
      };

      this.reviewService.updateReview(this.data.review.id, updateModel).subscribe({
        next: () => {
          this.snackbarService.success("Review updated successfully!");
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error("Error updating review:", error.error);
          this.snackbarService.error(error.error?.title || "Error updating review!");
        }
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
