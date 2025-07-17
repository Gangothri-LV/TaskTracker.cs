# Task Tracker Console App

This is a simple C# console application that allows users to register, login, and manage personal tasks using file storage.

## Features

- User registration and login
- Task management (Add, View, Complete, Delete)
- Data stored in `.csv` files (no database needed)

## How to Run

1. Make sure .NET 6.0 SDK is installed.
2. Clone this repo.
3. Run using:
   ```
   dotnet build
   dotnet run
   ```

## File Storage

- `users.csv` — stores username and password
- `tasks.csv` — stores tasks per user

## License

MIT