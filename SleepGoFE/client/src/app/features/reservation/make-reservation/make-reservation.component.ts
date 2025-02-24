import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { ReservationService } from '../../../core/services/reservation.service';
import { JwtService } from '../../../core/services/jwt.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ResponseHotelModel } from '../../../shared/models/userModels/responseHotelModel';
import { ResponseRoomModel } from '../../../shared/models/roomModels/responseRoomModel';
import { ReservationStatus } from '../../../shared/models/reservationModels/reservationStatus';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { HeaderComponent } from "../../../layout/header/header.component";
import { UserService } from '../../../core/services/user.service';
import { RoomService } from '../../../core/services/room.service';
import { RoomType } from '../../../shared/models/roomModels/roomType';
import { PaginationRequest } from '../../../shared/models/paginationModels/paginationResponse';
import { CreateReservationModel } from '../../../shared/models/reservationModels/createReservationModel';
import { ResponseReservationModel } from '../../../shared/models/reservationModels/responseReservationModel';

@Component({
  selector: 'app-make-reservation',
  standalone: true,
  imports: [
    MatButtonModule,
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatOptionModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatNativeDateModule,
    MatInputModule,
    MatCardModule,
    HeaderComponent
],
  templateUrl: './make-reservation.component.html',
  styleUrl: './make-reservation.component.scss'
})
export class MakeReservationComponent implements OnInit {
  private jwtService = inject(JwtService);
  private userService = inject(UserService);
  private roomService = inject(RoomService);
  private reservationService = inject(ReservationService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private fb = inject(FormBuilder);

  reservationForm!: FormGroup;
  hotelId!: string | null;
  hotel!: ResponseHotelModel;
  rooms!: ResponseRoomModel[];
  totalPrice: number = 0;
  availableRooms: ResponseRoomModel[] = [];
  availableRoomTypes: RoomType[] = [];

  ngOnInit(): void {
    const userId = this.jwtService.getUserIdFromToken();

    if(!userId) {
      this.snackbarService.error('You must be logged in to make a reservation!')
      this.router.navigate(['/login']);
      return;
    }

    this.route.queryParams.subscribe(params => {
      this.hotelId = params['hotelId'];

      if(!this.hotelId) {
        console.error('Hotel ID is missing in query params!');
        return;
      }

      console.log('Hotel ID: ', this.hotelId);
      this.fetchAvailableRoomsFromHotel();
    });

    this.reservationForm = this.fb.group({
      roomType: ['', Validators.required],
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required]
    });

    this.reservationForm.valueChanges.subscribe(() => {
      this.calculateTotalPrice();
    })
  }

  calculateTotalPrice() {
    const formValues = this.reservationForm.value;
    const {
      roomType, 
      checkIn,
      checkOut 
    } = formValues;

    if(!roomType || !checkIn || !checkOut) {
      this.totalPrice = 0;
      return;
    }

    const selectedRoom = this.availableRooms.find(room => room.roomType === roomType);

    if(!selectedRoom) {
      console.warn('Selected room type not found in available rooms');
      this.totalPrice = 0;
      return;
    }

    const checkInDate = new Date(checkIn);
    const checkOutDate = new Date(checkOut);

    if(checkOutDate <= checkInDate) {
      console.error('Invalid dates: Check-out can not be made before the check-in!');
      this.totalPrice = 0;
      return;
    }

    const numberOfNights = Math.ceil((checkOutDate.getTime() - checkInDate.getTime()) / (1000 * 60 * 60 * 24));

    this.totalPrice = numberOfNights * selectedRoom.price;

    console.log(`Total Price: ${this.totalPrice} (${numberOfNights} nights * ${selectedRoom.price} per night)`);
  }

  fetchAvailableRoomsFromHotel(): void {
    if(!this.hotelId) {
      console.error('Hotel ID is missing! Cannot fetch rooms!');
      return;
    }

    this.roomService.getAllAvailableRoomsFromHotelByHotelId(this.hotelId).subscribe({
      next: (rooms: ResponseRoomModel[]) => {
        this.availableRooms = rooms;
        this.extractUniqueRoomTypes();
        console.log('Available rooms:', this.availableRooms);
      },
      error: (error) => {
        console.error('Error fetching available rooms:', error);
      }
    })
  }

  extractUniqueRoomTypes(): void {
    this.availableRoomTypes = Array.from(
      new Set(this.availableRooms.map(room => room.roomType))
    ).sort((a, b) => a - b);
  }

  getRoomTypeName(roomType: RoomType): string {
    return RoomType[roomType];
  }

  onSubmit(): void {
    if(this.reservationForm.invalid) {
      this.snackbarService.error('Please fill out all fields before submitting.');
      return;
    }

    const formValues = this.reservationForm.value;
    const { roomType, checkIn, checkOut } = formValues;

    const selectedRoom = this.availableRooms.find(room => room.roomType === roomType);

    if (!selectedRoom) {
      this.snackbarService.error('Selected room type is not available.');
      return;
    }

    const reservationData: CreateReservationModel = {
      hotelId: this.hotelId!,
      roomType: roomType,
      checkIn: new Date(checkIn),
      checkOut: new Date(checkOut),
      price: this.totalPrice,
      status: ReservationStatus.Pending
    };

    const formData = new FormData();
    formData.append('hotelId', reservationData.hotelId);
    formData.append('roomType', reservationData.roomType as any);
    formData.append('checkIn', reservationData.checkIn.toISOString());
    formData.append('checkOut', reservationData.checkOut.toISOString());
    formData.append('price', reservationData.price.toString());
    formData.append('status', reservationData.status);

    this.reservationService.addReservation(formData).subscribe({
      next: (response: ResponseReservationModel) => {
        this.snackbarService.success(`Reservation confirmed at ${response.hotelName}`);
        this.router.navigate(['/user-reservations']);
      }, 
      error: (error) => {
        console.error('Reservation failed:', error);
      this.snackbarService.error('Failed to create reservation. Please try again.');
      }
    });
  }

}
