import { Component, inject, OnInit } from '@angular/core';
import { HeaderComponent } from "../../layout/header/header.component";
import { ActivatedRoute, Router } from '@angular/router';
import { ResponseHotelModel } from '../../shared/models/userModels/responseHotelModel';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModels/paginationResponse';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { UserService } from '../../core/services/user.service';
import { catchError, of, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-hotel',
  standalone: true,
  imports: [
    HeaderComponent,
    MatCardModule,
    MatPaginatorModule,
    MatIconModule,
    CommonModule
  ],
  templateUrl: './hotel.component.html',
  styleUrl: './hotel.component.scss'
})
export class HotelComponent implements OnInit {
  hotels: ResponseHotelModel[] = [];
  totalHotels: number = 0;
  pageIndex: number = 0;
  pageSize: number = 5;
  searchQuery: string | null = null;
  isSearchActive: boolean = false;

  private router = inject(Router);
  private userService = inject(UserService);  
  private route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchQuery = params['query'] || null;
      
      if (!this.searchQuery || this.searchQuery.trim() === '') {
        this.router.navigate(['/hotels'], { queryParams: {} }); 
        this.isSearchActive = false;
        this.loadAllHotels();
      } else {
        this.isSearchActive = true;
        this.loadHotels();
      }
    });
  }

  loadHotels(): void {
    if (!this.searchQuery || this.searchQuery.trim() === '') {
      this.loadAllHotels();
      return;
    }

    this.isSearchActive = true;
    const paginationRequest: PaginationRequest = { pageIndex: this.pageIndex + 1, pageSize: this.pageSize };

    this.userService.searchHotels(this.searchQuery, paginationRequest)
      .pipe(
        tap(response => {
          console.log("Search Response:", response);
          this.hotels = response.items;
          this.totalHotels = response.totalPages * this.pageSize;
          this.loadHotelImages();
        }),
        catchError(error => {
          console.error("Error fetching searched hotels:", error);
          this.hotels = [];
          return of();
        })
      )
      .subscribe();
  }

  loadAllHotels(): void {
    this.isSearchActive = false;
    const paginationRequest: PaginationRequest = { pageIndex: this.pageIndex + 1, pageSize: this.pageSize };

    this.userService.getAllPaginatedHotels(paginationRequest)
      .pipe(
        tap(response => {
          console.log("All Hotels Response:", response);
          this.hotels = response.items;
          this.totalHotels = response.totalPages * this.pageSize;
          this.loadHotelImages();
        }),
        catchError(error => {
          console.error("Error fetching all hotels:", error);
          this.hotels = [];
          return of();
        })
      )
      .subscribe();
  }

  loadHotelImages() {
    if (!this.hotels || this.hotels.length === 0) {
      console.warn("No hotels found, skipping image loading.");
      return;
    }
  
    this.hotels.forEach(hotel => {
      if (hotel.imageId) {
        this.userService.getImageById(hotel.imageId).subscribe(imageResponse => {
          hotel.imageUrl = imageResponse.imageSrc;
        });
      } else {
        hotel.imageUrl = '';
      }
    });
  }

  onPageChange(event: PageEvent): void {
    if (event.pageIndex * this.pageSize >= this.totalHotels) {
      console.warn("No more pages, preventing navigation.");
      return; 
    }
  
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadHotels();
  }

  navigateToHotel(id: string) {
    this.router.navigate(['/hotels', id]);
  }

}
