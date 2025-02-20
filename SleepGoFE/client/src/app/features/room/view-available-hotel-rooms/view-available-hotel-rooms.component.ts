import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from "../../../layout/header/header.component";
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { UserService } from '../../../core/services/user.service';
import { JwtService } from '../../../core/services/jwt.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { ResponseRoomModel } from '../../../shared/models/roomModels/responseRoomModel';
import { RoomType } from '../../../shared/models/roomModels/roomType';
import { PaginationRequest } from '../../../shared/models/paginationResponse';
import { RoomService } from '../../../core/services/room.service';
import { UpdateRoomDialogComponent } from '../../../shared/components/update-room-dialog/update-room-dialog.component';
import { UpdateRoomModel } from '../../../shared/models/roomModels/updateRoomModel';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-view-available-hotel-rooms',
  standalone: true,
  imports: [
    HeaderComponent,
    MatCardModule,
    MatButtonModule,
    MatPaginatorModule,
    CommonModule,
    MatIconModule,
    MatListModule,
    MatSelectModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatDialogModule,
    MatMenuModule
  ],
  templateUrl: './view-available-hotel-rooms.component.html',
  styleUrl: './view-available-hotel-rooms.component.scss'
})
export class ViewAvailableHotelRoomsComponent implements OnInit {
  private userService = inject(UserService);
  private jwtService = inject(JwtService);
  private snackbarService = inject(SnackbarService);
  private roomService = inject(RoomService);
  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private lastRoomType: RoomType | null = null;

  hotelId!: string | null;
  rooms: ResponseRoomModel[] = [];
  roomTypes = Object.values(RoomType).filter(value => isNaN(Number(value)));
  totalPages = 0;
  pageIndex = 0;
  pageSize = 5;
  RoomType = RoomType;
  filterForm!: FormGroup;
  selectedRoom: ResponseRoomModel | null = null;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      roomType: [RoomType.Single]
    });
    
    const userId = this.jwtService.getUserIdFromToken();

    if(!userId) {
      this.snackbarService.error('Error: No user ID found!');
      return;
    }

    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        if(!('hotelId' in user)) {
          console.error('Error: Logged0in user does not have a hotel profile.');
          this.snackbarService.error('Error: User is not associated with a hotel.');
          return;
        }

        this.hotelId = user.hotelId;
        console.log('Fetched Hotel ID: ', this.hotelId);

        const paginationRequest: PaginationRequest = {
          pageIndex: 1,
          pageSize: this.pageSize
        }

        this.filterForm.patchValue({ roomType: RoomType.Single });

        this.fetchAvailableRooms(RoomType.Single, paginationRequest);
      }
    });

    this.filterForm.get('roomType')?.valueChanges.subscribe(value => {
      this.pageIndex = 1;
      const paginationRequest: PaginationRequest = {
        pageIndex: this.pageIndex,
        pageSize: this.pageSize
      }

      this.fetchAvailableRooms(value, paginationRequest);
    });
  }

  fetchAvailableRooms(roomType: RoomType, paginationRequest: PaginationRequest): void {
    if(!this.hotelId) {
      console.error("Error: Hotel ID not found!");
      return;
    }

    console.log("Fetching rooms for hotelID:", this.hotelId, "Room Type:", roomType);

    this.roomService.getAvailableRoomsFromHotelByRoomType(this.hotelId, roomType, paginationRequest).subscribe({
      next: (response) => {
        this.rooms = response.items;
        this.totalPages = response.totalPages;
        console.log("Fetched Rooms:", response);

        if (this.lastRoomType !== roomType) {
          this.pageIndex = 0;
          if (this.paginator) {
              this.paginator.firstPage();
          }
          this.lastRoomType = roomType;
        }
      },
    error: (error) => {
      if (error.status === 404) {
        this.rooms = [];
        this.totalPages = 0;
        console.warn("No rooms found for selected type:", roomType);

        if (this.lastRoomType !== roomType) {
            this.pageIndex = 0;
            if (this.paginator) {
                this.paginator.firstPage();
            }
            this.lastRoomType = roomType;
          }
      } else {
        this.snackbarService.error("Error fetching rooms.");
        console.error("Error fetching rooms:", error);
        }
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;

    const paginationRequest: PaginationRequest = {
      pageIndex: this.pageIndex + 1,
      pageSize: this.pageSize
    };

    this.fetchAvailableRooms(this.filterForm.value.roomType, paginationRequest);
  }

  openUpdateDialog(room: ResponseRoomModel): void {
    if (!room) {
      console.warn("No room selected for update.");
      return;
    }

    const dialogRef = this.dialog.open(UpdateRoomDialogComponent, {
      width: '800px',
      data: { ...room, roomId: room.id }
    });

    dialogRef.afterClosed().subscribe((updatedRoom: UpdateRoomModel) => {
      if (updatedRoom) {
        this.updateRoom(room.id, updatedRoom);
      }
    });
  }

  updateRoom(roomId: string, updatedRoom:UpdateRoomModel): void {
    this.fetchAvailableRooms(this.filterForm.value.roomType, {
      pageIndex: this.pageIndex + 1,
      pageSize: this.pageSize
    });
  }

  deleteRoom(roomId: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: { 
        title: 'Delete Room',
        message: 'Are you sure you want to delete this room? This action is permanent!' 
      }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) { 
        this.roomService.removeRoom(roomId).subscribe({
          next: () => {
            this.snackbarService.success("Room deleted successfully!");
            this.fetchAvailableRooms(this.filterForm.value.roomType, {
              pageIndex: this.pageIndex + 1,
              pageSize: this.pageSize
            });
          },
          error: () => {
            this.snackbarService.error("Failed to delete room.");
          }
        });
      }
    });
  }

  setSelectedRoom(room: ResponseRoomModel): void {
    this.selectedRoom = room;
    console.log("Selected Room ID:", this.selectedRoom.id);
  }
}
