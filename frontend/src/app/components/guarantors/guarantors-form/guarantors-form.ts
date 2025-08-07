import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';


@Component({
  selector: 'app-guarantors-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './guarantors-form.html',
  styleUrls: ['./guarantors-form.scss']
})
export class GuarantorsFormComponent {
  form!: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      guarantors: this.fb.array([
        this.createGuarantor(),
        this.createGuarantor(),
        this.createGuarantor()
      ])
    });
  }

  // get guarantors(): FormArray {
  //   return this.form.get('guarantors') as FormArray;
  // }
  get guarantorFormGroups(): FormGroup[] {
  return (this.form.get('guarantors') as FormArray).controls as FormGroup[];
}


  private createGuarantor(): FormGroup {
    return this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      idNumber: ['', Validators.required],
      occupation: ['', Validators.required],
      city: ['', Validators.required],
      street: ['', Validators.required],
      houseNumber: ['', Validators.required],
      loanLink: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      console.log('Form Submitted:', this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }
}
