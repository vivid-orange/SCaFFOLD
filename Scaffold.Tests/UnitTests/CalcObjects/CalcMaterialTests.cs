using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.Enums;

namespace Scaffold.Tests.UnitTests.CalcObjects
{
    public class CalcMaterialTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var material = new CalcEnConcreteMaterial(EnConcreteGrade.C90_105, NationalAnnex.UnitedKingdom, "ConcreteMat", "C90/105");
            string json = $@"
{{
  ""$type"": ""Scaffold.Core.CalcObjects.Materials.StandardMaterials.En.CalcEnConcreteMaterial, Scaffold.Core"",
  ""DisplayName"": ""Foo"",
  ""Symbol"": ""M"",
  ""Status"": ""Fail"",
}}";

            // Act & Assert
            Assert.False(material.TryParse("invalid"));
            Assert.True(material.TryParse(json));
            Assert.Equal("Foo", material.DisplayName);
            Assert.Equal("M", material.Symbol);
            Assert.Equal(CalcStatus.Fail, material.Status);
            Assert.Equal(StandardBody.EN, material.Standard.Body);
            Assert.Equal(MaterialType.Concrete, material.Type);
        }

        [Fact]
        public void ParseFromMinimalStringTest()
        {
            // Arrange
            var material = new CalcEnConcreteMaterial(EnConcreteGrade.C90_105, NationalAnnex.UnitedKingdom, "ConcreteMat", "C90/105");
            string json = $@"{{""Grade"": ""C30_37""}}";

            // Act & Assert
            Assert.False(material.TryParse("invalid"));
            Assert.True(material.TryParse(json));
            Assert.Equal(EnConcreteGrade.C30_37, material.Grade);

            Assert.Equal(StandardBody.EN, material.Standard.Body);
            Assert.Equal(MaterialType.Concrete, material.Type);
        }
    }
}
