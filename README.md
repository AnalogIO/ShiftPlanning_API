# Analog-ShiftPlanner

The project requires a configuration file `connections.config` and `appSettings.config` with the following format:

```xml
<connectionStrings>
  <add name="ShiftPlannerDataContext"
       connectionString="Server=HOST;port=PORT;Database=DBNAME;User Id=USERNAME;Password=PASSWORD;"
       providerName="Npgsql" />
</connectionStrings>
```
```xml
<appSettings>
  <add key="ApiKey" value="A-KEY-123"/>
  <add key="TokenKey" value="SYMMETRICKEY"/>
</appSettings>
```
