using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Commons.Collections;
using NetAspect.Doc.Builder.Resources;
using NVelocity;
using NVelocity.App;

namespace NetAspect.Doc.Builder
{
    public class DocBuilder
   {
      private readonly Documentation documentation = new Documentation();

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


         public List<Possibility> InstructionWeavings
         {
            get { return Possibilities.Where(p => p.Group == "InstructionWeaving").ToList(); }
         }

         public List<Possibility> MethodWeavings
         {
            get { return Possibilities.Where(p => p.Group == "MethodWeaving").ToList(); }
         }

         public List<Possibility> ParameterWeavings
         {
            get { return Possibilities.Where(p => p.Group == "ParameterWeaving").ToList(); }
         }

         public List<Parameter> InstructionWeavingParameters
         {
            get { return AvailableParameters.Where(p => p.Kind == "InstructionWeaving").ToList(); }
         }

         public List<Parameter> MethodWeavingParameters
         {
            get { return AvailableParameters.Where(p => p.Kind == "MethodWeaving").ToList(); }
         }

         public List<Parameter> ParameterWeavingParameters
         {
            get { return AvailableParameters.Where(p => p.Kind == "ParameterWeaving").ToList(); }
         }

         public List<Possibility> Possibilities { get; set; }
      }
   }
}
