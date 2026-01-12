using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;
using VividOrange.Geometry;
using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Standards.Eurocode;

namespace Scaffold.Tests.UnitTests
{
    /// <summary>
    /// Automated test that will check any public class 
    /// with a constructor initialises its public properties to non-nullable values.
    /// </summary>
    public class AutomatedConstructorTests
    {
        private static List<string> ignoredClassNames = new List<string>
        {
            "CalcVariableCase",
            "CalcPerimeterProfile",
            "CalcConcreteSection",
            "CalcConcreteSectionProperties",
            "CalcSection",
            "CalcSectionProperties"
        };
        // static inputs used to populate constructor variables
        private static string _string { get { return "lava"; } }
        private static int _int { get { return 1; } }
        private static double _double { get { return 9.8; } }
        private static bool _bool { get { return true; } }
        private static byte _byte { get { return 3; } }
        private static Length _length { get { return new Length(2.5, LengthUnit.Centimeter); } }
        private static UnitsNet.Angle _angle { get { return new UnitsNet.Angle(33, AngleUnit.Degree); } }
        private static Pressure _stress { get { return new Pressure(45, PressureUnit.Megapascal); } }
        private static Strain _strain { get { return new Strain(7.5, StrainUnit.Percent); } }
        private static Dictionary<string, IMaterial> _materials = new Dictionary<string, IMaterial>()
        {
            {
                "Rebar", new EnRebarMaterial(EnRebarGrade.B500B, NationalAnnex.RecommendedValues)
            },
            {
                "Concrete", new EnConcreteMaterial(EnConcreteGrade.C30_37, NationalAnnex.RecommendedValues)
            },
            {
                "Steel", new EnSteelMaterial(EnSteelGrade.S355, NationalAnnex.RecommendedValues)
            }
        };
        private static IProfile _profile { get { return new HE320B(); } }
        private static ILocalPoint2d _point { get { return new LocalPoint2d(); } }
        private static ILocalPolyline2d _line { get { return new LocalPolyline2d(new List<ILocalPoint2d>() { new LocalPoint2d(), new LocalPoint2d() }); } }
        private static NationalAnnex _na { get { return NationalAnnex.UnitedKingdom; } }

        private Type _currentType { get; set; }

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void ConstructorTest(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            _currentType = type;
            foreach (ConstructorInfo constructor in constructors)
            {
                object[] args = CreateConstructorArguments(constructor);
                object instance = constructor.Invoke(args);
                Assert.NotNull(instance);
                TestObjectsPropertiesAreNotNull(instance);
            }
        }

