cd ../
del Migrations/*
rd Migrations
dotnet ef migrations add InitialCreate
dotnet ef migrations script