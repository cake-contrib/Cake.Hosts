# Cake.Hosts
Cake.Hosts addon for Cake Build: manipulation of hosts file from Cake. This addon provides the following functionality:

* Check if a record already exists in your hosts file
* Add a new record to your hosts file
* Remove a record from hosts file
 
This addon is aimed on automation and helping setting up of local development environment rather than for anything else. I can not see a need to change hosts file on any server in a production environment/build. However, please let me know if you do use it in production - I'm interested to hear your story.

You can add this addon added to your cake script via nuget:
```c#
#addin "Cake.Hosts"
```

### Check if Record Exists

``` c#
HostsRecordExists("127.0.0.1", "myproject.dev");
```
This does regex match through your hosts file and checks if there is a corresponding record.

### Add New Record

``` c#
AddHostsRecord("127.0.0.1", "myproject.dev");
```
This appends a new corresponding line to your hosts file.

### Remove Recrod

```c#
RemoveHostsRecord("127.0.0.1", "myproject.dev");
```


