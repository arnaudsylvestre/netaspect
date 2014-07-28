﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NetAspect.Doc.Builder.Resources;

namespace NetAspect.Doc.Builder
{
    public class DocBuilder
    {
        public class Documentation
        {
            public Documentation()
            {
                Possibilities = new List<Possibility>();
            }

            public string Header { get; set; }
            public string Footer { get; set; }
            
            public string Basics { get; set; }

            public string MethodWeaving { get; set; }

            public string MethodWeavingPossibilities { get; set; }

            public List<Possibility> Possibilities { get; set; }
        }

        
        private Documentation documentation = new Documentation();

        public DocBuilder()
        {
            documentation.Header = Content.Header;
            documentation.Basics = Content.Basics;
            documentation.MethodWeaving = Content.MethodWeaving;
        }


        public void Add(Possibility possibility)
        {
            documentation.Possibilities.Add(possibility);
        }

        public void Generate(string filePath)
        {
            var velocity = new VelocityEngine();
            var hashtable = new Hashtable();
            hashtable.Add("documentation", documentation);
            var context = new VelocityContext(hashtable);
            var props = new ExtendedProperties();
            velocity.Init(props);
            documentation.MethodWeavingPossibilities = BuildMethodWeavingPossibilities(velocity, context);

            using (var streamWriter = new StreamWriter(File.Create(filePath)))
                velocity.Evaluate(context, streamWriter, "", Templates.Templates.Documentation);
        }

        private static string BuildMethodWeavingPossibilities(VelocityEngine velocity, VelocityContext context)
        {
            var stringWriter = new StringWriter();
            velocity.Evaluate(context, stringWriter, "", Templates.Templates.WeavingPossibilities);

            var methodWeavingPossibilities = stringWriter.ToString();
            return methodWeavingPossibilities;
        }
    }

    public class Possibility
    {
        public Possibility()
        {
            Events = new List<PossibilityEvent>();
        }

        public string Kind { get; set; }
        public string Description { get; set; }
        public List<PossibilityEvent> Events { get; set; }

        public string Title { get; set; }

        public string Member { get; set; }
    }

    public class PossibilityEvent
    {
        public PossibilityEvent()
        {
            Parameters = new List<Parameter>();
            Samples = new List<Sample>();
        }

        public string Kind { get; set; }
        public string Description { get; set; }
        public string MethodName { get; set; }
        public string Called { get; set; }

        public List<Parameter> Parameters { get; set; }

        public List<Sample> Samples { get; set; } 
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}