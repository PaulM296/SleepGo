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
    MatNativeDateModule
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

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  ngOnInit(): void {
    this.fetchUserData();
  }

  fetchUserData() {
    this.userService.getLoggedUser().subscribe({
      next: (user) => {
        this.userData = user;
        this.isHotelUser = this.isHotel(user);
        this.userId = user.id;

        this.initializeForm();
        this.snackbarService.success('User data successfully fetched.');
      },
      error: (error) =>  {
        console.error('Error fetching user:', error);
        this.snackbarService.error('Error fetching user data.');
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
        this.fetchUserData;
      }, 
      error: (error) => {
        this.snackbarService.error("Error updating profile!");
      }
    });
  }

  deleteProfilePicture() {
    this.snackbarService.success('Profile picture deleted!');
  }

  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }

  onDateChange(event: any) {
    const selectedDate = event.value;
    this.userForm.patchValue({ dateOfBirth: selectedDate });
  }
}
