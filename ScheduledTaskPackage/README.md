# $projectname$

TODO: WRITE THIS README.

This is an empty task deployment project.

* The project is packaged for OctopusDeploy.
* Add your task's logic in Program.Main.
* The TaskDefinition.xml and PostDeploy.ps1 files can be customised to change scheduling rules, etc. The default behaviour is to run daily.
* When configuring the deployment, ensure that Octopus variable substitution is performed on TaskDefinition.xml.
* A default App.config has also been supplied which sets up log4net logging to a file, rotated after 7 runs.

## Deployment

### Package Parameters

Three variables have been defined:
* **$projectname$Principal**: The NT user account under which the task will run.
  * Type: NTPrincipal
* **$projectname$Password**: The password for the above NT user account.
  * Type: string
* **$projectname$StartTime**: The time, in HH:mm or HH:mm:ss form, when the task will be run.
  * Type: TimeSpan
