# open-source-pos
A Point of Sale Application built with ASP.NET WEB API and Angular 13

Using  dependency injection (DI) software design pattern

And JWT authentication

It contains complete authentication workflow including Register screen, Email Verification, Login screen, Remember me feature, Account Locking for some minutes if the user tries to login multiple times with wrong password, User Profile, Change password, Forget password, User Log for all activity, Logout feature and automatic logout after specified time. Password expiration after specified time.

It Contains features of creating products/Items, and Point of sale Screen to enter invoices. 

# how to run (install) the project

Download and install visual studio 2019 or above  from “https://visualstudio.microsoft.com/downloads/”. During installation of Visual Studio Select the “ASP.NET and web development” workload.

Install the .Net Framework 4.5.2 developer pack. and
Install the .NET Core 2.1 Runtime

Download and install sql server express 2012 or above (2019 is recommended). Also install SQL Server Management Studio.


install node.js from “https://nodejs.org/en/download/”. Although it should work on the latest version but v16.14.0 is recommended “https://nodejs.org/dist/v16.14.0/node-v16.14.0-x64.msi” 

install angular cli by opening the command prompt and executing the following command
npm install -g @angular/cli@13.2.5

Fork and then Clone or Download the repo
https://github.com/Alishan-1/open-source-pos.git


Open the solution file “open-source-pos.sln” in visual studio. You will see the five projects in solution explorer
Models 
Open-source-pos ===>> Web Api project
OpenSourcePosDB ===>> Database Project
Repositories
Services

Right click on the “OpenSourcePosDB” project and click publish as shown in the following image. 

Publish database dialogue will be shown.
Enter/select the connection string for your server and write “OpenSourcePosDB” in the database name field and click the publish button. As shown in the following image.

The database named OpenSourcePosDB will be created on the server.

Now create a copy the of this file "Open-source-pos\open-source-pos\appsettings.default.json" in the same folder and rename the copy file to appsettings.json

And update the following connection strings in appsettings.json file with the newly created databases (if you need to)
ConnectionString
FNNConnectionString

Update the email configuration in appsettings.json file under the property named “Email”

Now launch the project by pressing F5 or Play button. The output of the default Api should now be seen in the browser as shown in the image below.

Now to run the frontend project open "open-source-pos\open-source-pos-frontend" folder in command prompt and Run the command 
“npm install” 
in the command prompt.

if your web api url is different than https://localhost:44333/api
you will need to change it in “src\app\app.constants.ts” file line no 46

than run the following command
ng serve –open

Project should open in the browser automatically. Register a new user and login with it to explore the application.
