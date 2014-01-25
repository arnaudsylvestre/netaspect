using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.Core.Model;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After
{
    [TestFixture]
    public class AfterMethodParametersParameterTest
    {
       [Test]
       public void CheckParametersReferenced()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                aspect.AddAfterInterceptor()
                   .WithReferencedParameter<object[]>("parameters")
                   .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                   .WithReturn()
                   .WithAspect(aspect);

            })
           .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'parameters' in the method After of the type 'A.MyAspectAttribute'")))
                   .AndLaunchTest();
          
       }

        [Test]
        public void CheckParametersBadType()
       {
           DoUnit2.Test().ByDefiningAssembly(assembly =>
           {
               var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
               var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
               aspect.AddAfterInterceptor()
                  .WithParameter<int>("parameters")
                  .WithReturn();
               myClassToWeave.AddMethod("MyMethodToWeave")
                  .WithReturn()
                  .WithAspect(aspect);

           })
           .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the parameters parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object[]")))
                   .AndLaunchTest();
           
        }

       [Test]
       public void CheckParametersWithRealType()
        {
            DoUnit2.Test().ByDefiningAssembly(assembly =>
            {
                var myClassToWeave = assembly.AddClass("MyClassToWeave").WithDefaultConstructor();
                var aspect = assembly.AddDefaultAspect("MyAspectAttribute");
                var netAspectInterceptor = aspect.AddAfterInterceptor();
                netAspectInterceptor
                   .WithParameter<object[]>("parameters");
                netAspectInterceptor
                    .WithReturn();
                myClassToWeave.AddMethod("MyMethodToWeave")
                    .WithParameter<int>("first")
                   .WithReturn()
                   .WithAspect(aspect);

            })
                   .AndEnsureAssembly(assemblyP =>
                   {
                       var o = assemblyP.CreateObject("MyClassToWeave");
                       o.CallMethod("MyMethodToWeave", 12);
                       Assert.AreEqual(new object[]
                           {
                               12
                           }, assemblyP.GetStaticFieldValue("MyAspectAttribute", "AfterparametersField"));

                   })
                   .AndLaunchTest();
          
       }
   }
}