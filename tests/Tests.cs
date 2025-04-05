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

    private class TestClass
    {
        public string PropertyMissMatch { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }

    private class TestClassWithAttribute
    {
        [MorphPropertyName("Property1")]
        public string PropertyMissMatch { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
    }
}