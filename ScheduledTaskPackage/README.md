# $projectname$

TODO: WRITE THIS README.

This is an empty task deployment project.

* The project is packaged for OctopusDeploy.
* Add your task's logic in Program.Main.
* The TaskDefinition.xml and PostDeploy.ps1 files can be customised to change scheduling rules, etc. The default behaviour is to run daily.
* When configuring the deployment, ensure that Octopus variable substitution is performed on TaskDefinition.xml.
* An empty App.config has also been supplied.

Three variables have been defined:
* $safeprojectname$Principal: The NT user account under which the task will run.
* $safeprojectname$Password: The password for the above NT user account.
* $safeprojectname$StartTime: The time, in HH:mm form, when the task will be run.
