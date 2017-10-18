.NET Core is the modular and high performance implementation of .NET for creating web applications and services that run on Windows, Linux and Mac.

1) git clone https://gitlab.com/freshcode-.net/NetCoreLibraryApp.git -b v1.1

2) build

3) npm install (or Restore packajes)

4) - Update-Database (windows) 
   - $ sudo dotnet ef database update (linux).
   
5) check posgreSql and mongoDb connection

6) check env settings , if you use Rider . Env settings must be used for development not for production. 

7) replace project url in all projects solution Properties->lounchSettings.json  use project urls like from Config.cs in LibraryAppCore.AuthServer

