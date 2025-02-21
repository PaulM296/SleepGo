import { Component } from '@angular/core';
import { HeaderComponent } from "../../../layout/header/header.component";

@Component({
  selector: 'app-hotel-reservations',
  standalone: true,
  imports: [HeaderComponent],
  templateUrl: './hotel-reservations.component.html',
  styleUrl: './hotel-reservations.component.scss'
})
export class HotelReservationsComponent {

}
