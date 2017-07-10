import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { MessageService } from '../../core/services/message.service';

@Component({
  selector: 'ft-add-user',
   templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {

  user: User;

  @Output() onUpdated: EventEmitter<number>;
  @Input() userId: number = 0;

  constructor(private userService: UserService, private messageService: MessageService) {
    this.onUpdated = new EventEmitter<number>();
    console.log('Add person Create');
    
  }

  ngOnInit() {
    console.log('Add person On Init');
    if (this.userId <= 0) {
      this.clearForm();
    }
  }

  saveUser() {
    this.userService.createUser(this.user)
      .then(id => {
        this.messageService.showSuccess(this.userId > 0 ? 'User updated' : 'User created');
        this.onUpdated.emit(id);
        this.clearForm();
      });
  }

  private clearForm() {
    this.user = new User();
    this.user.active = true;
  }
}
