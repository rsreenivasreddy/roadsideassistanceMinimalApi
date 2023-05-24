# RoadsideAssitance.API 
     
      A complete working implemntation of Roadside Assitance Services based Geo Locations 
      with below Assumptions and Compromises. 

## Tech Stack
   - .Net 7 (core)
   - MINIMAL APIs ,  Followed latest Microsoft .Net 7 (Core) Minimal API approach, where there is no Controllers classes.
                     It is similar to regular Web API and gives better peformance.Well suites for Microservices Development.
   - EFCore 7     
   - SQL Server  ,  Storage 
   - XUnit             

 
## Key Components Implemented
  - HealthChecks endpoints ,  Followed and exposed microsoft recommend health endpoint , but can be extended much based on needs.
  - Enable NullableErrors in Code,  This helps for better coding and especially reduce at 
                                    greaterpoint of ObjectReferencNotfound errors in Production which are very expensive and time consuming.
  - Serilog
  - ProblemDetails ,  Standardize Error Response Pattern
  - DDD      , Followed Domain Driven Design approach , can be further enhanced with eventing mechanism.
  - DI
  - FluentValidations  
   

## Validations

   - Geo Location Request,  
             - Latitude and Longitude Validation for Viginia State. Accetable Range Latitude (36.5427 to 39.4659)  and Longitude (-83.6753 to -74.9707) 
             - Currently focused on just Virginia State locations. You can find more details in code with comments in GeoLocationRequestValidator.cs
   - Limit Query parameter,
             - FindNearest Service Assistant limit query parameter.  Accetable Range (1 to 50).

## ASSUMPTIONS

   - Geo Location Table 
              - Will have all Supported City Locations data , Considering current scope/time , attached sql insert scripts for  Virginia State All Cities GeoLocations.
              - but design can be improved further based scope & business need.
              - Sql Insert Script file  '\deployment\SqlScripts\GeoLocations_Data_Insert_Scripts_VirginiaState.sql'
   - Customer Table 
              - Will have just one vehicle for now to limit the scope and should allow to reserve one Assistant at a time.
              - Sql Insert Script for Customer Test Data available in '\deployment\SqlScripts\Customer_Test_Data.sql' 
              - Customer will always request for an Assistance with specific City Geo Location where the company supported/offered services city Geo Locations.
              - Validations are in place to restrict the input and allow valid locations. For now Just Virginia State All Cities Locations.
   - Assistant 
             - Asssitant Entity is treated as Transaction entity with limited details and to track the resevation.
             - Sql Insert Script for Assistant Test Data available in '\deployment\SqlScripts\Assistant_Test_Data.sql'  
             - Every time Assistant provide a service, Assistant Current Location required to update with Service Location.
   - Address
             - Address is treaded as field instead of enity. but can be seperated with state/country for effeciency filtering.
  
   - Request/Response Models
             - Seperating Request/Response Models from Domian Enities will gives better controll for changes/versioning, have sepearated them in the code.

## COMPROMISES

   - NEAREST Distance forumla  , 
             - Have used Sql Query to figure out shortest distance to make it complete working solution, it can be done within 
             - C# code using SortedSet with IComparer<Assistant> with distance or any other alogorithm.
   - RoadsideAssistanceService , 
             - Intention is to minimize changes of RoadsideAssistanceService structure and  have introduced another Service CustomerAssistantService
                 for doing some additional logic prior calling RoadsideAssistanceService which can be combined or split it based on behavior of service
             - Have to compromise on return type of FindNearestAssistants instead of SortedSet to IEnumerable to make it complete working solution 
                  and avoided sorting again as sort by distance logic is taken care by Sql Query.
   
   - Concurrent/Parallel Assitant Reseving of same Assistant is hadled with EF ConcurrencyToken but it can be enhanced in a better way if it require.
   
   
# RoadsideAssitance Code Setup and Testing

  Micro-service for prodiving Assistance to the customers when there is a Roadside Emergency needed 


## Getting started

- Clone the repository
- Open powershell at the repository root
- Run the application `dotnet run` 
  
  By default, the appsettings will be pointed to local database server , change the copnnection string "DataContext":
   
## Managing Packages

 To update tools, open powershell at the repository root and run the following two commands:

```pwsh
dotnet tool update dotnet-ef
dotnet tool update swashbuckle.aspnetcore.cli
```


