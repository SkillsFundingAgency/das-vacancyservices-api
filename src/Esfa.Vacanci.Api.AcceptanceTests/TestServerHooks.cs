using Microsoft.Owin.Testing;
using Esfa.Vacancy.Api.AcceptanceTests.Extensions;
using TechTalk.SpecFlow;

namespace Esfa.Vacancy.Api.AcceptanceTests
{
    [Binding]
    public sealed class TestServerHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeFeature]
        public static void BeforeFeature()
        {
            var testServer = TestServer.Create<TestStartup>();
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
