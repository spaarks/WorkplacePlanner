import { User } from '../../users/models/user'
import { CalendarEntry } from './calendar-entry'

export class CalendarRow {
    user: User;
    calendarEntries: CalendarEntry[];
    hasPermissionToEdit: boolean;
    membershipId: number;
}
