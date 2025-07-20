import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeMainSectionComponent } from './home-main-section';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, HomeMainSectionComponent],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class HomeComponent { }