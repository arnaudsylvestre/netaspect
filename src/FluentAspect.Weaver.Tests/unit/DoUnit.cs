using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit
{
    public class UnitTest
    {
        [Test]
        public void Check()
        {
            DoUnit2.DoAcceptanceConfiguration.Run<ClassToWeave>(e => { }, a =>
                {

                });
        }
    }

    public class ClassToWeave
    {
        [MyAspect]
        public void Weaved()
        {
            
        }
    }

    public class MyAspect : Attribute
    {
        public bool IsNetAspectAttribute = true;

        public void After(object instance)
        {
            
        }


    }
}
