import { Component } from '@angular/core';
import { HomeMainSectionComponent } from './home-main-section';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HomeMainSectionComponent],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']

})
export class HomeComponent { }