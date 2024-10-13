import {
  Component,
  OnInit,
  ViewChild,
  inject,
  input,
  output,
} from '@angular/core';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  private messageService = inject(MessageService);
  username = input.required<string>();
  messages = input.required<Message[]>();
  updateMessages = output<Message>();
  messageContent = '';


  ngOnInit(): void {
 
  }


}
