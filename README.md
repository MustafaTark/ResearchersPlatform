# ResWeb - Researcher Collaboration Platform

ResWeb is a cutting-edge platform designed to empower researchers by providing access to a diverse array of courses, facilitating seamless collaboration, and fostering growth within the research community. This repository contains the backend code for ResWeb, developed using .NET Core Web API.

# Website
The ResWeb is hosted at https://resweb.net/. Please visit the website to explore the platform's features and functionalities.

# Features
- Courses: Researchers can access a comprehensive selection of courses to enhance their skills and knowledge.
- Team Collaboration: Researchers can create and manage their teams, enabling efficient communication and file-sharing.
- Rating System: Team leaders can rate team members to promote accountability and encourage excellence.
- Mentorship: Researchers can request guidance and support from experienced mentors in their respective fields.
## Installation

Clone the repository using the following command:

```bash
git clone https://github.com/your-username/ResWeb.git
```
Open the solution file in Visual Studio or your preferred IDE.

Build the solution to restore NuGet packages and resolve dependencies.

Ensure you have a SQL Server instance set up. Update the connection string in the appsettings.json file to point to your database.

Run the database migrations to create the necessary tables:'
```sql
dotnet ef database update
```
Start the application:
```bash
dotnet run
```
