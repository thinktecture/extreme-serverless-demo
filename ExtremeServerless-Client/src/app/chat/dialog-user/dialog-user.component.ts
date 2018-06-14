import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MatDialogRef} from '@angular/material';

@Component({
  selector: 'tcc-dialog-user',
  templateUrl: './dialog-user.component.html',
  styleUrls: ['./dialog-user.component.css'],
})
export class DialogUserComponent {
  public formGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<DialogUserComponent>,
    private _formBuilder: FormBuilder,
  ) {
    this.createForm();
  }

  public onSave(): void {
    if (this.formGroup.invalid) {
      return;
    }

    this.dialogRef.close({ username: this.formGroup.value.username });
  }

  private createForm() {
    this.formGroup = this._formBuilder.group({
      username: ['', Validators.required],
    });
  }
}
