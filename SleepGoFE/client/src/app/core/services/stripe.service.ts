import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { loadStripe, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElements, StripePaymentElement } from '@stripe/stripe-js'; 
import { environment } from '../../../environments/environment';
import { firstValueFrom } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class StripeService {
  private stripePromise: Promise<Stripe | null>;
  private elements?: StripeElements;
  private paymentElement?: StripePaymentElement;
  private http = inject(HttpClient);

  constructor() { 
    this.stripePromise = loadStripe(environment.stripePublicKey);
  }

  getStripeInstance() {
    return this.stripePromise;
  }

  async initializeElements(reservationId: string) {
    if(!this.elements) {
      const stripe = await this.getStripeInstance();
      if(stripe) {
        const response = await firstValueFrom(
          this.http.post<{ clientSecret: string }> (
            `${ environment.apiUrl }payments/create-payment-intent/${reservationId}`,
            {}
          )
        );

        if(!response) throw new Error('Stripe clientSecret missing!');
        const { clientSecret } = response;

        this.elements = stripe.elements({
          clientSecret,
          appearance: { labels: 'floating' }
        });
      } else {
        throw new Error('Stripe failed to load!');
      }
    }

    return this.elements;
  }

  async createPaymentElement(reservationId: string) {
    if(!this.paymentElement) {
      const elements = await this.initializeElements(reservationId);

      if (elements) {
        this.paymentElement = elements.create('payment');
      } else {
        throw new Error('Elements instance has not been initialized');
      }
    }

    return this.paymentElement;
  }

  async confirmPayment(reservationId: string) {
    const stripe = await this.getStripeInstance();
    const elements = await this.initializeElements(reservationId);
    const result = await elements.submit();

    if(result.error) {
      throw new Error(result.error.message);
    }

    const { clientSecret } = await firstValueFrom(
      this.http.post<{ clientSecret: string }>(
        `${environment.apiUrl}payments/create-payment-intent/${reservationId}`,
        {}
      )
    );

    if(stripe && clientSecret) {
      return await stripe.confirmPayment({
        clientSecret,
        elements,
        confirmParams: {
          return_url: `${window.location.origin}/user-reservations`
        }
      });
    } else {
      throw new Error('Stripe not available or missing client secret.');
    }
  }

  disposeElements() {
    this.elements = undefined;
    this.paymentElement = undefined;
  }
  
}
