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
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private reservationService = inject(ReservationService);
  private jwtService = inject(JwtService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);

  reservationForm!: FormGroup;
  userId!: string | null;
  hotelId!: string;
  hotel!: ResponseHotelModel;
  rooms: ResponseRoomModel[] = [];
  selectedRoom!: ResponseRoomModel;
  totalPrice: number = 0;

  ngOnInit(): void {
    this.userId = this.jwtService.getUserIdFromToken();
    if(!this.userId) {
      this.snackbarService.error('You must be logged in to make a reservation.');
      this.router.navigate(['/login']);
      return;
    }

    this.reservationForm = this.fb.group({
      roomType: ['', Validators.required],
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required]
    });

    this.reservationForm.valueChanges.subscribe(() => {
      this.calculateTotalPrice();
    });

    this.route.queryParams.subscribe(params => {
      this.hotelId = params['hotelId'];
      if(this.hotelId) {
        this.fetchHotelDetails();
      }
    });
  }

  fetchHotelDetails() {
    
  }

  onRoomSelection(event: any) {
    this.selectedRoom = this.rooms.find(r => r.roomType === event.value)!;
    this.calculateTotalPrice();
  }

  calculateTotalPrice() {
    if(this.selectedRoom && this.reservationForm.value.checkIn && this.reservationForm.value.checkOut) {
      const checkInDate = new Date(this.reservationForm.value.checkIn);
      const checkOutDate = new Date(this.reservationForm.value.checkOut);
      const days = (checkOutDate.getTime() - checkInDate.getTime()) / (1000 * 3600 * 24);
      this.totalPrice = days * this.selectedRoom.price;
    }
  }

  submitReservation() {
    if(this.reservationForm.invalid) return;

    const reservationData = {
      hotelId: this.hotelId,
      roomType: this.reservationForm.value.roomType,
      checkIn: this.reservationForm.value.checkIn,
      checkOut: this.reservationForm.value.checkOut,
      price: this.totalPrice,
      status: ReservationStatus.Pending
    }; 

    this.reservationService.addReservation(reservationData).subscribe({
      next: () => {
        this.snackbarService.success('Reservation successful!');
        this.router.navigate(['/profile']);
      },
      error: (error) => {
        this.snackbarService.error(error.error?.message || 'Reservation failed.');
      }
    });
  }
}
