
$ErrorActionPreference = "Stop";

$thisDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent;

function Read-TaskDefinition($inputPath)
{
    $xml = New-Object System.Xml.XmlDocument;
    $xml.PreserveWhitespace = $true;
    $xml.Load($inputPath);
    return $xml;
}

function Write-TaskSchedulerCompatibleXml([xml]$sourceXml, [string]$outputPath)
{
    # Task Scheduler appears to demand UTF-16 XML with no schemaLocation attribute.
    $xml = $sourceXml.CloneNode($true);

    $xsiPrefix = $xml.DocumentElement.GetPrefixOfNamespace("http://www.w3.org/2001/XMLSchema-instance");
    $xsiSchemaLocationName = "${xsiPrefix}:schemaLocation";
    $xml.DocumentElement.RemoveAttribute($xsiSchemaLocationName);

    $options = New-Object System.Xml.XmlWriterSettings;
    $options.Encoding = [System.Text.Encoding]::Unicode;
    $stream = [System.IO.File]::Open($outputPath, "Create");
    $writer = [System.Xml.XmlWriter]::Create($stream, $options);
    $xml.Save($writer);
    $writer.Close();
    $stream.Dispose();
}

function Select-TaskDefinitionNodes([xml]$xml, [string]$xpath)
{
    $nsmgr = New-Object System.Xml.XmlNamespaceManager $xml.NameTable;
    $nsmgr.AddNamespace("x", "http://schemas.microsoft.com/windows/2004/02/mit/task");
    return $xml.SelectNodes($xpath, $nsmgr);
}

function Set-ActionsExecWorkingDirectory([xml]$xml, [string]$workingDirectory)
{
    Select-TaskDefinitionNodes $xml "/x:Task/x:Actions/x:Exec/x:WorkingDirectory" |
        # Only set empty/whitespace working directories.
        where { !$_.InnerText.Trim() } |
        foreach { $_.InnerText = $workingDirectory; }
}

function Resolve-FullyQualifiedPrincipalName($shortName)
{
    return New-Object System.Security.Principal.NTAccount($shortName)
        .Translate([System.Security.Principal.SecurityIdentifier])
        .Translate([System.Security.Principal.NTAccount])
        .Value;
}

$xml = Read-TaskDefinition "${thisDir}\TaskDefinition.xml";
Set-ActionsExecWorkingDirectory $xml $thisDir;
Write-TaskSchedulerCompatibleXml $xml "${thisDir}\TaskDefinition-Compatible.xml"

if (!$$projectname$Principal) { throw "`$$projectname$Principal was not specified." }
$fullyQualifiedPrincipalName = Resolve-FullyQualifiedPrincipalName "${$projectname$Principal}";

schtasks.exe /Create /TN "$projectname$" /XML "${thisDir}\TaskDefinition-Compatible.xml" /RU "${fullyQualifiedPrincipalName}" /RP "${$projectname$Password}" /F
