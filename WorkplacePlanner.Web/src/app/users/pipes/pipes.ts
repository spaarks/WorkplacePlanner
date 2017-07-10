import { Pipe, PipeTransform } from '@angular/core'
import { UserL } from '../models/user-l'

@Pipe({
    name: 'ftFilterUsers',
    pure: false
})

export class FilterUsersPipe implements PipeTransform {
    transform(users: any[], term) {
        term = term.toLowerCase();
        return term
            ? users.filter(u => u.firstName.toLowerCase().indexOf(term) !== -1 || u.lastName.toLowerCase().indexOf(term) !== -1)
            : users;
    }
}
