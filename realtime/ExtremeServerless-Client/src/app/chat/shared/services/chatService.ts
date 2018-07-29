import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import * as signalR from '@aspnet/signalr';
import {HubConnection, IHttpConnectionOptions} from '@aspnet/signalr';
import {environment} from 'environments/environment';
import {Observable, Subject} from 'rxjs';
import {ConnectionConfig} from '../model/connectionConfig';
import {Message} from '../model/message';

@Injectable()
export class ChatService {
  public messages$: Observable<Message[]>;

  private _hubConnection: HubConnection;
  private _messagesSubject: Subject<Message[]> = new Subject();

  constructor(private _http: HttpClient) {
    this.messages$ = this._messagesSubject.asObservable();
  }

  public init() {
    console.log(`Initializing ChatService...`);

    this.getConnectionInfo().subscribe(config => {
      console.log(`Received info for endpoint ${config.hubUrl}`);

      const options: IHttpConnectionOptions = {
        accessTokenFactory: () => config.accessToken,
      };

      this._hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(config.hubUrl, options)
        .configureLogging(signalR.LogLevel.Information)
        .build();

      this._hubConnection.start().catch(err => console.error(err.toString()));
      // TODO: only start when no error from start()...
      this._hubConnection.on('NewMessages', (data: any) => {
        // Seems we need to do this with the early stage of SignalR...
        const message = JSON.parse(data);
        this._messagesSubject.next(message);
      });
    });
  }

  public send(message: Message) {
    const requestUrl = `${environment.apiBaseUrl}save`;

    return this._http.post(requestUrl, message);
  }

  private getConnectionInfo(): Observable<ConnectionConfig> {
    const requestUrl = `${environment.apiBaseUrl}config`;

    return this._http.get<ConnectionConfig>(requestUrl);
  }
}
