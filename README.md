##  ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file ⛔

# Employer Demand Web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/SkillsFundingAgency.das-employerdemand-web?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2181&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-employerdemand-web&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/secure/RapidBoard.jspa?rapidView=664)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/2393178481/AED)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)


Employer Demand is a service to connect employers who cannot find appropriate training provision with providers who can potentially provide that provision.

## How It Works

Employer Demand web consumes [das-apim-endpoints](https://github.com/skillsfundingagency/das-apim-endpoints). The entry point to the Employer Demand journey is accessed through [das-findapprenticeshiptraining](https://github.com/SkillsFundingAgency/das-findapprenticeshiptraining/).
It also makes use of [SFA.DAS.Provider.Shared.UI](https://github.com/SkillsFundingAgency/das-shared-packages/tree/master/SFA.DAS.Provider.Shared.UI).

## 🚀 Installation

### Pre-Requisites

* A clone of this repository
* DotNet Core 3.1 and any supported IDE for DEV running.
* Azure Storage Emulator if not running in DEV mode

### Config

Appsetting.json file
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.EmployerDemand.Web",
  "EnvironmentName": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": "",
  "AllowedHosts": "*",
  "cdn": {
    "url": "https://das-at-frnt-end.azureedge.net"
  }
}
```


Azure Table Storage config

PartitionKey: LOCAL

RowKey: SFA.DAS.EmployerDemand.Web_1.0

Data:
```
{
 "EmployerDemandApi":{
     "Key":"test",
     "BaseUrl":"http://localhost:5021/",
     "PingUrl":"http://localhost:5021/"
    },
 "EmployerDemand":{
     "RedisConnectionString":" ",
     "FindApprenticeshipTrainingUrl":"https://at-findapprenticeshiptraining.apprenticeships.education.gov.uk"
    },
 "ProviderIdams":{
     "MetadataAddress":"",
     "Wtrealm":"https://localhost:5011/"
    },
 "ProviderSharedUIConfiguration":{
     "DashboardUrl":"https://at-pas.apprenticeships.education.gov.uk/"
    }
}
```

## Local Running

### Mock Server

Employer Demand web comes with a mock server that can be run alongside the web app to remove dependencies on other solutions.

It is important that your BaseUrl in your config is pointed to the MockServer url
You are able to run the website by doing the following:
* Run the console app ```SFA.DAS.EmployerDemand.MockServer``` - this will create a mock server on http://localhost:5021
* Start the web solution ```SFA.DAS.EmployerDemand.Web```

### Using APIM

Employer Demand web can be run alongside APIM, along with its dependencies, for end to end running.

## 🔗 External Dependencies

Authentication is managed via Azure Managed Identity when not running locally.

## Technologies

* .NetCore 3.1
* NLog
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions

## 🐛 Known Issues