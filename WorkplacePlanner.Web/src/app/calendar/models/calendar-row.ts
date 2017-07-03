import { Person } from '../../teams/models/person'
import { CalendarEntry } from './calendar-entry'

export class CalendarRow {
    person: Person;
    calendarEntries: CalendarEntry[];
    hasPermissionToEdit: boolean;
    membershipId: number;
}
