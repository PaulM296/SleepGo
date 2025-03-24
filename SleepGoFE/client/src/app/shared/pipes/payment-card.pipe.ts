import { Pipe, PipeTransform } from '@angular/core';
import { PaymentSummary } from '../models/paymentModels/paymentSummary';

@Pipe({
  name: 'paymentCard',
  standalone: true
})
export class PaymentCardPipe implements PipeTransform {

  transform(value?: PaymentSummary): string {
    if(value) {
      const { brand, last4, expMonth, expYear } = value;
      return `${brand.toUpperCase()}, **** **** **** ${last4}, Exp: ${expMonth}/${expYear}`;
    }

    return 'Unknown payment method';
  }

}
