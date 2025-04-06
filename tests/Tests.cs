using Morphiq;
using Morphiq.Attributes;
using tests.Data;

namespace tests;

public class Tests
{
    [Test]
    public async Task Given_Type_When_MorphTo_Then_PropertiesAreMapped()
    {
        var fromObj = new DataClass.FromClass
        {
            Property1 = "Hello",
            Property2 = 123,
            Property3 = true
        };

        var toObj = fromObj.MorphTo<DataClass.ToClass>();

        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<DataClass.ToClass>();
        await Assert.That(toObj.Property1).IsEqualTo("Hello");
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }

    [Test]
    public async Task Given_TypeWithNonMathingName_When_MorphTo_Then_PropertiesAreNotCorrectlyMapped()
    {
        var fromObj = new TestClass
        {
            PropertyMissMatch = "Hello",
            Property2 = 123,
            Property3 = true
        };
        
        var toObj = fromObj.MorphTo<DataClass.ToClass>();
        
        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<DataClass.ToClass>();
        await Assert.That(toObj.Property1).IsEqualTo(string.Empty);
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }

    [Test]
    public async Task Given_TypeWithNonMatchingName_When_MorphTo_Then_PropertiesAreMappen_When_AttributeApplied()
    {
        var fromObj = new TestClassWithAttribute
        {
            PropertyMissMatch = "Hello",
            Property2 = 123,
            Property3 = true
        };
        
        var toObj = fromObj.MorphTo<DataClass.ToClass>();
        
        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<DataClass.ToClass>();
        await Assert.That(toObj.Property1).IsEqualTo("Hello");
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }

    [Test]
    public async Task Given_TypeWithNonMatchingName_When_MorphTo_Then_PropertiesAreMapped_When_Configuration()
    {
        var fromObj = new TestClass
        {
            PropertyMissMatch = "Hello",
            Property2 = 123,
            Property3 = true
        };
        
        var toObj = fromObj.MorphTo<TestClassWithAttribute>(x => x.PropertyMissMatch = "Hello World");
        
        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<TestClassWithAttribute>();
        await Assert.That(toObj.PropertyMissMatch).IsEqualTo("Hello World");
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }
    
    [Test]
    public async Task Given_TypeWithIgnoreAttribute_When_MorphTo_Then_PropertyIsIgnored()
    {
        var fromObj = new DataClass.FromClass
        {
            Property1 = "Hello",
            Property2 = 123,
            Property3 = true
        };

        var toObj = fromObj.MorphTo<ClassWithIgnoreAttribute>();

        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<ClassWithIgnoreAttribute>();
        await Assert.That(toObj.Property1).IsEqualTo(string.Empty); // Ignored
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }

    [Test]
    public async Task Given_TypeWithDefaultValueAttribute_When_MorphTo_Then_DefaultValueIsSet()
    {
        var fromObj = new DataClass.FromClass
        {
            Property1 = null,
            Property2 = 123,
            Property3 = true
        };

        var toObj = fromObj.MorphTo<ClassWithDefaultValueAttribute>();

        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<ClassWithDefaultValueAttribute>();
        await Assert.That(toObj.Property1).IsEqualTo("Default Value"); // Default value
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }

    [Test]
    public async Task Given_TypeWithCustomMappingAttribute_When_MorphTo_Then_CustomMappingIsApplied()
    {
        var fromObj = new DataClass.FromClass
        {
            Property1 = "Hello",
            Property2 = 123,
            Property3 = true
        };

        var toObj = fromObj.MorphTo<ClassWithCustomMappingAttribute>();

        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<ClassWithCustomMappingAttribute>();
        await Assert.That(toObj.Property1).IsEqualTo("Mapped: Hello"); // Custom mapping
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }
    
    [Test]
    public async Task Given_TypeWithIgnoreAttributeOnFromClass_When_MorphTo_Then_PropertyIsIgnored()
    {
        var fromObj = new FromClassWithIgnoreAttribute
        {
            Property1 = "Hello",
            Property2 = 123,
            Property3 = true
        };

        var toObj = fromObj.MorphTo<DataClass.ToClass>();

        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<DataClass.ToClass>();
        await Assert.That(toObj.Property1).IsEqualTo(string.Empty); // Ignored
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }

    [Test]
    public async Task Given_TypeWithDefaultValueAttributeOnFromClass_When_MorphTo_Then_DefaultValueIsSet()
    {
        var fromObj = new FromClassWithDefaultValueAttribute
        {
            Property1 = null,
            Property2 = 123,
            Property3 = true
        };

        var toObj = fromObj.MorphTo<DataClass.ToClass>();

        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<DataClass.ToClass>();
        await Assert.That(toObj.Property1).IsEqualTo("Default Value"); // Default value
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }

    [Test]
    public async Task Given_TypeWithCustomMappingAttributeOnFromClass_When_MorphTo_Then_CustomMappingIsApplied()
    {
        var fromObj = new FromClassWithCustomMappingAttribute
        {
            Property1 = "Hello",
            Property2 = 123,
            Property3 = true
        };

        var toObj = fromObj.MorphTo<DataClass.ToClass>();

        await Assert.That(toObj).IsNotNull();
        await Assert.That(toObj).IsTypeOf<DataClass.ToClass>();
        await Assert.That(toObj.Property1).IsEqualTo("Mapped: Hello"); // Custom mapping
        await Assert.That(toObj.Property2).IsEqualTo(123);
        await Assert.That(toObj.Property3).IsEqualTo(true);
    }
    
    private class FromClassWithIgnoreAttribute
    {
        [IgnoreProperty]
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }

    private class FromClassWithDefaultValueAttribute
    {
        [MorphToDefaultValue("Default Value")]
        public string? Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }

    private class FromClassWithCustomMappingAttribute
    {
        [MorphToCustomMapping(nameof(MapProperty1))]
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }

        private string MapProperty1(object value)
        {
            return $"Mapped: {value}";
        }
    }

    private class TestClass
    {
        public string PropertyMissMatch { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }

    private class TestClassWithAttribute
    {
        [MorphToProperty("Property1")]
        public string PropertyMissMatch { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }
    
    private class ClassWithIgnoreAttribute
    {
        [IgnoreProperty]
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }

    private class ClassWithDefaultValueAttribute
    {
        [MorphToDefaultValue("Default Value")]
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }

    private class ClassWithCustomMappingAttribute
    {
        [MorphToCustomMapping(nameof(MapProperty1))]
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }

        private string MapProperty1(object value)
        {
            return $"Mapped: {value}";
        }
    }
}