import { Component, inject, OnInit } from '@angular/core';
import { HeaderComponent } from "../../layout/header/header.component";
import { Router } from '@angular/router';
import { ResponseHotelModel } from '../../shared/models/responseHotelModel';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationResponse';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { UserService } from '../../core/services/user.service';

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

  private router = inject(Router);
  private userService = inject(UserService);  

  ngOnInit(): void {
    this.loadHotels();
  }

  loadHotels() {
    const paginationRequest: PaginationRequest = { 
      pageIndex: this.pageIndex + 1, 
      pageSize: this.pageSize 
    };

    this.userService.getAllPaginatedHotels(paginationRequest).subscribe((response: PaginationResponse<ResponseHotelModel>) => {
      this.hotels = response.items;
      this.totalHotels = response.totalPages * this.pageSize;

      this.hotels.forEach(hotel => {
        if(hotel.imageId) {
          this.userService.getImageById(hotel.imageId).subscribe(imageResponse => {
            hotel.imageUrl = imageResponse.imageSrc;
          });
        } else {
          hotel.imageUrl = '';
        }
      });
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
