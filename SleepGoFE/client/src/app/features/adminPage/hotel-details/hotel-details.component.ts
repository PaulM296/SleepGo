import { AfterViewInit, ChangeDetectorRef, Component, inject, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../../../core/services/user.service';
import { ResponseHotelModel } from '../../../shared/models/userModels/responseHotelModel';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { MatCard } from '@angular/material/card';
import { HeaderComponent } from "../../../layout/header/header.component";

@Component({
  selector: 'app-hotel-details',
  standalone: true,
  imports: [
    MatButtonModule,
    MatTableModule,
    MatToolbarModule,
    MatPaginatorModule,
    CommonModule,
    MatCard,
    HeaderComponent
],
  templateUrl: './hotel-details.component.html',
  styleUrl: './hotel-details.component.scss'
})
export class HotelDetailsComponent implements OnInit, AfterViewInit {
  hotels: ResponseHotelModel[] = [];
  dataSource = new MatTableDataSource<ResponseHotelModel>();
  displayedColumns: string[] = ['username', 'email', 'hotelName', 'actions'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private userService = inject(UserService);
  private snackbarService = inject(SnackbarService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit(): void {
    this.loadHotels();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.cdr.detectChanges();
  }

  loadHotels(): void {
    this.userService.getPaginatedHotels(1, 10).subscribe(response => {
      this.hotels = response.items;
      this.dataSource.data = response.items;
    });
  }

  toggleBlock(hotel: ResponseHotelModel): void {
    if (hotel.isBlocked) {
      this.userService.unblockUser(hotel.id).subscribe({
        next: () => {
          hotel.isBlocked = false;
          this.snackbarService.success(`${hotel.userName} has been unblocked`);
        }, 
        error: () => {
          this.snackbarService.error(`Failed to unblock ${hotel.userName}. Please try again.`);
        }
      });
    } else {
      this.userService.blockUser(hotel.id).subscribe({
        next: () => {
          hotel.isBlocked = true;
          this.snackbarService.success(`${hotel.userName} has been blocked`);
        }, 
        error: () => {
          this.snackbarService.error(`Failed to block ${hotel.userName}. Please try again.`);
        }
      });
    }
  }

  previousPage(): void {
    if (this.paginator && this.paginator.hasPreviousPage()) {
      this.paginator.previousPage();
    }
  }

  nextPage(): void {
    if (this.paginator && this.paginator.hasNextPage()) {
      this.paginator.nextPage();
    }
  }
}
