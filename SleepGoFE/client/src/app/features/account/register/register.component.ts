import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { UserService } from '../../../core/services/user.service';
import { AuthResponse } from '../../../shared/models/authResponse';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatStepperModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCardModule,
    RouterModule,
    CommonModule,
    MatNativeDateModule,
    MatDatepickerModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private formBuilder = inject(FormBuilder);
  private snackbarService = inject(SnackbarService);
  private userService = inject(UserService);
  errorMessage: string = '';
  router = inject(Router);
  today = new Date();

  role = signal<'user' | 'hotel' | null>(null);

  baseForm: FormGroup = this.formBuilder.group({
    userName: ['', [Validators.required, Validators.minLength(6)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(10)]],
    phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
    role: ['', Validators.required]
  });

  userForm: FormGroup = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    image: [null]
  });

  hotelForm: FormGroup = this.formBuilder.group({
    hotelName: ['', Validators.required],
    address: ['', Validators.required],
    city: ['', Validators.required],
    country: ['', Validators.required],
    zipCode: ['', Validators.required]
  });

  hotelExtraForm: FormGroup = this.formBuilder.group({
    hotelDescription: ['', Validators.required],
    latitude: [null, Validators.required],
    longitude: [null, Validators.required],
    image: [null]
  });

  futureDateValidator(control: any) {
    const inputDate = new Date(control.value);
    const today = new Date();

    return inputDate > today ? { futureDateNotAllowed: true } : null;
  }

  onRoleChange(role: 'user' | 'hotel') {
    this.role.set(role);
  }

  onFileChange(event: any, form: FormGroup) {
    const file: File = event.target.files[0];
    if(file) {
      const allowedExtensions = ['image/png', 'image/jpeg', 'image/jpg'];

      if(!allowedExtensions.includes(file.type)) {
        alert('Invalid file type. Only PNG, JPG and JPEG are allowed.');
        form.patchValue({ image: null });
        event.target.value = '';
        return;
      }

      form.patchValue({ image: file });
    }
  }

  goNextStep(form: FormGroup, stepper: any) {
    if (form.invalid) {
      form.markAllAsTouched();
      return;
    }
    stepper.next();
  }

  onSubmit() {
    this.errorMessage = '';

    let registrationData;

    if (this.role() === 'user') {
        if (this.userForm.invalid) {
            this.userForm.markAllAsTouched();
            return;
        }
        registrationData = { ...this.baseForm.value, ...this.userForm.value };
    } else if (this.role() === 'hotel') {
        if (this.hotelForm.invalid || this.hotelExtraForm.invalid) {
            this.hotelForm.markAllAsTouched();
            this.hotelExtraForm.markAllAsTouched();
            return;
        }

        if (!this.hotelExtraForm.value.latitude || !this.hotelExtraForm.value.longitude) {
            this.snackbarService.error('Please enter valid latitude and longitude.');
            return;
        }

        registrationData = { 
            ...this.baseForm.value, 
            ...this.hotelForm.value, 
            ...this.hotelExtraForm.value
        };
    }

    this.userService.register(registrationData).subscribe({
        next: (response: AuthResponse) => {
            this.snackbarService.success('Registration successful! Redirecting to login...');
            setTimeout(() => {
                this.router.navigate(['/users/login']);
            }, 1500);
        },
        error: (error) => {
            this.errorMessage = 'Registration failed. Please check your details and try again.';
            this.snackbarService.error(error.error?.message || 'Registration failed. Please try again.');
        }
    });
}

}
