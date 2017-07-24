import { Injectable } from "@angular/core";
import { UserL } from '../models/user-l';
import { User } from '../models/user';
import { DataService } from '../../shared/services/data.service';
import { LoginData } from '../models/login-data';
import { AuthToken } from '../models/auth-token';


@Injectable()
export class UserService {

    constructor(private dataService: DataService) { }

    public GetAll(): Promise<User[]> {
        return this.dataService.get("users")
            .toPromise()
            .then(res => res.json() as User[]);
    }

    public GetAllWithTeam(): Promise<UserL[]> {
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
        return this.dataService.post("account", "register", user)
            .toPromise()
            .then((res) => { console.log(res); return res.json() as number; });
    }

    public login(loginData: LoginData) {
        return this.dataService.post("account", "login2", loginData)
            .toPromise()
            .then((res) => res.json() as AuthToken)
            .then((token) => {
                if (loginData.rememberMe) {
                    localStorage.setItem("authToken", token.token);
                } else {
                    sessionStorage.setItem("authToken", token.token);
                }
            });
    }

    // return this.dataService.create("users", "", user)
    // .toPromise()
    // .then(res => res.json() as number);
    // }

}


