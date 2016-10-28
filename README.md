# Analog-ShiftPlanner

The project requires a configuration file `connections.config` and `appSettings.config` with the following format:

```xml
<connectionStrings>
  <add name="ShiftPlannerDataContext"
       connectionString="Server=<HOST>;port=<PORT>;Database=<DBNAME>;User Id=<USERNAME>;Password=<PASSWORD>;"
       providerName="Npgsql" />
</connectionStrings>
```
```xml
<appSettings>
  <add key="ApiKey" value="<ApiKey>"/>
  <add key="TokenKey" value="<SymmetricKey>"/>
</appSettings>
```
