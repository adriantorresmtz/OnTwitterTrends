import { Component, OnInit } from '@angular/core';
import { TwitterHubService } from '../app/services/twitter-hub.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  hubTwitterTopMessage: any;
  hashtagsData: Array<any> = [];
  hashtagsTopCount: number;
  TwitterHastTagsTotal: number;
  TwitterTotal: number;

  constructor(public signalrService: TwitterHubService) {
    this.hubTwitterTopMessage = [];
    this.TwitterHastTagsTotal = 0;
    this.TwitterTotal = 0;
    this.hashtagsTopCount = 0;
  }

  ngOnInit(): void {
  
     //Subscribe to get get top of hashtags
     this.signalrService.hubTwitterTop.subscribe((resp) => {
      this.hashtagsData = resp.data?.twitterHashTagTops || []; 
      this.hashtagsTopCount = this.hashtagsData.length;
      this.TwitterHastTagsTotal =  resp.data.totalTwitterHashTagsTotal;  
    });

    //Subscribe to get Total twitters
    this.signalrService.hubTwitterTotal.subscribe((resp) => {
      console.log(resp);
      this.TwitterTotal = resp.data;    
    });

  }
}


