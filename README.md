# Quizzy

Quizzy is a robust quiz platform that allows users attempt quizzes. The platform provides various services for user authentication, quiz creation, answer validation, and results tracking. 

## Features

- User Authentication (Registration, Login, and User Management)
- Quiz Creation and Management
- Question and Answer Management
- Quiz Attempts and Result Tracking
- API Services for all functionalities

## Installation

1. Clone the repository:
  ```bash
  git clone <your-repo-url>
  cd Quizzy
  ```

2. Build the solution:
  ```bash
  dotnet build
  ```

3. Update the database connection string in the appsettings.json file under Quizzy.Api to point to your database.
  
4. Run the migrations to set up the database:
  ```bash
  dotnet ef database update
  ```

5. Start the API:
  ```bash
  dotnet run --project Quizzy.Api
  ```

6. Open the Swagger window and enjoy :)
