using System;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After
{
    [TestFixture]
    public class AfterPropertyUpdaterInstanceParameterTest
    {
       [Test]
       public void CheckInstanceReferenced()
        {
            DoUnit.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterPropertySetInterceptor();
                netAspectInterceptor
                   .WithReferencedParameter<object>("instance");
                netAspectInterceptor
                    .WithReturn();
                var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                    .WithAspect(aspect);
                propertyDefinition.AddSetMethod()
                   .WithReturn()
                   ;
                        
}).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method AfterPropertySet of the type 'A.MyAspectAttribute'")))
           
                      .AndLaunchTest();
          
       }

        [Test]
        public void CheckInstanceBadType()
       {
           DoUnit.Test().ByDefiningAssembly(assembly =>
           {
               var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
               var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
               var netAspectInterceptor = aspect.AddAfterPropertySetInterceptor();
               netAspectInterceptor
                  .WithParameter<int>("instance");
               netAspectInterceptor
                   .WithReturn();
               var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                   .WithAspect(aspect);
               propertyDefinition.AddSetMethod()
                  .WithReturn()
                  ;
           }).EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method AfterPropertySet of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave")))
           .AndLaunchTest();
           
        }

       [Test]
       public void CheckInstanceWithObjectType()
        {
            DoUnit.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterPropertySetInterceptor();
                netAspectInterceptor
                   .WithParameter<object>("instance");
                netAspectInterceptor
                    .WithReturn();
                var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                    .WithAspect(aspect);
                propertyDefinition.AddSetMethod()
                   .WithReturn()
                   ;

            })
                   .AndEnsureAssembly(assemblyP =>
                   {

                       var o = assemblyP.CreateObject("MyClassToWeave");
                       o.UpdateProperty("MyProperty", "value");
                       Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterPropertySetinstanceField"));

                   })
                   .AndLaunchTest();
          
       }


       [Test]
        public void CheckInstanceWithRealType()
       {
           DoUnit.Test().ByDefiningAssembly(assembly =>
           {
               var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
               var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
               var netAspectInterceptor = aspect.AddAfterPropertySetInterceptor();
               netAspectInterceptor
                  .WithParameter("instance", myClassToWeave);
               netAspectInterceptor
                   .WithReturn();
               var propertyDefinition = myClassToWeave.AddProperty<string>("MyProperty")
                   .WithAspect(aspect);
               propertyDefinition.AddSetMethod()
                  .WithReturn()
                  ;

           })
                   .AndEnsureAssembly(assemblyP =>
                   {

                       var o = assemblyP.CreateObject("MyClassToWeave");
                       o.UpdateProperty("MyProperty", "value");
                       Assert.AreEqual(o, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterPropertySetinstanceField"));

                   })
                   .AndLaunchTest();
           
        }
   }
}