// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using FluentAssertions;
using Microsoft.DotNet.Tools.Test.Utilities;
using Xunit;

namespace Microsoft.DotNet.Cli.Publish3.Tests
{
    public class GivenDotnetPublish3PublishesProjects : TestBase
    {
        [Fact]
        public void ItPublishesARunnablePortableApp()
        {
            var testAppName = "MSBuildTestApp";
            var testInstance = TestAssetsManager
                .CreateTestInstance(testAppName);

            var testProjectDirectory = testInstance.TestRoot;

            new Restore3Command()
                .WithWorkingDirectory(testProjectDirectory)
                .Execute()
                .Should()
                .Pass();

            new Publish3Command()
                .WithWorkingDirectory(testProjectDirectory)
                .Execute("--framework netcoreapp1.0")
                .Should()
                .Pass();

            var configuration = Environment.GetEnvironmentVariable("CONFIGURATION") ?? "Debug";
            var outputDll = Path.Combine(testProjectDirectory, "bin", configuration, "netcoreapp1.0", "publish", $"{testAppName}.dll");

            new TestCommand("dotnet")
                .ExecuteWithCapturedOutput(outputDll)
                .Should()
                .Pass()
                .And
                .HaveStdOutContaining("Hello World");
        }
    }
}
