to add migrations just run 
dotnet ef migrations add InitialCreate --project Data --startup-project api

to update 
dotnet ef database update --project Data --startup-project api
