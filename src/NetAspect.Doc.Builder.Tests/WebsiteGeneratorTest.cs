﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using NetAspect.Doc.Builder.Core;
using NetAspect.Doc.Builder.Core.GettingStarted;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Tests
{
    [TestFixture]
    public class WebsiteGeneratorTest
    {

        [Test]
        public void CheckGenerate()
        {
            var configuration = new DocumentationConfiguration();
            configuration.InterceptorKinds = new List<InterceptorKind>()
                {
                    new InterceptorKind()
                        {
                            Name = "Method",
                            Configurations = new List<InterceptorKindConfiguration>
                {
                    new InterceptorKindConfiguration()
                        {
                            Title = "Title",
                            Parameters = new List<ParameterConfiguration>
                                {
                                    new ParameterConfiguration()
                                        {
                                            Name = "name",
                                            Description = "description"
                                        }
                                },
                            Interceptors = new List<InterceptorConfiguration>
                                {
                                    new InterceptorConfiguration()
                                        {
                                            MethodName = "Method",
                                            When = "Hello"
                                        }
                                }
                        }
                }}};
            var serializer = new XmlSerializer(typeof(DocumentationConfiguration));
            using (var file = File.Create(@"C:\Documentation.xml"))
            {
                serializer.Serialize(file, configuration);
                
            }
        }

        [Test]
         public void CheckWebsiteGeneration()
        {
            string baseDirectory = @"D:\Developpement\fluentaspect\";
            var tests = baseDirectory + @"web\Tests";
            //var gettingStartedPage1 = baseDirectory +
            //                          @"src\FluentAspect.Weaver.Tests\docs\GettingStarted\GettingStartedPart1Test.cs";
            //var gettingStartedPage2 = baseDirectory +
            //                          @"src\FluentAspect.Weaver.Tests\docs\GettingStarted\GettingStartedPart2Test.cs";
            // var generator = new WebsiteGenerator();

            //var website = new WebSite();
            //website.Pages.Add(new HomePage());
            //var gettingStartedPage = new GettingStartedPage();
            //var reader = new GettingStartedReader();
            //reader.Read(gettingStartedPage, gettingStartedPage1, gettingStartedPage2);
            //website.Pages.Add(gettingStartedPage);
            //generator.Generate(website, tests);

            var generator = new WebsiteGenerator();
            var webSite = WebsiteFactory.Create(baseDirectory + @"src\FluentAspect.Weaver.Tests\docs\");
            generator.Generate(webSite, tests);
         }
    }
}