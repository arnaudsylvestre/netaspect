﻿using System;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException
{
    [TestFixture]
    public class OnExceptionMethodInstanceParameterTest
    {
       [Test]
       public void CheckInstanceReferenced()
        {

           throw new NotImplementedException();
       }

        [Test]
        public void CheckInstanceBadType()
       {
           throw new NotImplementedException();
           
        }

       [Test]
       public void CheckInstanceWithObjectType()
        {
            throw new NotImplementedException();
          
       }


       [Test]
        public void CheckInstanceWithRealType()
       {
           throw new NotImplementedException();
           
        }
   }
}