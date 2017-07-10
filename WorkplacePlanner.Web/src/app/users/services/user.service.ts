import { Injectable } from "@angular/core";
import { UserL } from '../models/user-l';
import { User } from '../models/user';
import { DataService } from '../../shared/services/data.service'; 

@Injectable()
export class UserService{

    constructor(private dataService: DataService ) {}

    public GetAll() : Promise<User[]> {
        return this.dataService.get("users")
        .toPromise()
        .then(res => res.json() as User[]);
    }

    public GetAllWithTeam() : Promise<UserL[]> {
        return this.dataService.get("users", "withteam")
        .toPromise()
        .then(res => res.json() as UserL[]);
    }

    public getUser(id: number): Promise<UserL> {
        return this.dataService.get("users", "", [id.toString()])
        .toPromise()
        .then(res => res.json() as UserL);
    }


    public createUser(user: User): Promise<number> {
        return this.dataService.create("users", "", user)
        .toPromise()
        .then(res => res.json() as number);
        }

}


