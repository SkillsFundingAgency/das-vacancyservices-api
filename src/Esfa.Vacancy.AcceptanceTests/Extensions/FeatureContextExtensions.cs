using Microsoft.Owin.Testing;
using TechTalk.SpecFlow;

namespace Esfa.Vacancy.AcceptanceTests.Extensions
{
    public static class FeatureContextExtensions
    {
        private const string TestServerKey = "TestServer";

        public static TestServer TestServer(this FeatureContext source)
        {
            return source.Get<TestServer>(TestServerKey);
        }

        public static void TestServer(this FeatureContext source, TestServer testServer)
        {
            source.Add(TestServerKey, testServer);
        }
    }
}