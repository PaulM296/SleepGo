import { Component, inject, OnInit } from '@angular/core';
import { HeaderComponent } from "../../layout/header/header.component";
import { ActivatedRoute, Router } from '@angular/router';
import { ResponseHotelModel } from '../../shared/models/responseHotelModel';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationResponse';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { UserService } from '../../core/services/user.service';
import { switchMap } from 'rxjs';

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

  loadHotels() {
    if (!this.searchQuery || this.searchQuery.trim() === '') {
      this.loadAllHotels();
      return;
    }
  
    this.isSearchActive = true;
    this.userService.searchHotels(this.searchQuery)
      .subscribe((hotels: ResponseHotelModel[]) => {
  
        if (hotels) {
          this.hotels = hotels;
          this.loadHotelImages();
        } else {
          this.hotels = [];
        }
      }, error => {
        console.error("Error fetching searched hotels:", error);
        this.hotels = [];
      });
  }

  loadAllHotels() {
    this.isSearchActive = false;
    this.userService.getAllPaginatedHotels({ pageIndex: 1, pageSize: 10 })
      .subscribe((response: PaginationResponse<ResponseHotelModel>) => {
        console.log("All Hotels Response:", response);
  
        if (response && response.items) {
          this.hotels = response.items;
          this.loadHotelImages();
        } else {
          this.hotels = [];
        }
      }, error => {
        console.error("Error fetching all hotels:", error);
        this.hotels = [];
      });
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


  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadHotels();
  }

  navigateToHotel(id: string) {
    this.router.navigate(['/hotels', id]);
  }

}
