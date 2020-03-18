dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.WebApi.Host.Tests\TauCode.WebApi.Host.Tests.csproj
dotnet test -c Release .\tests\TauCode.WebApi.Host.Tests\TauCode.WebApi.Host.Tests.csproj

nuget pack nuget\TauCode.WebApi.Host.nuspec
