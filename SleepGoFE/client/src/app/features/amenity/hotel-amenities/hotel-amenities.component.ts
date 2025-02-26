import { Component, inject, OnInit, signal } from '@angular/core';
import { HeaderComponent } from '../../../layout/header/header.component';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { AmenityService } from '../../../core/services/amenity.service';
import { JwtService } from '../../../core/services/jwt.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '../../../core/services/user.service';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-hotel-amenities',
  standalone: true,
  imports: [
    HeaderComponent,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    CommonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatIconModule,
    MatOptionModule,
    MatSelectModule
  ],
  templateUrl: './hotel-amenities.component.html',
  styleUrl: './hotel-amenities.component.scss'
})
export class HotelAmenitiesComponent implements OnInit {
  private amenityService = inject(AmenityService);
  private jwtService = inject(JwtService);
  private snackbarService = inject(SnackbarService);
  private dialog = inject(MatDialog);
  private userService = inject(UserService);
  private fb = inject(FormBuilder);
  private router = inject(Router);

  hotelId!: string | null;
  amenityId!: string | null;
  hasAmenities = false;
  amenitiesForm!: FormGroup;
  isEditMode = false;

  ngOnInit(): void {
    const userId = this.jwtService.getUserIdFromToken();

    if(!userId) {
      this.snackbarService.error('Error: No user Id found!');
      return;
    }

    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        if(!('hotelId' in user)) {
          console.error('Error: Logged-in user does not have a hotel profile.');
          this.snackbarService.error("Error: User is not associated with a hotel.");
          return;
        }

        this.hotelId = user.hotelId;
        console.log('Fecthed Hotel ID: ', this.hotelId);

        this.fetchAmenities();
      }
    });
  }

  fetchAmenities(): void {
    if(!this.hotelId) {
      console.error('Error: Hotel ID not found!');
      return;
    }

    this.amenitiesForm = this.fb.group({
      pool: [{ value: false, disabled: true }, Validators.required],
      restaurant: [{ value: false, disabled: true }, Validators.required],
      fitness: [{ value: false, disabled: true }, Validators.required],
      wiFi: [{ value: false, disabled: true }, Validators.required],
      roomService: [{ value: false, disabled: true }, Validators.required],
      bar: [{ value: false, disabled: true }, Validators.required]
    });

    this.amenityService.getAmenitiesByHotelId(this.hotelId).subscribe({
      next: (response) => {
        if(!response) {
          console.error('Error: Api response is undefined!');
          return;
        }

        if (!response || (Array.isArray(response) && response.length === 0)) {
          console.log("No amenities found.");
          this.hasAmenities = false;
          return;
        }

        if(Array.isArray(response) && response.length > 0) {
          response = response[0];
        }

        this.amenityId = response.id;
        this.hasAmenities = true;

        this.amenitiesForm.patchValue({
          pool: response.pool ?? false,
          restaurant: response.restaurant ?? false,
          fitness: response.fitness ?? false,
          wiFi: response.wiFi ?? false,
          roomService: response.roomService ?? false,
          bar: response.bar ?? false
        });

        console.log('API Response:', response);
        console.log('Updated Form Values:', this.amenitiesForm.value);
      },
      error: (error) => {
        this.snackbarService.error('Error fetching amenities!');
            console.error('Error fetching amenities:', error);
      }
    });
  }

  toggleEditMode(): void {
    this.isEditMode = !this.isEditMode;

    Object.keys(this.amenitiesForm.controls).forEach(field => {
      if(this.isEditMode) {
        this.amenitiesForm.controls[field].enable();
      } else {
        this.amenitiesForm.controls[field].disable();
      }
    });
  }

  saveChanges(): void {
    if(this.amenitiesForm.invalid || !this.amenityId) return;

    const updatedAmenities = this.amenitiesForm.value;

    this.amenityService.updateAmenities(this.amenityId, updatedAmenities).subscribe({
      next: () => {
        this.snackbarService.success('Amenities updated successfully!');
        this.toggleEditMode();
      }, 
      error: (error) => {
        this.snackbarService.error('Failed to update amenities!');
        console.error('Failed to update amenities: ', error);
      }
    });
  }

  cancelEdit(): void {
    this.toggleEditMode();
    this.fetchAmenities();
  }

  openDeleteDialog() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Amenities',
        message: 'Are you sure you want to delete your amenities? This action is permanent!'
      }
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if(confirmed) {
        this.deleteAmenities();
      }
    })
  }

  deleteAmenities() {
    if(!this.amenityId) return;

    this.amenityService.deleteAmenities(this.amenityId).subscribe({
      next: () => {
        this.snackbarService.success('Amenities deleted successfully!');
      },
      error: (error) => {
        this.snackbarService.error('Failed to delete amenities!');
        console.error('Error: Failed to delete amenities!', error);
      }   
    });
  }

  navigateToCreateHotelAmenities() {
    this.router.navigate(['/create-amenities']);
  }
}
