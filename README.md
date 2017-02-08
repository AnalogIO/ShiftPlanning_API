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
  <add key="TokenKey" value="SYMMETRICKEY"/>
  <add key="TokenAgeHour" value="24"/>
</appSettings>
```

Live documentation of the API can be found here:
https://analogio.dk/publicshiftplanning/Help

We are using shortkeys to differ between organizations.
For instance, the shortkey for Café Analog is `analog`
Therefore, a call to get shifts could look like this:
https://analogio.dk/publicshiftplanning/api/shifts/analog
