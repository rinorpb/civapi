# Civapi
Prototype API for an assessment about handling sensitive documents. 
Made with .NET 10.0.14 (SDK 10.0.403)

## Installation
Make sure you have .NET SDK 10 installed. 
Clone the repo and execute `dotnet run --project Civapi`, this will run the API part of the project once and install the required NuGet packages. 

## Testing
Simply run `dotnet test` to run the unit tests for the document classification function.  

An example database is supplied in the `Civapi` project with the name `Documents.example.db`. Simply copy at and rename it `Database.db` if it does not exist already.

Testing the API with HTTP requests is more involved. 
I recommend using `dotnet user-jwts create --role "admin" --scope "internal personal sensitive"` to create a JWT locally for testing. 
Feel free to make multiple tokens with different scopes. 
An overview of your tokens can be found using `dotnet user-jwts list` and the token itself can be viewed with `dotnet user-jwts print [Token ID]`. 
You can then use this token at the top of the `Civapi.http` file in the `Civapi` project to easily execute HTTP requests with your IDE of choice. 
I used JetBrains Rider. 
CLI tools also exist for this purpose.
