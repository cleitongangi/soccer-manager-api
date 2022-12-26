# Project Demo - Soccer Manager API
## Cleiton Gangi
## Linkedin: https://www.linkedin.com/in/cleitongangi

### Disclaimer
This application is for study purposes only, it is not a production application.

## Instructions to run the project
### Softwares required
- Visual Studio 2022
- .Net 6
- Docker

### Run
Run through Visual Studio selecting Docker-compose.
or 
Execute the following command in powershell inside the project folder:
> docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" -p dockercompose-soccermanager-cg --ansi never up -d --force-recreate --remove-orphans
> Access the API using the URL: http://localhost:5194/swagger/index.html

If you need down the environment, you can use the following comand:
> docker-compose  -f "docker-compose.yml" -f "docker-compose.override.yml" -p dockercompose-soccermanager-cg down

Note: If you get error in compilation using docker, remove all bin and obj folders and try again.  

## Technologies implemented
 - .Net 6
 - Dapper
 - Entity Framework Core 6
 - SQL Server 2019 
 - AutoMapper
 - FluentValidation
 - Swagger
 - XUnit
 - Unit Test
 - Integration Test

## Architecture
 - Responsibility separation concerns, SOLID, YAGNI and Clean Code
 - Domain Driven Design (Layers and Domain Model Pattern)
 - Repository
 - IoC

## Documentation
Endpoints are accessible through swagger for easy testing and understanding.  
The names and parameters are pretty clear, follow a brief about each endpoint.

### Register a rew user
```
POST /api/Account/SignUp  
{  
    "username": "cleiton.gangi@gmail.com",  
    "password": "P@ssw0rd*"  
}
```  	
When a new user is registered, is generated a new team with a random name, a random country and a budget of 5,000,000. Is genereted a random team players with 3 Goalkeeper, 6 Defender, 6 Midfielder and 5 Attacker.  
#### Responses 
Status code 200 - User created successfully.  
Status code 400 - If the are any error messages. E.g., The username already exists.  

### Login
```
POST /api/Account/Login  
{  
    "username": "cleiton.gangi@gmail.com",  
    "password": "P@ssw0rd*"  
}
```  
Checks the username and password and returns a bearer token to allow access to secure endpoints.  
#### Responses
Status code 200 - Returns a bearer token to be used in the secure endpoints.  
```
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.yJzdWIiOiIxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImNsZWl0b25AZ21haWwuY29tIiwiaWF0IjoiMTIvMTUvMjAyMiAxNjo0MTo0OSIsImp0aSI6IjE1NDc1NTFkLTQ5Y2YtNGMyOS1iYTY3LTNmODJiZWQyZTkwNSIsImV4cCI6MTY3MTEyNjEwOSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTk0IiwiYXVkIjoiKiJ9.EUjkgBr09VEA3tmdEPyRpEMr5YPef3_olun9pvJFE6c",
    "expiration": "2022-12-15T17:41:49Z"
}
```	
Status code 400 - If the are any error messages. E.g., Username or Password invalid.  

### Logged user Team
```
GET /api/Teams
```
Allows get information about the team of authenticated user.  
#### Responses
Status code 200 - Returns the logged user team information.  
```
{
  "teamId": 1,
  "teamName": "Flamengo",
  "teamCountry": "Brazil",
  "teamValue": 20000000
}
```  
Status code 401 - Unauthorized - User is not authenticated.  

### Team
```
GET /api/Teams/{teamId}
```  
Allows get information about a specific team by teamId.  
#### Responses
Status code 200 - Returns team information.  
```
{
  "teamId": 1,
  "teamName": "Flamengo",
  "teamCountry": "Brazil",
  "teamValue": 20000000
}
```
Status code 404 - Team not found.  
Status code 401 - Unauthorized - User is not authenticated.   

### Update Team
```
PUT /api/Teams
{ 
	"teamName": "Team Name Test", 
	"teamCountry": "Brazil" 
}
```
Allows update team information of the logged in user.  
#### Response
Status code 200 - Team information updated successfully.  
Status code 400 - If there are any error messages.  
Status code 401 - Unauthorized - User is not authenticated.  

### Team Players
```
GET /api/Teams/{teamId}/Players
```
Allows get the players of a team.  
#### Responses
Status code 200 - Returns the team players.  
```
[
  {
    "id": 1,
    "firstName": "Pelé",
    "lastName": "Nascimento",
    "country": "Brazil",
    "age": 17,
    "marketValue": 50000000
  },
  {
    "id": 2,
    "firstName": "Neymar",
    "lastName": "Junior",
    "country": "Brazil",
    "age": 25,
    "marketValue": 20000000
  }
]
```
Status code 404 - Team not found.  

### Update a player
```
PUT /api/Teams/Players/{playerId}
{ 
	"firstName": "Neymar", 
	"lastName": "Junior", 
	"country": "Brazil" 
}
```
Allows update a player's informations.  
#### Responses
Status code 200 - Player information updated successfully.  
Status code 400 - If there are any error messages.  
Status code 401 - Unauthorized - User is not authenticated.  
Status code 404 - PlayerId not found.  

#### Add player to Transfer List 
```
POST /api/TransferList
{ 
	"PlayerId": 10, 
	"Price": 1500000 
}
```  
Allows put a player on the transfer list.  
#### Responses
Status code 201 - Player added in transfer list successfully.  
Status code 400 - If there are any error messages.  
Status code 401 - Unauthorized - User is not authenticated.  

