FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ["ShiftPlanning.Shifty/ShiftPlanning.Shifty.csproj", "ShiftPlanning.Shifty/"]
COPY ["ShiftPlanning.DTOs/ShiftPlanning.DTOs.csproj", "ShiftPlanning.DTOs/"]
RUN dotnet restore "ShiftPlanning.Shifty/ShiftPlanning.Shifty.csproj"

COPY . ./
RUN dotnet publish "ShiftPlanning.Shifty/ShiftPlanning.Shifty.csproj" -c Release -o output

FROM nginx:alpine AS prod
WORKDIR /var/www/web
COPY --from=build-env /app/output/wwwroot .
COPY "ShiftPlanning.Shifty/nginx.conf" /etc/nginx/nginx.conf
EXPOSE 80
EXPOSE 443
