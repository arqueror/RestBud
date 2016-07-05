# RestBud
RestBud is a simple C# library built on the top of HttpClient that provides you with generic asynchronous/synchronous methods to: CREATE,PUT,POST,DELETE operations
when communicating with ASP.NET Web Api 2

# Referencing 
Once you download the solution, please compile the library and reference it from your project

# Configuration

Once the library is referenced in your project:

          //1.-Initialize client
            var myClient=new RestBudClient();
            
          //2.-Configure client with deault values if applies with baseUrl,user,password and api version if applies
            myClient.SetRestBudConfig("http://www.myapi.com/api/","user","password",1);
            
          //3.-Before making a request to the API configure request with controller,action and apiToken if needed
            myClient.SetRequestParameters("controller","action",new BearerModel() {AccessToken="apiTOken"});
            
           //4.- Perform a POST,CREATE,CREATE,DELETE
            myClient.Post<expectedObjectType>(objectToSend);

           //If your api needs AUTHENTICATION you can get an apiToken with this method
            var mytoken =myClient.GetBearerToken<expectedObjectType>("http://www.api.com/api/","getToken", "user", "password");
