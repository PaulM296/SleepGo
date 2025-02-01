import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../core/services/user.service';
import { AuthResponse } from '../../../shared/models/authResponse';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private formBuilder = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);
  private snackbarService = inject(SnackbarService);
  errorMessage: string = '';
  showPassword: boolean = false;

  loginForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(10)]]
  });

  get form() {
    return this.loginForm.controls;
  }

  onSubmit() {
    if(this.loginForm.invalid)
      return;

    const { email, password } = this.loginForm.value;

    this.userService.login(email, password).subscribe({
      next: (response: AuthResponse) => {
        localStorage.setItem('token', response.token);
        this.router.navigate(['']);
        this.snackbarService.success('Login successful.');
      },
      error: () => {
        this.errorMessage = 'Invalid email or password!';
        this.snackbarService.error('Login failed. Please try again.');
      }
    });
  }

  goToRegister() : void {
    this.router.navigate(['/users/register']);
  }

  togglePasswordVisibility() : void {
    this.showPassword = !this.showPassword;
  }
}