### Generate new migration

- Open powershell to the repository root
- Ensure your dotnet tools are restored with:

```pwsh 
dotnet tool restore --interactive
```

- To Create Database use below command from Powershell/DevCommand CLI or else use RoadsideAssistance.Api.Database script file to setup Database in Sql Server.

```pwsh
dotnet ef database update `
  --project .\src\RoadsideAssistance.Api\RoadsideAssistance.Api.csproj
```
## Testing 
 
 Use below Swagger Url or Postman for testing
     https://localhost:7214/swagger/index.html

  - To Test end to end functionality Run below test data scripts for inserting data before moving to endpoints testing
      - GeoLocations Table Test Data. Run `.\deployment\SqlScripts\GeoLocations_Data_Insert_Scripts_VirginiaState.sql` - This will setup All Cities Geo Locations of Virginia State  
                       - Example GeoLocation Request  
                           `{
                                "latitude": 36.589352,
                                "longitude": -79.020237
                            }
                           `
      - Customers Table Test Data. Run `.\deployment\SqlScripts\Customer_Test_Data.sql` , This will setup 10 Customers, we can use any Id from 1 - 10 to test
                    - Example Customer Id : 1
      - Assistants Table Test Data . Run  `.\deployment\SqlScripts\Assistant_Test_Data.sql`  , This will setup 31 Assistants, we can use any Id from 1 - 31 to test
                    - Example Assistant Id : 24
                 
  - Endpoints To Test
       1.  Update Assistant Location :   
                         This endpoint will Updates Assistant's Current Location with location in the request body.
                      
                         Request Url : https://localhost:7214/v1/Assistants/2/Location
                         HttpVerb: PUT
                         RequestBody : {
                                           "latitude": 37.122162,
                                           "longitude": -79.291145
                                        }
                         Curl : 
                                ```pwsh
                                          curl -X 'PUT' \
                                          'https://localhost:7214/v1/Assistants/2/Location' \
                                          -H 'accept: */*' \
                                          -H 'Content-Type: application/json' \
                                          -d '{
                                                 "latitude": 37.122162,
                                                 "longitude": -79.291145
                                              }'
   
                                   ```

       2.  Find Nearest Assistant :
                        This endpoint will Find nearest available Assistantants ( with limit) for given location who are not reserved for any customers.
                    
                         Request Url : https://localhost:7214/v1/Assistants/FindNearest?limit=5
                         HttpVerb: POST
                         RequestBody : {
                                         "latitude": 37.962624,
                                         "longitude": -78.841046
                                       }
                         Curl : 
                                ```pwsh
                                      curl -X 'POST' \
                                           'https://localhost:7214/v1/Assistants/FindNearest?limit=5' \
                                           -H 'accept: application/json' \
                                           -H 'Content-Type: application/json' \
                                           -d '{
                                                 "latitude": 37.962624,
                                                 "longitude": -78.841046
                                               }'
   
                                ```
      3. Reserve an Assistant :
                      This endpoint will Reserve nearest Available Assistant for a given customer for the requested location. 
                      Once Assistant is reserved he can't available for any customer until released.

                         Request Url : https://localhost:7214/v1/Customers/5/ReserveAssistant
                         HttpVerb    : POST
                         RequestBody : {
                                         "latitude": 37.150833,
                                         "longitude":-77.744444
                                       }
                         Curl : 
                                ``` pwsh
                                       curl -X 'POST' \
                                          'https://localhost:7214/v1/Customers/5/ReserveAssistant' \
                                          -H 'accept: application/json' \
                                          -H 'Content-Type: application/json' \
                                          -d '{
                                                "latitude": 37.150833,
                                                "longitude":-77.744444
                                              }'
   
                                ```
       4. Release an Assistant :
        
                      This endpoint is to Release  Given Reserved Assistant for a given customer. 
                      If Assistance is not reserved for this customer , system will not release.

                         Request Url : https://localhost:7214/v1/Customers/5/Assistants/26/Release
                         HttpVerb    : PUT
                         Curl : 
                                ``` pwsh
                                      curl -X 'PUT' \
                                     'https://localhost:7214/v1/Customers/5/Assistants/26/Release' \
                                     -H 'accept: */*'
   
                                ```
                       
