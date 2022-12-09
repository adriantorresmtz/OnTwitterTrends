import { APP_INITIALIZER,NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { TwitterHubService } from '../app/services/twitter-hub.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [
    TwitterHubService,
    {
      provide: APP_INITIALIZER,
      useFactory: (signalrService: TwitterHubService) => () => signalrService.initiateSignalrConnection(),
      deps: [TwitterHubService],
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
