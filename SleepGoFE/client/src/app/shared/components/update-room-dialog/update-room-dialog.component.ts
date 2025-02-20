import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogActions, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UpdateRoomModel } from '../../models/roomModels/updateRoomModel';
import { RoomService } from '../../../core/services/room.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { RoomType } from '../../models/roomModels/roomType';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-room-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule, 
    MatSelectModule,
    CommonModule,
    MatDialogModule
  ],
  templateUrl: './update-room-dialog.component.html',
  styleUrl: './update-room-dialog.component.scss'
})
export class UpdateRoomDialogComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<UpdateRoomDialogComponent>);
  private fb = inject(FormBuilder);
  private roomService = inject(RoomService);
  private snackbarService = inject(SnackbarService);
  // data: UpdateRoomModel = inject(MAT_DIALOG_DATA);
  data: any = inject(MAT_DIALOG_DATA);

  updateRoomForm!: FormGroup;
  roomTypes = Object.values(RoomType).filter(value => isNaN(Number(value)));

  ngOnInit(): void {
    this.updateRoomForm = this.fb.group({
      roomType: [this.data.roomType, Validators.required],
      price: [this.data.price, [Validators.required, Validators.min(1)]],
      roomNumber: [this.data.roomNumber, [Validators.required, Validators.min(1)]],
      balcony: [this.data.balcony, Validators.required],
      airConditioning: [this.data.airConditioning, Validators.required],
      kitchenette: [this.data.kitchenette, Validators.required],
      hairdryer: [this.data.hairdryer, Validators.required],
      tv: [this.data.tv, Validators.required],
      isReserved: [this.data.isReserved, Validators.required]
    });
  }

  submitUpdate(): void {
    if (this.updateRoomForm.invalid) return;

    const updatedRoom: UpdateRoomModel = {
      roomType: RoomType[this.updateRoomForm.value.roomType as keyof typeof RoomType],
      price: this.updateRoomForm.value.price,
      roomNumber: this.updateRoomForm.value.roomNumber,
      balcony: this.updateRoomForm.value.balcony,
      airConditioning: this.updateRoomForm.value.airConditioning,
      kitchenette: this.updateRoomForm.value.kitchenette,
      hairdryer: this.updateRoomForm.value.hairdryer,
      tv: this.updateRoomForm.value.tv,
      isReserved: this.updateRoomForm.value.isReserved
    };

    console.log('Submitting updated room data:', updatedRoom);

    this.roomService.updateRoom(this.data.roomId, updatedRoom).subscribe({
      next: () => {
        this.snackbarService.success('Room updated successfully!');
        this.dialogRef.close(true);
      },
      error: (error) => {
        this.snackbarService.error('Failed to update room! ' + error.message);
        console.error('Error updating room:', error);
      }
    });
  }

  cancel(): void {
      this.dialogRef.close();
  }
}
