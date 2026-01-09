using MagmaWorks.Taxonomy.Standards;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.Enums;

namespace Scaffold.Tests.UnitTests.CalcObjects
{
    public class CalcObjectWrapperTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var standard = new CalcObjectWrapper<En1992>(new En1992(En1992Part.Part1_1, NationalAnnex.UnitedKingdom), "BS EN 1992-1-1", "EC2");
            string fromJson = standard.GetValueAsString();
            string json =
                "{\"$type\": \"Scaffold.Core.CalcObjects.CalcObjectWrapper`1[[" +
                "MagmaWorks.Taxonomy.Standards.Eurocode.En1992, MagmaWorks.Taxonomy.Standards]], " +
                "Scaffold.Core\", \"DisplayName\": \"DS EN 1992-1-1\", \"Symbol\": \"EN2\", \"Status\": \"Fail\"," +
                "\"Value\": {\"$type\": \"MagmaWorks.Taxonomy.Standards.Eurocode.En1992, MagmaWorks.Taxonomy.Standards\"," +
                "\"Body\": \"EN\", \"Part\": \"Part1_2\", \"NationalAnnex\": \"Denmark\"}}";

            // Act & Assert
            Assert.False(standard.TryParse("invalid"));
            Assert.True(standard.TryParse(json));
            Assert.Equal("DS EN 1992-1-1", standard.DisplayName);
            Assert.Equal("EN2", standard.Symbol);
            Assert.Equal(CalcStatus.Fail, standard.Status);
            Assert.Equal(StandardBody.EN, standard.Value.Body);
            Assert.Equal("DS EN 1992-1-2: Eurocode 2: Design of Concrete Structures - Part 1-2: General rules - Structural fire design", standard.Value.Title);
        }
    }
}
