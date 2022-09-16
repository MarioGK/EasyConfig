# EasyConfig #

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![NuGet](https://buildstats.info/nuget/EasyConfigStandard)](https://www.nuget.org/packages/EasyConfigStandard/)

## Tutorial ##

Create a config class or record:

```csharp
public class Config
{
    public string Prop { get; set; }
}
```

Then you can fetch the data from the fille like this:


```csharp
var config = ConfigurationManager.Get<Config>();
```

When there is a need to save the config you can do it like this:

```csharp
ConfigurationManager.Save(config);
```

That's it, really easy to use like the name.