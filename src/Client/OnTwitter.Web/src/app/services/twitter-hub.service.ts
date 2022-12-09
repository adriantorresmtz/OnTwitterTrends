import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TwitterHubService {
  baseURL: string;
  hubUrl: string;
  connection: any;
  hubTwitterTop: BehaviorSubject<any>;
  hubTwitterTotal: BehaviorSubject<any>;

  constructor() {
    this.baseURL = environment.ApiURL;
    this.hubUrl = this.baseURL +"/twitterdatahub";
    this.hubTwitterTop = new BehaviorSubject<any>([]);
    this.hubTwitterTotal = new BehaviorSubject<any>([]);
  }

  public async initiateSignalrConnection(): Promise<void> {
    try {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(this.hubUrl)
        .withAutomaticReconnect()
        .build();

      await this.connection.start();
      this.setSignalrClientMethods();

      console.log(`TwitterHub connection success! connectionId: ${this.connection.connectionId}`);
    }
    catch (error) {
      console.log(`TwitterHub connection error: ${error}`);
    }
  }

  private setSignalrClientMethods(): void {

    this.connection.on('TwitterTop', (data: any) => {
      this.hubTwitterTop.next(data);
    });

    this.connection.on('TwitterTotal', (data: any) => {
      this.hubTwitterTotal.next(data);
    });
    
  }
}
