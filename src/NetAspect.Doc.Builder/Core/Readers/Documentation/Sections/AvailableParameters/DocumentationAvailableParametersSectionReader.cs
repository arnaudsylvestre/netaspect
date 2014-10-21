using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetAspect.Doc.Builder.Core.Readers.Core;
using NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation
{
    public static class DocumentationAvailableParametersSectionReader
    {
        public static AvailableParametersSectionModel ExtractAvailableParameters(string baseFolder)
        {
            var doc = new List<ParameterModel>();
            foreach (string file_L in baseFolder.GetAllCsFiles())
            {
                var test = CsTestFileReader.Read(file_L);

                ParameterModel model = doc.FirstOrDefault(p => p.Name == test.Parameters[0]);
                // TODO : Voir pourquoi il peut ne pas avoir de paramètres
                if (model == null)
                {
                    model = new ParameterModel
                    {
                        Name = test.Parameters[0],
                        Samples = new List<ParameterModel.ParameterSample>()
                    };
                    doc.Add(model);
                }
                model.Samples.Add(
                    new ParameterModel.ParameterSample
                    {
                        Description = test.When,
                        AspectCode = test.AspectCode,
                        CallCode = test.CallCode,
                        ClassToWeaveCode = test.ClassToWeaveCode,
                    });

            }


            return new AvailableParametersSectionModel(doc);
        }
    }
}