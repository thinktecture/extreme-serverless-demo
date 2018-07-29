import {AfterViewInit, Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, NgForm, Validators} from '@angular/forms';
import {MatDialog, MatDialogRef, MatList, MatListItem} from '@angular/material';
import {Observable} from 'rxjs/internal/Observable';
import {of} from 'rxjs/internal/observable/of';
import {filter, flatMap} from 'rxjs/operators';
import {DialogUserComponent} from './dialog-user/dialog-user.component';
import {Message} from './shared/model/message';
import {User} from './shared/model/user';
import {ChatService} from './shared/services/chatService';

const AVATAR_URL = 'https://api.adorable.io/avatars/285';

@Component({
  selector: 'tcc-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit, AfterViewInit {
  user: User;
  messages$: Observable<Message[]>;
  dialogRef: MatDialogRef<DialogUserComponent> | null;
  defaultDialogUserParams: any = { disableClose: true };

  formGroup: FormGroup;
  @ViewChild('messageForm') messageForm: NgForm;

  @ViewChild(MatList, { read: ElementRef }) matList: ElementRef;

  @ViewChildren(MatListItem, { read: ElementRef }) matListItems: QueryList<MatListItem>;

  private _messages: Message[] = [];

  constructor(
    private _chatService: ChatService,
    public dialog: MatDialog,
    private _formBuilder: FormBuilder,
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.initModel();
    // Using timeout due to https://github.com/angular/angular/issues/14748
    Promise.resolve()
      .then(() => this.openUserPopup(this.defaultDialogUserParams));

    this._chatService.init();

    this.messages$ = this._chatService.messages$
      .pipe(
        flatMap(messagesFromServer => {
          this._messages = this._messages.concat(messagesFromServer);
          return of(this._messages);
        }),
      );
  }

  ngAfterViewInit(): void {
    this.matListItems.changes.subscribe(() => {
      this.scrollToBottom();
    });
  }

  // auto-scroll fix: inspired by this stack overflow post

  public sendMessage(): void {
    this._chatService.send({
      user: this.user,
      message: this.formGroup.value.messageContent,
    }).subscribe();

    this.messageForm.resetForm();
  }

  // https://stackoverflow.com/questions/35232731/angular2-scroll-to-bottom-chat-style
  private scrollToBottom(): void {
    try {
      this.matList.nativeElement.scrollTop = this.matList.nativeElement.scrollHeight;
    } catch (err) {
    }
  }

  private initModel(): void {
    const randomId = this.getRandomId();
    this.user = {
      id: randomId,
      avatar: `${AVATAR_URL}/${randomId}.png`,
    } as User;
  }

  private getRandomId(): number {
    return Math.floor(Math.random() * (1000000)) + 1;
  }

  private openUserPopup(params): void {
    this.dialogRef = this.dialog.open(DialogUserComponent, params);
    this.dialogRef.afterClosed()
      .pipe(filter(paramsDialog => paramsDialog))
      .subscribe(paramsDialog => this.user.name = paramsDialog.username);
  }

  private createForm() {
    this.formGroup = this._formBuilder.group({ messageContent: ['', Validators.required] });
  }
}
