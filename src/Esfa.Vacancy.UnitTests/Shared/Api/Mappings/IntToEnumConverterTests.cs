using System.Reflection;
using AutoMapper;
using Esfa.Vacancy.Register.Api.Mappings;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.Shared.Api.Mappings
{
    [TestFixture]
    public class IntToEnumConverterTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<int?, Gender>().ConvertUsing(new IntToEnumConverter<Gender>());
                cfg.AddProfiles(Assembly.GetExecutingAssembly());
            });
        }

        [Test]
        public void MappingDefaultValuesTests()
        {
            var domainObject = new DomainType();
            //Act
            var result = Mapper.Map<ViewModel>(domainObject);
            //Assert
            Assert.AreEqual(result.Gender1, Gender.Unknown);
            Assert.AreEqual(result.Gender2, Gender.Unknown);
        }

        [Test]
        public void MappingSpecificValuesTests()
        {
            var domainObject = new DomainType() { Gender1 = 1, Gender2 = 2 };
            //Act
            var result = Mapper.Map<ViewModel>(domainObject);
            //Assert
            Assert.AreEqual(result.Gender1, Gender.Female);
            Assert.AreEqual(result.Gender2, Gender.Male);
        }
    }

    public class ViewModelMappings : Profile
    {
        public ViewModelMappings()
        {
            CreateMap<DomainType, ViewModel>();
        }
    }

    public enum Gender
    {
        Unknown = 0,
        Female = 1,
        Male = 2
    }

    public class DomainType
    {
        public int Gender1 { get; set; }

        public int? Gender2 { get; set; }
    }

    public class ViewModel
    {
        public Gender Gender1 { get; set; }

        public Gender Gender2 { get; set; }
    }

}
