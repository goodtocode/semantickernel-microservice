@minLength(1)
@maxLength(60)
param name string

@minLength(1)
@maxLength(60)
param location string = toLower(replace(resourceGroup().location, ' ', ''))

@description('Describes jobCollection pricing tier')
@allowed([
  'standard'
])
param sku string = 'standard'

@minLength(1)
@maxLength(60)
param webName string
param maxJobs int = 10
param timerInterval int = 5
param timerFrequency string = 'minute'
param startTime string = utcNow()

var nameLower = toLower(replace(replace(name, '-', ''), ' ', ''))

resource name_resource 'Microsoft.Scheduler/jobCollections@2014-08-01-preview' = {
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