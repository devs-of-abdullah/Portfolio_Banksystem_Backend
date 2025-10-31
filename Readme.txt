														  **Bank System (Backend)** 

This project was developed by Abdullah(me) to showcase my skills and ability to write clean, well-structured, and maintainable backend code.

The backend is built using .NET Core (C#) with Entity Framework Core for database integration and JWT for secure user authentication.

The architecture follows a 3-tier structure for better organization and scalability:

Data Layer: Contains entities and database context.

Business Layer: Includes interfaces, services, and utility classes where most of the logic resides.

API Layer: Exposes endpoints and handles communication with the frontend through JSON responses.

Currently, the system focuses on three main entities: User, Account, and Transaction — with plans to add more in the future.

														** Entity Framework Commands **

To add a migration:

dotnet ef migrations add InitialCreate --project Data --startup-project api

To update the database:

dotnet ef database update --project Data --startup-project api