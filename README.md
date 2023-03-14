# Testaufgabe: Contact Manager WebAPI

## Architecture

WebAPI: ASP.NET Core 7.0

Database: PostgreSQL with Entity FW Core

Containerization: Docker with Docker Compose


## Procedure

1. Create new project with dotnet CLI: ```dotnet new webapi -n ContactManagerWebApi```.
2. With the dotnet CLI commmand ```dotnet add package <package_name>``` install the following packages:
   - Npgsql.EntityFrameworkCore.Postgresql
   - Microsoft.EntityFrameworkCore
   - Microsoft.EntityFrameworkCore.Tools
   - (Swashbuckle for Swagger support comes preinstalled with step 1)
3. Create folders Models and Data.
4. Delete the WeatherForecast templates.
5. Inside Models create the Contact model that we will use.
6. Inside the Data folder create:
   - IContactsRepository.cs: the repo interface that defines the functions to be used for routes for contacts
   - ContactsRepository.cs: the repository implementing the interfae and containing the actual CRUD method behaviours
   - ContactsDbSeeder.cd: the contact data seeder populating the database with initial data
   - ContactsDbContext.cs: the context that handling the database session and is used to build the migrations
7. Inside the Controllers folder add file ContactsController.cs defining the API endpoints.
8. Do initial migration using the dotnet-cli tool for EF Core: ```dotnet-ef migrations add "initial-migration-with-trigger"```.
9. In the newly created Migrations folder open the ```<dateandtime>_initial-migration-with-trigger.cs``` file and add raw PostgreSQL for:
   - ```FUNCTION func_update_last_modified_timestamp()```
   - ```TRIGGER trg_update_last_modified_timestamp```
10. Split Program into Program.cs and Startup.cs and configure Program.cs to initialize the host and Startup.cs to configure the services.
11. Write the Dockerfile which defines the docker container for the WebAPI.
12. Write the docker-compose file to set up the WebAPI and the PostgreSQL-Database to run at the same time.
13. Finally run the command ```docker-compose up``` and visit http://localhost/swagger for the documentation and testing.


## Notes
- No need for git since that is just not necessary for a project of that size.
- NotifyHasBirthdaySoon is just a logical piece that is gained from two other data therefore it is not in the database.
- I willfully ignored implementing the database persistance with docker-compose, because there is not much of a reason.
- I added a field Id because (almost) every good database table has one or wont suffer from a identity column.
- As per Microsofts own docs: "data annotations will override conventions, but will be overriden by FluentAPI configuration", therefore I can mix and only what is not achievable in Data Annotations I will do in Fluent API.
- I am aware that I can rename the columns in the PostgreSQL database, but for simplicity's sake I won't do it.