        public class TestDataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = GetAllClassesOf<ICalcValue>();

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private static List<object[]> GetAllClassesOf<T>()
            {
                var data = new List<object[]>();
                Type[] typelist = Assembly.GetAssembly(typeof(CalcBool)).GetTypes()
                    .Where(p => typeof(T).IsAssignableFrom(p)
                    && !p.IsAbstract && !p.ContainsGenericParameters).ToArray();
                foreach (Type type in typelist)
                {
                    if (type.Namespace == null || ignoredClassNames.Contains(type.Name))
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

        private void TestObjectsPropertiesAreNotNull(object obj)
        {
            PropertyInfo[] propertyInfo = obj.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (PropertyInfo property in propertyInfo)
            {
                if (property.CanRead && !property.CanWrite)
                {
                    Assert.NotNull(property.GetValue(obj));
                }
                else if (property.CanRead && property.CanWrite)
                {
                    object value = property.GetValue(obj);
                    if (value != null)
                    {
                        Assert.NotNull(value);
                    }
                    else
                    {
                        object newValue = CreateObjectInstance(property.PropertyType);
                        property.SetValue(obj, newValue, null);
                        Assert.NotNull(property.GetValue(obj));
                    }
                }
            }
        }

        private object[] CreateConstructorArguments(ConstructorInfo constructor)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            var args = new List<object>();
            foreach (ParameterInfo parameter in parameters)
            {
                Type type = parameter.ParameterType;

                if (type.IsEnum)
                {
                    var enums = Enum.GetNames(type);
                    args.Add(Enum.Parse(type, enums[0]));
                    continue;
                }

                // check if parameter is primitive type
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Int32:
                        args.Add(_int);
                        continue;

                    case TypeCode.String:
                        args.Add(_string);
                        continue;

                    case TypeCode.Double:
                        args.Add(_double);
                        continue;

                    case TypeCode.Boolean:
                        args.Add(_bool);
                        continue;

                    case TypeCode.Byte:
                        args.Add(_byte);
                        continue;
                }

                // check if type is IQuanty type
                if (typeof(IQuantity).IsAssignableFrom(type))
                {
                    switch (type.Name)
                    {
                        case "Length":
                            args.Add(_length);
                            continue;

                        case "Angle":
                            args.Add(_angle);
                            continue;

                        case "Pressure":
                            args.Add(_stress);
                            continue;

                        case "Strain":
                            args.Add(_strain);
                            continue;
                    }
                }

                // check if type is IList type
                Type[] genericArguments = type.GetGenericArguments();
                if (genericArguments.Length == 1)
                {
                    Type listType = typeof(IList<>).MakeGenericType(genericArguments);
                    dynamic list = CreateGenericList(typeof(List<>), genericArguments[0]);
                    dynamic objectInstance = CreateObjectInstance(genericArguments[0]);
                    list.Add(objectInstance);
                    args.Add(list);
                    continue;
                }

                if (type.Name == "IMaterial")
                {
                    if (_currentType.Name.Contains("Rebar") || _currentType.Name.Contains("Link"))
                    {
                        args.Add(_materials["Rebar"]);
                    }
                    else if (_currentType.Name.Contains("Reinf"))
                    {
                        args.Add(_materials["Rebar"]);
                    }
                    else if (_currentType.Name.Contains("Concrete"))
                    {
                        args.Add(_materials["Concrete"]);
                    }
                    else if (_currentType.Name.Contains("Steel"))
                    {
                        args.Add(_materials["Steel"]);
                    }
                    else
                    {
                        throw new Exception("Unknown material required in " + _currentType.Name);
                    }
                    continue;
                }

                if (type.Name == "IProfile")
                {
                    args.Add(_profile);
                    continue;
                }

                if (type.Name == "ILocalPoint2d")
                {
                    args.Add(_point);
                    continue;
                }

                if (type.Name == "ILocalPolyline2d")
                {
                    args.Add(_line);
                    continue;
                }

                if (type.Name == "T" && type.FullName == null)
                {
                    args.Add(_length);
                    continue;
                }

                // use the types constructor
                args.Add(CreateObjectInstance(type));
            }

            return args.ToArray();
        }

        private dynamic CreateObjectInstance(Type type)
        {
            if (type.IsInterface == false)
            {
                ConstructorInfo[] ctor = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                if (ctor.Length == 0)
                {
                    ctor = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                }

                object[] args = CreateConstructorArguments(ctor[0]);
                return ctor[0].Invoke(args);
            }

            // reflect on assembly to find concrete implementation of interface
            AssemblyName[] referencedNames = Assembly.GetAssembly(typeof(CalcBool)).GetReferencedAssemblies();
            List<Assembly> assemblies = new List<Assembly>()
            {
                Assembly.GetAssembly(typeof(CalcBool))
            };
            assemblies.AddRange(referencedNames.Select(a => Assembly.Load(a)).ToList());

            foreach (Assembly assembly in assemblies)
            {
                if (!assembly.FullName.StartsWith("VividOrange."))
                {
                    continue;
                }

                Type[] typelist = assembly.GetTypes();
                foreach (Type concreteType in typelist)
                {
                    if (concreteType.GetInterfaces().Contains(type) && concreteType.IsInterface == false)
                    {
                        return CreateObjectInstance(concreteType);
                    }
                }
            }

            throw new NotImplementedException($"Could not find concrete implementation of type {type}");
        }

        public static dynamic CreateGenericList(Type generic, Type innerType, params object[] args)
        {
            Type specificType = generic.MakeGenericType(new Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }
    }
}
