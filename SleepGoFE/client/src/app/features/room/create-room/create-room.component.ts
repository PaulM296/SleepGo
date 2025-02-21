import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RoomService } from '../../../core/services/room.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { JwtService } from '../../../core/services/jwt.service';
import { RoomType } from '../../../shared/models/roomModels/roomType';
import { Router } from '@angular/router';
import { UserService } from '../../../core/services/user.service';
import { ResponseHotelModel } from '../../../shared/models/userModels/responseHotelModel';
import { HeaderComponent } from "../../../layout/header/header.component";

@Component({
  selector: 'app-create-room',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    HeaderComponent
],
  templateUrl: './create-room.component.html',
  styleUrl: './create-room.component.scss'
})
export class CreateRoomComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roomService = inject(RoomService);
  private snackbarService = inject(SnackbarService);
  private jwtService = inject(JwtService);
  private router = inject(Router);
  private userService = inject(UserService)

  roomForm!: FormGroup;
  roomTypes = Object.values(RoomType).filter(value => isNaN(Number(value)));
  hotelId!: string | null;

  ngOnInit(): void {
    const userId = this.jwtService.getUserIdFromToken();

    if(!userId) {
      this.snackbarService.error("Error: user ID not found!");
      return;
    }

    console.log('UserId:', userId);

    this.userService.getUserById(userId).subscribe({
      next: (user) => {

        console.log("Fetched user object: ", user);

        if (this.isHotelUser(user)) {
          this.hotelId = user.hotelId;
          console.log("Fetched Hotel ID:", this.hotelId);
        } else {
          this.snackbarService.error("Error: User does not have a hotel profile!");
        }
      },
      error: (error) => {
        this.snackbarService.error("Error fetching user details!");
        console.error("Error fetching user:", error);
      }
    });

    this.roomForm = this.fb.group({
      roomType: ['', Validators.required],
      roomNumber: ['', [Validators.required, Validators.min(1)]],
      price: ['', [Validators.required, Validators.min(1)]],
      balcony: [false, Validators.required],
      airConditioning: [false, Validators.required],
      kitchenette: [false, Validators.required],
      hairdryer: [false, Validators.required],
      tv: [false, Validators.required],
      isReserved: [false, Validators.required]
    });
  }

  private isHotelUser(user: any): user is ResponseHotelModel {
    return 'hotelId' in user;
  }

  submitRoom(): void {
    if (this.roomForm.invalid) return;

    const formData = new FormData();
    formData.append('hotelId', this.hotelId ?? '');
    formData.append('roomType', this.roomForm.value.roomType);
    formData.append('roomNumber', this.roomForm.value.roomNumber.toString());
    formData.append('price', this.roomForm.value.price.toString());
    formData.append('balcony', this.roomForm.value.balcony.toString());
    formData.append('airConditioning', this.roomForm.value.airConditioning.toString());
    formData.append('kitchenette', this.roomForm.value.kitchenette.toString());
    formData.append('hairdryer', this.roomForm.value.hairdryer.toString());
    formData.append('tv', this.roomForm.value.tv.toString());
    formData.append('isReserved', this.roomForm.value.isReserved.toString());

    console.log('Sending form data:', formData);

    this.roomService.addRoomToHotel(formData).subscribe({
      next: () => {
        this.snackbarService.success('Room added successfully!');
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.snackbarService.error('Failed to add new room! ' + error.message);
        console.error('Error adding new room:', error);
      }
    });
  }
}
