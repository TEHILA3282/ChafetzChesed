import { Component } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-rejected-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './rejected-dialog.html',
  styleUrls: ['./rejected-dialog.scss']
}

)
export class RejectedDialogComponent {
  onClose() {
  console.log('Dialog close clicked');
}
}
