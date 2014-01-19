using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before
{
    [TestFixture]
    public class BeforeUpdateFieldParameterInCallerParameterTest 
    {
         [Test]
         public void CheckUpdateFieldWithParameterInCallerAndNoDebuggingInformation()
        {
            throw new NotImplementedException();
          //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
          //       .ByDefiningAssembly(simpleClassAndWeaver =>
          //          {
          //             simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
          //                .WithParameter<int>("parameterInCaller");
          //          })
          //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
          //        .AndEnsureAssembly((assembly, actual) =>
          //            {
          //                var caller = actual.CreateCallerObject();
          //                actual.CallCallerMethod(caller);
          //                Assert.AreEqual(0, actual.Aspect.BeforeUpdateFieldValueParameterInCaller);
          //            })
          //        .AndLaunchTest();
         }

         [Test]
         public void CheckUpdateFieldWithParameterInCallerAndDebuggingInformation()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Warnings.Add("The parameter parameterInCaller in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.AreEqual(0, actual.Aspect.BeforeUpdateFieldValueParameterInCaller);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdateFieldWithParameterInCallerAndNoDebuggingInformationAndWrongType()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithParameter<string>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeUpdateFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }


         [Test]
         public void CheckUpdateFieldWithParameterInCallerAndNoDebuggingInformationAndReferenced()
         {
             throw new NotImplementedException();
             //DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
             //       .ByDefiningAssembly(simpleClassAndWeaver =>
             //       {
             //           simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
             //              .WithReferencedParameter<int>("parameterInCaller");
             //       })
             //          .EnsureErrorHandler(e => e.Errors.Add("The parameter parameterInCaller in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
             //        .AndEnsureAssembly((assembly, actual) =>
             //        {
             //            var caller = actual.CreateCallerObject();
             //            actual.CallCallerMethod(caller);
             //            Assert.False(actual.Aspect.BeforeUpdateFieldValuePassed);
             //        })
             //        .AndLaunchTest();
         }

    }
}