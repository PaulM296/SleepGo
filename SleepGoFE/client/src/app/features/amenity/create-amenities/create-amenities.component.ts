import { Component, inject, OnInit } from '@angular/core';
import { JwtService } from '../../../core/services/jwt.service';
import { HeaderComponent } from '../../../layout/header/header.component';
import { UserService } from '../../../core/services/user.service';
import { AmenityService } from '../../../core/services/amenity.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Router } from '@angular/router';
import { ResponseHotelModel } from '../../../shared/models/userModels/responseHotelModel';
import { RoomService } from '../../../core/services/room.service';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-create-amenities',
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
    MatOptionModule,
    MatSelectModule
  ],
  templateUrl: './create-amenities.component.html',
  styleUrl: './create-amenities.component.scss'
})
export class CreateAmenitiesComponent implements OnInit {
  private jwtService = inject(JwtService);
  private userService = inject(UserService);
  private amenityService = inject(AmenityService);
  private fb = inject(FormBuilder);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);

  amenitiesForm!: FormGroup;
  hotelId!: string | null;

  ngOnInit(): void {
    const userId = this.jwtService.getUserIdFromToken();

    if(!userId) {
      this.snackbarService.error('You must be logged in to create amenities!');
      this.router.navigate(['/login']);
      return;
    }

    console.log('User ID: ', userId);

    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        if(this.isHotelUser(user)) {
          this.hotelId = user.hotelId;
          console.log('Fecthed Hotel ID: ', this.hotelId);
        } else {
          this.snackbarService.error('Error: User does not have a hotel profile!');
        }
      },
      error: (error) => {
        console.error('Error fetching user: ', error);
        this.snackbarService.error('Error fetching user details!');
      }
    });

    this.amenitiesForm = this.fb.group({
      pool: [false, Validators.required],
      restaurant: [false, Validators.required],
      fitness: [false, Validators.required],
      wiFi: [false, Validators.required],
      roomService: [false, Validators.required],
      bar: [false, Validators.required]
    });
  }

  private isHotelUser(user: any): user is ResponseHotelModel {
    return 'hotelId' in user;
  }

  submitAmenities(): void {
    if(this.amenitiesForm.invalid) return;

    const formData = new FormData();
    formData.append('hotelId', this.hotelId ?? '');
    formData.append('pool', this.amenitiesForm.value.pool);
    formData.append('restaurant', this.amenitiesForm.value.restaurant);
    formData.append('fitness', this.amenitiesForm.value.fitness);
    formData.append('wiFi', this.amenitiesForm.value.wiFi);
    formData.append('roomService', this.amenitiesForm.value.roomService);
    formData.append('bar', this.amenitiesForm.value.bar);

    console.log('Sending form data: ', formData);

    this.amenityService.addAmenities(formData).subscribe({
      next: () => {
        this.snackbarService.success('Amenities added successfully!');
        this.router.navigate(['/']);
      }, 
      error: (error) => {
        this.snackbarService.error('Failed to add new amenities!');
        console.error('Failed to add new amenities: ', error);
      }
    });
  }
}
