# Configuration of API

Both the `Public API` and `API` solutions relies on a `appSettings.config` and `connections.config` to run. Both solutions depend on the same `connections.config` file since they connect to the same data source.

The config files are saved in the root of the ASP.NET project.

`connections.config`
```xml
<connectionStrings>
  <add name="ShiftPlannerDataContext"
       providerName="System.Data.SqlClient"
       connectionString=""/>
</connectionStrings>
```

## Public API

The `Public API` solution needs the following configuration:

`appSettings.config`

```xml
 <appSettings>
  <add key="databaseSchema" value="" /> <!-- Database Schema used, either prod or test -->
</appSettings>
```



## API

The `API` solution needs the following configuration:

`appSettings.config`

```xml
 <appSettings>
  <add key="ApiKey" value=""/> <!--  -->
  <add key="TokenKey" value=""/> <!-- Token key for TokenManager -->
  <add key="TokenAgeHour" value="24"/> <!-- Token expiration -->
  <add key="podio_app_id" value=""/> <!-- Must refer exactly the Podio app and view for Applicants -->
  <add key="podio_app_token" value=""/> <!-- Podio API credentials -->
  <add key="podio_client_id" value=""/>
  <add key="podio_client_secret" value=""/>
  <add key="podio_applicants_app_id" value=""/> <!-- Must refer exactly the Podio app and view for Applicants -->
  <add key="podio_view_id" value=""/> 
  <add key="ftpUsername" value=""/> <!-- FTP credentials and host (location) for saving the member pictures -->
  <add key="ftpPassword" value=""/>
  <add key="ftpHost" value=""/>
  <add key="databaseSchema" value="test" /> <!-- Database Schema used, either prod or test -->
</appSettings>
```