### Get Transfer List
```
GET /api/TransferList?page=2
```
Allows view all transfer list.
#### Responses
Status code 200 - Got the transfer list successfully.  
```
{
  "results": [
    {
      "transferId": 11,
      "createdAt": "2022-12-26T18:58:48.901Z",
      "price": 1500000,
      "player": {
        "id": 1,
        "firstName": "Neymar",
        "lastName": "Junior",
        "country": "Brazil",
        "age": 25,
        "positionName": "Attacker"
      },
      "sourceTeam": {
        "teamId": 1,
        "teamName": "Flamengo",
        "teamCountry": "Brazil"
      }
    }
  ],
  "currentPage": 2,
  "pageSize": 10
}
```

### Get Transfer List of a team 
```
GET /api/Teams/{teamId}/TransferList?page2
```
Allows view a team transfer list by teamId.

#### Responses
Status code 200 - Got the transfer list successfully.  
```
{
  "results": [
    {
      "transferId": 11,
      "createdAt": "2022-12-26T18:58:48.901Z",
      "price": 1500000,
      "player": {
        "id": 1,
        "firstName": "Neymar",
        "lastName": "Junior",
        "country": "Brazil",
        "age": 25,
        "positionName": "Attacker"
      },
      "sourceTeam": {
        "teamId": 1,
        "teamName": "Flamengo",
        "teamCountry": "Brazil"
      }
    }
  ],
  "currentPage": 2,
  "pageSize": 10
}
```

### Get Transfer List item
```
GET /api/TransferList/11
```  
Allows get a transfer item (player).  
#### Responses
Status code 200 - Got the informaion successfully.  
```
{   
    "transferId": 11,  
    "createdAt": "2022-12-26T18:58:48.901Z",  
  "price": 1500000,  
  "player": {  
	"id": 1,  
	"firstName": "Neymar",  
	"lastName": "Junior",  
	"country": "Brazil",  
	"age": 25,  
	"positionName": "Attacker"  
  },  
  "sourceTeam": {  
	"teamId": 1,  
	"teamName": "Flamengo",  
	"teamCountry": "Brazil"  
  }
}
```
Status code 404 - If transfer list is not found.  

### DELETE /api/TransferList/{transferId}
```
DELETE /api/TransferList/5
```
Allows remove a player from a transfer list.  
#### Responses
Status code 200 - Player was removed from a transfer list successfully.  
Status code 400 - If there are any error messages.  
Status code 401 - Unauthorized - User is not authenticated.  

### Buy a player from a Transfer List
```
PUT /api/TransferList/{transferId}/Buy
```
Allows user to buy a player from a transfer list.
#### Responses
Status code 200 - The player has been successfully purchased.  
Status code 400 - If there are any error messages.  
Status code 401 - Unauthorized - User is not authenticated.  

### Database

#### Users
Stores the user informations. When a new user sign up, is registered in this table.

| Column Name | Type          | Nullable | PK |
| ----------- | ----          | -------- | -- |
| Id          | bigint        | no       | yes|
| Username    | nvarchar(255) | no       | no |
| Password    | varchar(100)  | no       | no |
| CreatedAt   | datetime      | no       | no |

#### Teams
Stores the team information.

| Column Name | Type          | Nullable | PK |
| ----------- | ----          | -------- | -- |
| TeamId      | bigint        | no       | yes|
| TeamName    | nvarchar(50)  | no       | no |
| TeamCountry | nvarchar(50)  | no       | no |
| Budget      | money         | no       | no |

#### TeamPlayers
Stores the team players informations.

| Column Name | Type          | Nullable | PK |
| ----------- | ----          | -------- | -- |
| TeamId      | bigint        | no       | yes|
| PlayerId    | bigint        | no       | yes|
| Sequence    | Int           | no       | yes|
| CreatedAt   | datetime      | no       | no |
| RemovedAt   | datetime      | no       | no |
| Active      | bit           | no       | no |

#### Players
Stores the players informations.

| Column Name | Type          | Nullable | PK |
| ----------- | ----          | -------- | -- |
| Id          | bigint        | no       | yes|
| FirstName   | nvarchar(50)  | no       | no |
| LastName    | nvarchar(50)  | no       | no |
| Country     | nvarchar(50)  | no       | no |
| Age         | int           | no       | no |
| MarketValue | money         | no       | no |
| PositionId  | smallint      | no       | no |
| CreatedAt   | datetime      | no       | no |
| UpdatedAt   | datetime      | no       | no |

#### PlayerPositions
Stores the positions (goalkeepers, defenders, etc).

| Column Name  | Type          | Nullable | PK |
| -----------  | ----          | -------- | -- |
| Id           | smallint      | no       | yes|
| PositionName | varchar(50)   | no       | no |

#### TransferList
Stores the transfer list informations.

| Column Name  | Type     | Nullable | PK |
| -----------  | ----     | -------- | -- |
| Id           | bigint   | no       | yes|
| PlayerId     | bigint   | no       | no |
| CreatedAt    | datetime | no       | no |
| SourceTeamId | bigint   | no       | no |
| Price        | money    | no       | no |
| UpdatedAt    | datetime | no       | no |
| TransferedAt | datetime | yes      | no |
| TargetTeamId | bigint   | yes      | no |
| StatusId     | smallint | no       | no |

#### TransferListStatus
Stores the informations about the transfer status (Open, Transferred and Canceled).

| Column Name | Type          | Nullable | PK |
| ----------- | ----          | -------- | -- |
| Id          | smallint      | no       | yes|
| StatusName  | varchar(11)   | no       | no |
