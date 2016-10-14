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
  <add key="APIKEY" value="1618849b911d4b53bb2f714b0b41e721d70373184872dc5d0b78100acb29cac1"/>
</appSettings>
```
