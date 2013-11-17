﻿using NUnit.Framework;
using SheepAspect.Compile;
using FluentAssertions;
using SheepAspect.UnitTest.TestHelper;

namespace SheepAspect.UnitTest.ConfigurationTests
{
    [TestFixture]
    public class XmlConfigurationTest
    {
        private XmlCompilerSettings _config;

        [SetUp]
        public void ReadXml()
        {
            _config = new XmlCompilerSettings("ConfigurationTests/TestCfg.xml");
        }

        [Assert]
        public void CanReturnCorrectNumberOfAspectFiles()
        {
            _config.AspectAssemblyFiles.Should().HaveCount(6);
        }

        [Assert]
        public void ParseAspectFiles()
        {
            _config.AspectAssemblyFiles
                .Should().Contain("aspect1.dll")
                .And.Contain("aspect2.dll");
        }

        [Assert]
        public void ParseAspectDirs()
        {
            _config.AspectAssemblyFiles
                .Should().Contain(@"aspectDir1\blah1*.dll")
                .And.Contain(@"aspectDir1\blah1*.exe")
                .And.Contain(@"aspectDir2/blah2*.dll")
                .And.Contain(@"aspectDir2/blah2*.exe");
        }

        [Assert]
        public void CanReturnCorrectNumberOfTargetFiles()
        {
            _config.TargetAssemblyFiles.Should().HaveCount(6);
        }

        [Assert]
        public void ParseTargetFiles()
        {
            _config.TargetAssemblyFiles
                .Should().Contain("weave1.dll")
                .And.Contain("weave2.dll");
        }

        [Assert]
        public void ParseTargetDirs()
        {
            _config.TargetAssemblyFiles
                .Should().Contain(@"weaveDir1\blah1*.dll")
                .And.Contain(@"weaveDir1\blah1*.exe")
                .And.Contain(@"weaveDir2/blah2*.dll")
                .And.Contain(@"weaveDir2/blah2*.exe");
        }

    }
}