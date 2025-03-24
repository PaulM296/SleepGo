import { Component, inject, OnInit, signal } from '@angular/core';
import { HeaderComponent } from "../../../layout/header/header.component";
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { ReservationService } from '../../../core/services/reservation.service';
import { JwtService } from '../../../core/services/jwt.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { ResponseReservationModel } from '../../../shared/models/reservationModels/responseReservationModel';
import { PaginationRequest, PaginationResponse } from '../../../shared/models/paginationModels/paginationResponse';
import { ResponseReviewModel } from '../../../shared/models/reviewModels/responseReviewModel';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-reservation',
  standalone: true,
  imports: [
    HeaderComponent,
    MatCardModule,
    CommonModule,
    MatButtonModule,
    MatPaginatorModule,
    MatMenuModule,
    MatDialogModule,
    MatListModule,
    MatIconModule
  ],
  templateUrl: './user-reservation.component.html',
  styleUrl: './user-reservation.component.scss'
})
export class UserReservationComponent implements OnInit {
  private reservationService = inject(ReservationService);
  private jwtService = inject(JwtService);
  private snackbarService = inject(SnackbarService);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  reservations = signal<ResponseReservationModel[]>([]);
  totalReservations = signal<number>(0);
  pageSize: number = 5;
  pageIndex: number = 0;

  ngOnInit(): void {
    this.loadUserReservations();
  }

  loadUserReservations(): void {
    const userId = this.jwtService.getUserIdFromToken();

    if(!userId) {
      console.error('Error: No user ID found!');
      return;
    }

    const paginationRequest: PaginationRequest = {
      pageIndex: this.pageIndex + 1,
      pageSize: this.pageSize
    };

    this.reservationService.getAllUserReservations(userId, paginationRequest).subscribe({
      next: (response: PaginationResponse<ResponseReservationModel>) =>{
        console.log('Received reservations response:', response);
        this.reservations.set(response.items);

        if(this.pageIndex === 0) {
          this.fetchTotalReservations(userId);
        } else {
          this.totalReservations.set(Math.max(this.totalReservations(), response.items.length + this.pageIndex * this.pageSize));
        }
      },
      error: (error) => console.error("Error fetching user reviews:", error)
    });
  }

  fetchTotalReservations(userId: string): void {
    const paginationRequest: PaginationRequest = {
      pageIndex: 1,
      pageSize: 100
    };

    this.reservationService.getAllUserReservations(userId, paginationRequest).subscribe({
      next: (response: PaginationResponse<ResponseReservationModel>) => {
        this.totalReservations.set(response.items.length);
      },
      error: (error) => console.log("Error fetching total reservations: ", error)
    });
  }

  deleteReservation(reservationId: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Reservation',
        message: 'Are you sure you want to cancel this reservation? This action is permament!'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result) {
        this.reservationService.deleteReservation(reservationId).subscribe({
          next: () => {
            this.snackbarService.success('Reservation was successfully canceled!');
            this.loadUserReservations();
          },
          error: (error) => {
            this.snackbarService.error('Failed to cancel reservation!');
            console.error('Error deleting reservation: ', error);
          }
        });
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadUserReservations();
  }

  trackById(items: number, item: ResponseReservationModel): string {
    return item.id;
  }

  navigateToPayment(reservationId: string): void {
    this.router.navigate(['/payment', reservationId]);
  }
}
