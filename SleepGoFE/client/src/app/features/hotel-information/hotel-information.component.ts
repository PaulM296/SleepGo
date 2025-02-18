import { Component } from '@angular/core';
import { HeaderComponent } from '../../layout/header/header.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-hotel-information',
  standalone: true,
  imports: [
    HeaderComponent,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    
  ],
  templateUrl: './hotel-information.component.html',
  styleUrl: './hotel-information.component.scss'
})
export class HotelInformationComponent {

}
