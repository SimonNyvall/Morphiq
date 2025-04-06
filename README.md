# Morphiq

[![Build & Test](https://github.com/SimonNyvall/Morphiq/actions/workflows/build.yml/badge.svg)](https://github.com/SimonNyvall/Morphiq/actions/workflows/build.yml)

## Premise
Morphiq is a versatile tool that enhances object mapping and transformation in C# using attribute-based configurations. It simplifies the process of converting objects between different types, making it ideal for scenarios such as data transfer objects (DTOs), view models, and response objects in web APIs.

## Features
* Attribute-Based Mapping: Use attributes to define custom mappings, default values, and properties to ignore.
* Custom Mapping Methods: Define custom mapping logic using methods specified by attributes.
* Default Value Handling: Automatically set default values for properties when the source property is null.
* Property Ignoring: Exclude specific properties from the mapping process using ignore attributes.
* Flexible Configuration: Apply additional configuration to the target object during the mapping proces

## Example

### Basic Usage
```csharp
using Morphiq;

public class Entity
{
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 30;
}

public class DTO
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

var entity = new Entity();

var dto = entity.MorphTo<DTO>();

Console.WriteLine(dto.Name); // Output: John Doe
Console.WriteLine(dto.Age);  // Output: 30
```

### Attributes
Some attributes are available to control the behavior of the mapping.

#### IgnoreProperty

```csharp
using Morphiq;
using Morphiq.Attributes;

public class Entity
{
    public string Name { get; set; } = "John Doe";
    [IgnoreProperty]
    public int Age { get; set; } = 30;
}

public class DTO
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var entity = new Entity();
var dto = entity.MorphTo<DTO>();

Console.WriteLine(dto.Name); // Output: John Doe
Console.WriteLine(dto.Age);  // Output: 0 (default value)
```

#### Default Value

```csharp
using Morphiq;
using Morphiq.Attributes;

public class Entity
{
    [MorphToDefaultValue("Unknown")]
    public string Name { get; set; }
    public int Age { get; set; } = 30;
}

public class DTO
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var entity = new Entity();
var dto = entity.MorphTo<DTO>();

Console.WriteLine(dto.Name); // Output: Unknown
Console.WriteLine(dto.Age);  // Output: 30
```

#### Custom Mapping

```csharp
using Morphiq;
using Morphiq.Attributes;

public class Entity
{
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 30;

    [MorphToCustomMapping(nameof(MapName))]
    public string FullName { get; set; } = "John Doe";

    private string MapName(object value)
    {
        return $"Mapped: {value}";
    }
}

public class DTO
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string FullName { get; set; }
}

var entity = new Entity();
var dto = entity.MorphTo<DTO>();

Console.WriteLine(dto.Name);     // Output: John Doe
Console.WriteLine(dto.Age);      // Output: 30
Console.WriteLine(dto.FullName); // Output: Mapped: John Doe
```

#### Configuration
```csharp
using Morphiq;

public class Entity
{
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 30;
}

public class DTO
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var entity = new Entity();
var dto = entity.MorphTo<DTO>(x => x.Name = "Jane Doe");

Console.WriteLine(dto.Name); // Output: Jane Doe
Console.WriteLine(dto.Age);  // Output: 30
```

## Installation

## License
This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.