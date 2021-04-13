# Konfidence.BaseClasses 
This is a collection of dotnet library projects I use for the software I develop, containing shortcuts or base implementations that make developing and maintaining software a lot easier.
  
# Using the libraries: 
All projects generate a .nupkg on build. These can be used to inlcude in your software projects. Most libraries are build for **netstandard2.0** and **net5.0**.

# Build and Test

- Clone Konfidence.BaseClasses
- Open  Konfidence.BaseClasses.sln in visual studio
- build the solution

All tests can either be run in visual studio, with dotnet test konfidence.baseclasses.sln or dotnet test [testprojectname].tests.csproj. 

Integration tests running against SQl server will fail until a test database setup is made available.

# Libraries

### Konfidence.BaseClasses
- Some extensions to make reading some constructs more fluent
	- when objects/string/guid/datatime/timespan assignments are actually assigned, .IsAssigned()
	- eof for stream reading, .IsEof()
	- string is a guid, .IsGuid()
	- string is numeric,  .IsNumeric()
	- earliest and latest time on a day, .StartOfDayTime(), .EndOfDayTime()
- CommandLineArgument parser: Meant for the configuration argument line parser used with MS dependency injection.
- Environment Variable getter: unified(user, machine, process). Should work on both Windows and Linux.
- Some (unexpected) fast string extensions
	- TrimStart(..), TrimStartIgnoreCase(..), TrimEnd(..), TrimEndIgnoreCase(..)
	- TrimList()
	- ReplaceIgnoreCase(..)
	- InitLowerCase(), InitUpperCase()
	- Contains(..) with specified casing type
	- ToDecimal(), parse a string into a decimal
  
this package is available on [nuget.org](https://www.nuget.org/packages/Konfidence.BaseClasses). 

### Konfidence.BaseDataBaseClasses
- Some classes that make CRUD on SQL with the enterprise libraries realy easy, without the strong dependencies included in the EntityFramework, only usefull with my ClassGenerator. Also referenced by the Konfidence.SqlHostProvider package.

### Konfidence.BaseRest.Client
- Client for basic Restservice access. Using the RestSharp client. Used by my ClassGenerator.
  
### Konfidence.BaseThreadClasses
- basic manage-run-action pattern for simple threading. Consider using simpler Tasks before using this library.

### Konfidence.DataBaseInterface
- Interfacing between Konfidence.BaseDataBaseClasses && Konfidence.SqlHostProvider. And ofcourse dependency injection infra.  

### Konfidence.Mail
- Base smtp client implementation: new BaseMailSender(..) -> SendEmail(..) 

### Konfidence.Security
- Creation and retrieval of public and private RSA keys.
- Save in (secured) local storage.
- Delete from (secured) local storage.
- Encoding with a shared public key.
- Decoding with your secret private key. 

### Konfidence.SqlHostProvider
Provides MS Sql database access, based on the enterprise libaries. Configured with app.config and SqlClientSettings.json. Allows manipulation of app.config settings directly or in memory. Allows pinging for a SqlServer instance. Can return a DatabaseStructure, which describes a database. Its tables, columns, types and some constraints. Used by my ClassGenerator and its generated artifacts.

### Konfidence.TestTools
Prepares the configuration of a unittest with live access to SqlServer. Since dotnet, the location of the TestHost and where the tests are running are different. The app.config not being in the expected location is an issue.

### Konfidence.UtilHelper
Some obsolete classes. Technical debt :(   

### Tools/ClientSettingsUpdater
For me: updates the SqlClientSettings.json in a buildpipeline, keeping secrets out of the git repository. Packed as installable dotnet tool.
