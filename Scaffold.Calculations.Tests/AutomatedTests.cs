using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Scaffold.Calculations.Eurocode.Concrete;

namespace Scaffold.Calculations.Tests
{
    /// <summary>
    /// Automated test that will check any calculation inheriting from ICalculation
    /// with a constructor initialises its public properties to non-nullable values.
    /// </summary>
    public class AutomatedTestsOfICalcalculation
    {

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void ConstructorTest(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            object instance = constructor.Invoke(null);
            Assert.NotNull(instance);
            TestObjectsPropertiesAreInputOutputAttributesAndNotNull(instance);
            TestObjectsFieldsAreNotInputOutputAttributes(instance);
        }

        public class TestDataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = GetAllClassesOf<ICalculation>();

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private static List<object[]> GetAllClassesOf<T>()
            {
                var data = new List<object[]>();
                Type[] typelist = Assembly.GetAssembly(typeof(ConcreteMaterialProperties)).GetTypes()
                    .Where(p => typeof(T).IsAssignableFrom(p)).ToArray();
                foreach (Type type in typelist)
                {
                    if (type.Namespace == null)
                    {
                        continue;
                    }

                    if (type.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) == null)
                    {
                        data.Add([type]);
                    }
                }

                return data;
            }
        }

        private void TestObjectsPropertiesAreInputOutputAttributesAndNotNull(object obj)
        {
            PropertyInfo[] propertyInfo = obj.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (PropertyInfo property in propertyInfo)
            {
                if (property.Name == "CalculationTitle" || property.Name == "EntityLabel"
                    || property.Name == "Status")
                {
                    continue;
                }

                if (property.SetMethod == null)
                {
                    continue;
                }
                Assert.False(property.GetValue(obj) == null,
                    $"The calculation '{obj}'\ncontains a property '{property.Name}' that is null.");
                CalcParameterAttribute baseAttribute = property.GetCustomAttribute<CalcParameterAttribute>();
                Assert.False(baseAttribute == null,
                    $"The calculation '{obj}' \ncontains a public property '{property.Name}'\n" +
                    $"that has not been decorated with input/output attributes.");
            }
        }

        private void TestObjectsFieldsAreNotInputOutputAttributes(object obj)
        {
            FieldInfo[] fieldInfo = obj.GetType().GetFields(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fieldInfo)
            {
                CalcParameterAttribute baseAttribute = field.GetCustomAttribute<CalcParameterAttribute>();
                Assert.True(baseAttribute == null,
                    $"The calculation '{obj}' \ncontains a public field '{field.Name}'\n" +
                    $"that has been decorated with input/output attributes. \n" +
                    $"Convert this field to a property by adding get/set.");
            }
        }
    }
}
