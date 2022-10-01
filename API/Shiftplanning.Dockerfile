FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /sln

COPY ./*.sln ./

# Copy the main source project files, done to avoid doing the restoration of nuget packages unless needed
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj | grep -v "*Shifty*"); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore

# Remainder of build process
COPY . .
RUN dotnet build -c Release -o /app/build --no-restore ShiftPlanning.WebApi/ShiftPlanning.WebApi.csproj

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore ShiftPlanning.WebApi/ShiftPlanning.WebApi.csproj

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShiftPlanning.WebApi.dll"]
