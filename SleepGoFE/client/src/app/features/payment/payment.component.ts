import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from "../../layout/header/header.component";
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [
    CommonModule,
    HeaderComponent,
    MatCardModule,
    MatButtonModule
],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.scss'
})
export class PaymentComponent implements OnInit {
  private stripeService = inject(StripeService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  @ViewChild('paymentElement') paymentElementRef!: ElementRef;

  reservationId!: string;
  isLoading = false;
  error: string | null = null;

  ngOnInit(): void {
    this.loadPaymentElement();
  }

  private async loadPaymentElement() {
    this.reservationId = this.route.snapshot.paramMap.get('reservationId')!;
    const paymentElement = await this.stripeService.createPaymentElement(this.reservationId);
    paymentElement.mount(this.paymentElementRef.nativeElement);
  }

  async onPay() {
    this.isLoading = true;
    this.error = null;
    
    try {
      const result = await this.stripeService.confirmPayment(this.reservationId);
      
      if (result.error) {
        this.error = result.error.message || 'Payment failed!';
        this.isLoading = false;
      } else {
        this.router.navigate(['/user-reservations']);
      }
    } catch (e: any) {
      this.error = e.message || 'Something went wrong!';
      this.isLoading = false;
    }
  }
}
