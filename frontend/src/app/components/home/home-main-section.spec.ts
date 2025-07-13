import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeMainSection } from './home-main-section';

describe('HomeMainSection', () => {
  let component: HomeMainSection;
  let fixture: ComponentFixture<HomeMainSection>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeMainSection]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeMainSection);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
