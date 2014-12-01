using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples.Methods
{
   public class TransactionalSampleTest :
      NetAspectTest<TransactionalSampleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
             {
                 TransactionalAttribute.SetSession(new FakeSession());
                 var classToWeave = new ClassToWeave();
                 classToWeave.Divide(12, 3);
                 Assert.True(FakeTransation.IsCommited);
                 Assert.False(FakeTransation.IsRollback);
                 
             };
      }

      

      public class ClassToWeave
      {
          [Transactional] 
         public int Divide(int value, int divideBy)
          {
              return value/divideBy;
          }
      }

       public class NHibernate
       {

           public interface ISession
           {
               ITransaction BeginTransaction();
           }

           public interface ITransaction
           {
               void Commit();
               void Rollback();
           }
            
       }

       class FakeSession : NHibernate.ISession
       {
           public NHibernate.ITransaction BeginTransaction()
           {
               return new FakeTransation();
           }
       }

       class FakeTransation : NHibernate.ITransaction
       {
           public static bool IsCommited { get; set; }
           public static bool IsRollback { get; set; }

           public void Commit()
           {
               IsCommited = true;
           }

           public void Rollback()
           {
               IsRollback = true;
           }
       }

      public class TransactionalAttribute : Attribute
      {
          private static NHibernate.ISession session;

          public bool NetAspectAttribute = true;
          private NHibernate.ITransaction transaction;

          public static void SetSession(NHibernate.ISession newSession)
          {
              session = newSession;
          }

          public void AfterMethod()
          {
              transaction.Commit();
          }

          public void BeforeMethod()
          {
              transaction = session.BeginTransaction();
          }

          public void OnExceptionMethod()
          {
              transaction.Rollback();
          }
      }
   }
}
