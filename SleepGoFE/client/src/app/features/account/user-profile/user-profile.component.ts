import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ResponseUserModel } from '../../../shared/models/responseUserModel';
import { ResponseHotelModel } from '../../../shared/models/responseHotelModel';
import { UserService } from '../../../core/services/user.service';
import { JwtService } from '../../../core/services/jwt.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AvatarComponent } from '../../../shared/components/avatar/avatar.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    ConfirmDialogComponent,
    AvatarComponent
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent implements OnInit {
  userForm!: FormGroup;
  userData!: ResponseUserModel | ResponseHotelModel;
  isHotelUser: boolean = false;
  userId!: string;
  avatarHover: boolean = false;
  selectedFile: File | null = null;

  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private jwtService = inject(JwtService);
  private snackbarService = inject(SnackbarService);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  ngOnInit(): void {
  this.userForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    userName: ['', [Validators.required]],
    phoneNumber: ['', [Validators.required]],
    firstName: [''],
    lastName: [''],
    dateOfBirth: [''],
    hotelName: [''],
    city: [''],
    country: [''],
    zipCode: [''],
    address: [''],
    latitude: [null],
    longitude: [null],
    hotelDescription: ['']
  });

  this.fetchUserData();
}


  fetchUserData() {
    this.userService.getLoggedUser().subscribe({
      next: (user) => {
        this.userData = user;
        this.isHotelUser = this.isHotel(user);
        this.userId = user.id;

        this.initializeForm();
        this.fetchUserImage();

        console.log('User data successfully fetched.');
      },
      error: (error) =>  {
        console.error('Error fetching user:', error);
      }
    });
  }

  isHotel(data: ResponseUserModel | ResponseHotelModel): data is ResponseHotelModel {
    return 'hotelName' in data;
  }

  initializeForm() {
    this.userForm = this.fb.group({
      email: [this.userData.email, [Validators.required, Validators.email]],
      userName: [this.userData.userName, [Validators.required]],
      phoneNumber: [this.userData.phoneNumber, [Validators.required]],
      ...(this.isHotel(this.userData)
        ? {
          hotelName: [this.userData.hotelName, Validators.required],
          city: [this.userData.city, Validators.required],
          country: [this.userData.country, Validators.required],
          zipCode: [this.userData.zipCode, Validators.required],
          address: [this.userData.address, Validators.required],
          latitude: [this.userData.latitude, Validators.required],
          longitude: [this.userData.longitude, Validators.required],
          hotelDescription: [this.userData.hotelDescription, Validators.required],

        } : {
          firstName: [this.userData.firstName, Validators.required],
          lastName: [this.userData.lastName, Validators.required],
          dateOfBirth: [this.userData.dateOfBirth, Validators.required]
        })
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  updateProfile() {
    if(this.userForm.invalid) return;

    const formData = new FormData();

    Object.keys(this.userForm.controls).forEach((key) => {
      formData.append(key, this.userForm.get(key)?.value);
    });

    if(this.selectedFile) {
      formData.append('profilePicture', this.selectedFile);
    }

    this.userService.updateUser(this.userId, formData).subscribe({
      next: () => {
        this.snackbarService.success("Profile updated successfully!");
      }, 
      error: (error) => {
        this.snackbarService.error("Error updating profile!");
      }
    });
  }

  deleteProfilePicture() {
    if(!this.userData?.imageId) {
      this.snackbarService.error('No profile picture to delete.');
      return;
    }

    this.userService.deleteImage(this.userData.imageId).subscribe({
      next: () => {
        this.snackbarService.success('Profile picture deleted successfully!');
        this.userData.imageId = '';
        this.userData.imageUrl = '';
      }, 
      error: (error) => {
        console.log('Error deleting profile picture: ', error);
        this.snackbarService.error('Failed to delete profile picture.');
      }
    });
  }

  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }

  onDateChange(event: any) {
    const selectedDate = event.value;
    this.userForm.patchValue({ dateOfBirth: selectedDate });
  }

  fetchUserImage() {
    if (this.userData?.imageId) {
      this.userService.getImageById(this.userData.imageId).subscribe({
        next: (response) =>  {
          this.userData.imageUrl = response.imageSrc;
        },
        error: (error) => {
          console.error('Error fetching image:', error);
        }
      });
    }
  }

  getUserImage(): string {
    return this.userData?.imageUrl || '';
  }

  openDeleteDialog() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Account',
        message: 'Are you sure you want to delete your account? This action cannot be undone.'
      }
    });
  
    dialogRef.afterClosed().subscribe((confirmed) => {
      if (confirmed) {
        this.deleteAccount();
      }
    });
  }

  deleteAccount() {
    this.userService.deleteUser(this.userId).subscribe({
      next: () => {
        this.snackbarService.success('Account deleted successfully.');
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Error deleting account:', error);
        this.snackbarService.error('Failed to delete account.');
      }
    });
  }
}
