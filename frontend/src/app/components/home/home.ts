import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeMainSectionComponent } from './home-main-section';
import { InstitutionService, InstitutionConfig } from '../../services/institution.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, HomeMainSectionComponent],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class HomeComponent {
  institution: InstitutionConfig;

  constructor(private institutionService: InstitutionService) {
    this.institution = this.institutionService.getInstitution();
  }
}
