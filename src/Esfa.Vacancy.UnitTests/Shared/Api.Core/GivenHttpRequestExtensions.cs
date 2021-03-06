﻿using System.Net.Http;
using Esfa.Vacancy.Api.Core.Extensions;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.UnitTests.Shared.Api.Core
{
    [TestFixture]
    public class GivenHttpRequestExtensions
    {
        [TestCase("x-request-context-user-id", "JohnSerco", true, TestName = "And the key is valid then it is included")]
        [TestCase("user-email", "JohnSerco@hotmail.com", false, TestName = "And the key is unknown then it is not included")]
        [TestCase("x-request-context-user-note", null, true, TestName = "And the header value is null then it is included")]
        public void WhenGetApimUserContextHeaders(string key, string value, bool included)
        {
            var request = new HttpRequestMessage();

            request.Headers.Add(key, value);

            var result = request.GetApimUserContextHeaders();

            result.ContainsKey(key).Should().Be(included);
        }

        [TestCase("TestHeader1", "Value", TestName = "And Header exists then return value of the header")]
        [TestCase("TestHeader2", null, TestName = "And Header doesn't exists then return null")]
        public void WhenGetHeaderValue(string key, string value)
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("TestHeader1", "Value");

            request.GetHeaderValue(key).Should().Be(value);
        }
    }
}