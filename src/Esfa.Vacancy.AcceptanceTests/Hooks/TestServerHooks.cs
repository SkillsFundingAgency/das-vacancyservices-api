using Esfa.Vacancy.AcceptanceTests.Extensions;
using Microsoft.Owin.Testing;
using TechTalk.SpecFlow;

namespace Esfa.Vacancy.AcceptanceTests.Hooks
{
    [Binding]
    public sealed class TestServerHooks
    {
        [BeforeFeature]
        public static void BeforeFeature()
        {
            var testServer = TestServer.Create<TestStartUp>();
            FeatureContext.Current.TestServer(testServer);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            var testServer = FeatureContext.Current.TestServer();
            testServer.Dispose();
        }
    }
}