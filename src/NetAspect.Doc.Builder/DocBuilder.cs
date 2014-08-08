using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NetAspect.Doc.Builder.Resources;

namespace NetAspect.Doc.Builder
{
   public class PossibilityDescription
   {
      public string Kind { get; set; }
      public string Description { get; set; }
      public List<PossibilityEvent> Events { get; set; }

      public string Title { get; set; }

      public string Member { get; set; }
      public string Group { get; set; }
   }

   public class TestDescription
   {
      public TestDescription()
      {
         AspectParameters = new List<string>();
      }

      public string CallCode { get; set; }
      public string AspectCode { get; set; }
      public string ClassToWeaveCode { get; set; }
      public string Possibility { get; set; }
      public string Called { get; set; }
      public string Description { get; set; }
      public string MethodName { get; set; }
      public List<string> AspectParameters { get; set; }
      public string Kind { get; set; }
      public string Member { get; set; }
   }

    public class DocBuilder
    {
        public class Documentation
        {
            public Documentation()
            {
                Possibilities = new List<Possibility>();
                AvailableParameters = new List<Parameter>();
            }

            public List<Parameter> AvailableParameters { get; set; }

            public string Footer { get; set; }
            
            public string Basics { get; set; }
            

            public List<Possibility> InstructionWeavings { get { return Possibilities.Where(p => p.Group == "InstructionWeaving").ToList(); } }
            public List<Possibility> MethodWeavings
            {
                get
                {
                    return Possibilities.Where(p => p.Group == "MethodWeaving").ToList();
                }
            }
            public List<Possibility> ParameterWeavings { get { return Possibilities.Where(p => p.Group == "ParameterWeaving").ToList(); } }
            public List<Parameter> InstructionWeavingParameters { get { return AvailableParameters.Where(p => p.Kind == "InstructionWeaving").ToList(); } }
            public List<Parameter> MethodWeavingParameters { get { return AvailableParameters.Where(p => p.Kind == "MethodWeaving").ToList(); } }
            public List<Parameter> ParameterWeavingParameters { get { return AvailableParameters.Where(p => p.Kind == "ParameterWeaving").ToList(); } }

            public List<Possibility> Possibilities { get; set; }
        }

        
        private Documentation documentation = new Documentation();

        public DocBuilder()
        {
            
            documentation.Basics = Content.Basics;
        }


        public void Add(IEnumerable<Parameter> parameters)
        {
            documentation.AvailableParameters.AddRange(parameters);
        }

        public void Generate(string filePath)
        {
            var velocity = new VelocityEngine();
            var hashtable = new Hashtable();
            hashtable.Add("documentation", documentation);
            var context = new VelocityContext(hashtable);
            var props = new ExtendedProperties();
            velocity.Init(props);

            using (var streamWriter = new StreamWriter(File.Create(filePath)))
                velocity.Evaluate(context, streamWriter, "", Templates.Templates.Documentation);
        }

        public void Add(Possibility possibility)
        {
            documentation.Possibilities.Add(possibility);
        }
    }

    public class Possibility
    {
        public Possibility()
        {
            Events = new List<PossibilityEvent>();
            AvailableParameters = new List<Parameter>();
        }

        public string Kind { get; set; }
        public string Description { get; set; }
        public List<PossibilityEvent> Events { get; set; }
        public List<Parameter> AvailableParameters { get; set; }

        public string Title { get; set; }

        public string Member { get; set; }

        public string Group { get; set; }
    }

    public class PossibilityEvent
    {
        public PossibilityEvent()
        {
            Parameters = new List<Parameter>();
        }

        public string Kind { get; set; }
        public string Description { get; set; }
        public string MethodName { get; set; }
        public string Called { get; set; }

        public List<Parameter> Parameters { get; set; }

        public Sample Sample { get; set; } 
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
    }
}