﻿using NJsonSchema.Converters;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace NJsonSchema.Tests.Generation.SystemTextJson
{
    public class SystemTextJsonInheritanceTests
    {
        public class Apple : Fruit
        {
            public string Foo { get; set; }
        }

        public class Orange : Fruit
        {
            public string Bar { get; set; }
        }

        [JsonInheritance("a", typeof(Apple))]
        [JsonInheritance("o", typeof(Orange))]
        [JsonInheritanceConverter(typeof(Fruit), "k")]
        public class Fruit
        {
            public string Baz { get; set; }
        }

        [Fact]
        public async Task When_using_JsonInheritanceAttribute_and_SystemTextJson_then_schema_is_correct()
        {
            //// Act
            var schema = JsonSchema.FromType<Fruit>();
            var data = schema.ToJson();

            //// Assert
            Assert.NotNull(data);
            Assert.Contains(@"""a"": """, data);
            Assert.Contains(@"""o"": """, data);
            Assert.Contains(@"""discriminator"": {
    ""propertyName"": ""k"",
    ""mapping"": {
      ""a"": ""#/definitions/Apple"",
      ""o"": ""#/definitions/Orange""
    }
  },", data);
        }

#if !NETFRAMEWORK

        public class Apple2 : Fruit2
        {
            public string Foo { get; set; }
        }

        public class Orange2 : Fruit2
        {
            public string Bar { get; set; }
        }

        [JsonDerivedType(typeof(Apple2), "a")]
        [JsonDerivedType(typeof(Orange2), "o")]
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "k")]
        public class Fruit2
        {
            public string Baz { get; set; }
        }

        [Fact]
        public async Task When_using_native_attributes_in_SystemTextJson_then_schema_is_correct()
        {
            //// Act
            var schema = JsonSchema.FromType<Fruit2>();
            var data = schema.ToJson();

            //// Assert
            Assert.NotNull(data);
            Assert.Contains(@"""a"": """, data);
            Assert.Contains(@"""o"": """, data);
            Assert.Contains(@"""discriminator"": {
    ""propertyName"": ""k"",
    ""mapping"": {
      ""a"": ""#/definitions/Apple2"",
      ""o"": ""#/definitions/Orange2""
    }
  },", data);
        }

#endif
    }
}
