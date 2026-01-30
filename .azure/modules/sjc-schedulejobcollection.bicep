
@description('The name of the Job Collection. Must be 1-60 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(60)
param name string

@description('The Azure region where the Job Collection will be deployed.')
@minLength(1)
@maxLength(60)
param location string = toLower(replace(resourceGroup().location, ' ', ''))

@description('The SKU (pricing tier) for the Job Collection. Allowed value: Standard. Default is Standard.')
@allowed([
  'Standard'
])
param sku string = 'Standard'

@description('The name of the associated Web App. Must be 1-60 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(60)
param webName string

@description('The maximum number of jobs allowed in the Job Collection. Default is 10.')
@minValue(1)
param maxJobs int = 10

@description('The interval for the job recurrence. Default is 5.')
@minValue(1)
param timerInterval int = 5

@description('The frequency for the job recurrence. Default is minute.')
param timerFrequency string = 'minute'

@description('The start time for the scheduled job. Default is the current UTC time.')
param startTime string = utcNow()

var nameLower = toLower(replace(replace(name, '-', ''), ' ', ''))

resource name_resource 'Microsoft.Scheduler/jobCollections@2016-03-01' = {
  name: name
  location: location
  properties: {
    sku: {
      name: sku
    }
    quota: {
      maxJobCount: maxJobs
      maxRecurrence: {
        frequency: timerFrequency
        interval: timerInterval
      }
    }
  }
  dependsOn: []
}

resource name_nameLower 'Microsoft.Scheduler/jobCollections/jobs@2014-08-01-preview' = {
  parent: name_resource
  name: '${nameLower}'
  properties: {
    startTime: startTime
    action: {
      request: {
        uri: '${list(resourceId('Microsoft.Web/sites/config', webName, 'publishingcredentials'), '2014-06-01').properties.scmUri}/api/triggeredjobs/MyScheduledWebJob/run'
        method: 'POST'
      }
      type: 'Http'
      retryPolicy: {
        retryType: 'Fixed'
        retryInterval: 'PT1M'
        retryCount: 2
      }
    }
    state: 'Enabled'
    recurrence: {
      frequency: timerFrequency
      interval: timerInterval
    }
  }
}
