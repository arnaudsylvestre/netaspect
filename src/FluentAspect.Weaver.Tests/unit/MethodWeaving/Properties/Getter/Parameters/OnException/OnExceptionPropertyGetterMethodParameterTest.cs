﻿using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException
{
    [TestFixture]
    public class OnExceptionPropertyGetterMethodParameterTest
    {
       [Test]
       public void CheckMethodReferenced()
        {
            throw new NotImplementedException();
       }

        [Test]
        public void CheckMethodBadType()
       {
           throw new NotImplementedException();
        }


       [Test]
        public void CheckMethodWithRealType()
        {
            throw new NotImplementedException();
        }
   }
}