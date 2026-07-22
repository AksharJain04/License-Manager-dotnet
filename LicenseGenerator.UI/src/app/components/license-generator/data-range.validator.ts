import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const dateRangeValidator: ValidatorFn =
  (control: AbstractControl): ValidationErrors | null => {

    const activation = control.get('activationDate')?.value;
    const expiration = control.get('expirationDate')?.value;

    // Don't validate until both dates are entered
    if (!activation || !expiration) return null;

    const activationDate = new Date(activation);
    const expirationDate = new Date(expiration);

    return new Date(expiration) >= new Date(activation)? null : { invalidDateRange: true }; 

};