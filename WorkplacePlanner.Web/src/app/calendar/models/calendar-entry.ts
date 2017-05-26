import { Person } from '../../teams/models/person'
import { DeskUsageEntry } from './desk-usage-entry'

export class CalendarEntry {
    person: Person;
    deskUsages: DeskUsageEntry[];
}
