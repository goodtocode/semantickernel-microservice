# Semantic Kernel Microservice Quick-Start
A simple Semantic Kernel CRUD Microservice solution including Domain Models, Aggregates, Persistence Repositories and an API presentation layer. This demonstrates the most basic use cases of Semantic Kernel in an Clean Architecture Microservice.

## Prerequisites
You will need the following tools:
### Visual Studio
[Visual Studio Workload IDs](https://learn.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-community?view=vs-2022&preserve-view=true)
```
winget install --id Microsoft.VisualStudio.2022.Community --override "--quiet --add Microsoft.Visualstudio.Workload.Azure --add Microsoft.VisualStudio.Workload.Data --add Microsoft.VisualStudio.Workload.ManagedDesktop --add Microsoft.VisualStudio.Workload.NetWeb"
```
### Or VS Code (code .)
```
winget install Microsoft.VisualStudioCode --override '/SILENT /mergetasks="!runcode,addcontextmenufiles,addcontextmenufolders"'
```

### .NET SDK (dotnet)
```
winget install Microsoft.DotNet.SDK.8 --silent
```

### SQL Server
[Optional: SQL Server 2022 or above](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Configurations
Follow these steps to get your development environment set up:

### ASPNETCORE_ENVIRONMENT set to "Local" in launchsettings.json
1. This project uses the following ASPNETCORE_ENVIRONMENT to set configuration profile
- Debugging uses Properties/launchSettings.json
- launchSettings.json is set to Local, which relies on appsettings.Local.json
2. As a standard practice, set ASPNETCORE_ENVIRONMENT entry in your Enviornment Variables and restart Visual Studio
	```
	Set-Item -Path Env:ASPNETCORE_ENVIRONMENT -Value "Development"
	Get-Childitem env:
	```	
  
### Setup Azure Open AI or Open AI configuration
**Important:** Do this for both Presentation.WebAPI and Specs.Infrastructure
#### Azure Open AI in Presentation.WebAPI and Specs.Infrastructure
```
cd src/Presentation/WebAPI
dotnet user-secrets init
dotnet user-secrets set "AzureOpenAI:ChatDeploymentName" "gpt-4"
dotnet user-secrets set "AzureOpenAI:Endpoint" "https://YOUR_ENDPOINT.openai.azure.com/"
dotnet user-secrets set "AzureOpenAI:ApiKey" "YOUR_API_KEY"
```
Alternately you can set in Environment variables
```
AzureOpenAI__ChatDeploymentName
AzureOpenAI__Endpoint
AzureOpenAI__ApiKey
```

#### Open AI
```
cd src/Presentation/WebAPI
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ChatModelId" "gpt-3.5-turbo"
dotnet user-secrets set "OpenAI:ApiKey" "YOUR_API_KEY"
```
Alternately you can set in Environment variables
```
OpenAI__ChatModelId	
OpenAI__ApiKey
```

### Setup your SQL Server connection string
```
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_SQL_CONNECTION_STRING"
```

### Launch the backend
Right-click Presentation.WebApi and select Set as Default Project
```
dotnet run WebApi.csproj
```

### Open http://localhost:7777/swagger/index.html in your browser to the Swagger API Interface
Open Microsoft Edge or modern browser
Navigate to: http://localhost:7777/swagger/index.html
  
## dotnet ef migrate steps
### dotnet ef cli
Install
```
dotnet tool install --global dotnet-ef
```
Update
```
dotnet tool update --global dotnet-ef
```
Remember to add the following package to appropriate project
```
dotnet add package Microsoft.EntityFrameworkCore.Design
```
### Steps

1. Open Windows Terminal in Powershell or Cmd mode
2. cd to /src folder
3. (Optional) If you have an existing database, scaffold current entities into your project
	
	```
	dotnet ef dbcontext scaffold "Data Source=localhost;Initial Catalog=semantickernelmicroservice;Min Pool Size=3;MultipleActiveResultSets=True;Trusted_Connection=Yes;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -t WeatherForecastView -c WeatherChannelContext -f -o WebApi
	```

4. Create an initial migration
	```
	dotnet ef migrations add InitialCreate --project .\Infrastructure\SqlServer\Infrastucture.SqlServer.csproj --startup-project .\Presentation\WebApi\Presentation.WebApi.csproj --context ChatCompletionContext
	```

5. Develop new entities and configurations
6. When ready to deploy new entities and configurations
   
	```	
	dotnet ef database update --project .\Infrastructure\SqlServer\Infrastucture.SqlServer.csproj --startup-project .\Presentation\WebApi\Presentation.WebApi.csproj --context ChatCompletionContext --connection "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SemanticKernelMicroservice;Min Pool Size=3;MultipleActiveResultSets=True;Trusted_Connection=Yes;TrustServerCertificate=True;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30"
	```

## dotnet new steps
1. Start Windows Terminal
2. Navigate to template.json folder
	```
	cd ./dotnet-microservices/v3-webapi/.template/dotnet-new
	```
3. Install Template command: 
	```
	dotnet new --install .
	```
4. Create a folder where you're creating your new solution
	```
	mkdir /repos/dotnet-microservice
	cd /repos/dotnet-microservice
	```
5. Create microservice solution
	```
	dotnet new gtc-msv3 -o "MyOrg.DomainMicroservice"
	```
# Entity Framework vs. Semantic Kernel Memory
This example uses Entity Framework (EF) to store messages and responses for Semantic Kernel, and does not rely on SK Memory (SM). EF and SM serve different purposes. If you need natural language querying and efficient indexing, Semantic Kernel Memory is a great fit. If you’re building a standard application with a relational database, Entity Framework is more appropriate.

The key differences between Entity Framework (EF) and Semantic Kernel memory:

## Purpose and Functionality
- Entity Framework (EF): EF is an Object-Relational Mapping (ORM) framework for .NET applications. It allows you to map database tables to C# classes and provides an abstraction layer for database operations. EF focuses on CRUD (Create, Read, Update, Delete) operations and data modeling.
- Semantic Kernel Memory: Semantic Kernel Memory (SM) is part of the Semantic Kernel project. It’s a library for C#, Python, and Java that wraps direct calls to databases and supports vector search. SM is designed for long-term memory and efficient indexing of datasets. It’s particularly useful for natural language querying and retrieval augmented generation (RAG).

## Data Storage and Retrieval
- EF: EF stores data in relational databases (e.g., SQL Server, MySQL, PostgreSQL). It uses SQL queries to retrieve data.
- SM: SM can use various storage mechanisms, including vector databases. It supports vector search, which allows efficient similarity-based retrieval. SM is well-suited for handling large volumes of data and complex queries.

## Querying
- EF: EF queries are typically written in LINQ (Language Integrated Query) or SQL. You express queries in terms of C# objects and properties.
- SM: SM supports natural language querying. You can search for information using text-based queries, making it more user-friendly for applications like chatbots.

## Integration with Chat Systems
- EF: EF doesn’t directly integrate with chat systems. It’s primarily used for data persistence.
- SM: SM integrates seamlessly with chat systems like ChatGPT, Copilot, and Semantic Kernel. It enhances data-driven features in AI applications.

## Scalability and Performance
- EF: EF is suitable for small to medium-sized applications. It may not perform optimally with very large datasets.
- SM: SM is designed for scalability. It can handle large volumes of data efficiently, making it suitable for memory-intensive applications.

## Use Cases
- EF: Use EF for traditional CRUD operations, business logic, and data modeling.
- SM: Use SM for long-term memory, chatbots, question-answering systems, and information retrieval.

# Contact
* [GitHub Repo](https://www.github.com/goodtocode/templates)
* [@goodtocode](https://www.twitter.com/goodtocode)
* [github.com/goodtocode](https://www.github.com/goodtocode)

# Technologies
* [ASP.NET .Net](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [Specflow](https://specflow.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [FluentAssertions](https://fluentassertions.com/)
* [Moq](https://github.com/moq)

# Semantic Kernel
* [GitHub](https://github.com/microsoft/semantic-kernel.git)
* [Getting Started Blog](https://devblogs.microsoft.com/semantic-kernel/how-to-get-started-using-semantic-kernel-net/)
* [Understanding the Kernel](https://learn.microsoft.com/en-us/semantic-kernel/agents/kernel/?tabs=Csharp)
* [Creating Plugins](https://devblogs.microsoft.com/semantic-kernel/using-semantic-kernel-to-create-a-time-plugin-with-net/)

## Additional Technologies References
* AspNetCore.HealthChecks.UI
* Entity Framework Core
* FluentValidation.AspNetCore
* Microsoft.AspNetCore.App
* Microsoft.AspNetCore.Cors
* Swashbuckle.AspNetCore.SwaggerGen
* Swashbuckle.AspNetCore.SwaggerUI


This project is licensed with the [MIT license](LICENSE).