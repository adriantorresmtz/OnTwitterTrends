# OnTwitterTrends
This app display information in real time about hashtags in twitter

![image](https://user-images.githubusercontent.com/21364401/206961323-bb82b4b1-cebc-41ca-adc0-0675e61bb0c8.png)



## How to test

# Published

Step 1 -
     Start twitter collection service
     
     On postman call with Get verb this url ->
     
            https://ontwitter.azurewebsites.net/api/twitter/v1/Start
   
Step 2 -
     Open browser url -> 
                
                 https://adriantorresmtz.github.io/OnTwitterTrends/

![image](https://user-images.githubusercontent.com/21364401/207730419-7530ed5d-0031-4f4f-ba70-ecd00319ceae.png)

# Stop twitter data collection service

Step 1-
 On postman call with Get verb this url ->
     
            https://ontwitter.azurewebsites.net/api/twitter/v1/Stop

# Local Enviroment

Step 1 -  
      Run Server project

Step 2 - 
      On Swagger call andpoint ->   api/twitter/v1/start 
     
Step 3 -
      Run Client project

Step 4 -
      Open browser with ->  http://localhost:2301
      
## Stop Data collection

Step 1 -
     On Swagger call andpoint ->   api/twitter/v1/stop 
     
## Start Data collection

Step 1 -
     On Swagger call andpoint ->   api/twitter/v1/start 
     
![image](https://user-images.githubusercontent.com/21364401/207731188-776708e6-5060-4b37-b109-5d10af61d46b.png)

