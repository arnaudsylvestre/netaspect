﻿using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder
{
    internal class InterceptorDocumentationVisitor : DepthFirstAstVisitor
    {
        private readonly InterceptorDocumentation model;

        public InterceptorDocumentationVisitor(InterceptorDocumentation model)
        {
            this.model = model;
        }


        public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
        {
            if (IsWeaved(parameterDeclaration.Attributes))
                model.Member = "parameter";
            base.VisitParameterDeclaration(parameterDeclaration);
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (IsWeaved(propertyDeclaration.Attributes))
                model.Member = "property";
            base.VisitPropertyDeclaration(propertyDeclaration);
        }

        public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            if (IsWeaved(fieldDeclaration.Attributes))
                model.Member = "field";
            base.VisitFieldDeclaration(fieldDeclaration);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            if (constructorDeclaration.Initializer.Arguments.Count == 3)
            {
                List<PrimitiveExpression> arguments = constructorDeclaration.Initializer.Arguments.Cast<PrimitiveExpression>().ToList();
                model.When = GetValue(arguments[0]);
            }
            if (IsWeaved(constructorDeclaration.Attributes))
                model.Member = "constructor";
            base.VisitConstructorDeclaration(constructorDeclaration);
        }


        private static string GetValue(PrimitiveExpression expression)
        {
            return expression.LiteralValue.Replace("\"", "");
        }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            model.CallCode = lambdaExpression.Body.ToString();
            base.VisitLambdaExpression(lambdaExpression);
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration.Name.EndsWith("Attribute"))
            {
                model.AspectCode = typeDeclaration.ToString();
            }
            if (typeDeclaration.Name == "MyInt")
            {
                model.ClassToWeaveCode = typeDeclaration.ToString();
            }
            base.VisitTypeDeclaration(typeDeclaration);
        }

        public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (((TypeDeclaration) methodDeclaration.Parent).Name.EndsWith("Attribute"))
            {
                model.Name = methodDeclaration.Name;
                foreach (ParameterDeclaration parameter_L in methodDeclaration.Parameters)
                {
                    model.Parameters.Add(parameter_L.Name);
                }
            }
            if (IsWeaved(methodDeclaration.Attributes))
                model.Member = "method";
            base.VisitMethodDeclaration(methodDeclaration);
        }

        private bool IsWeaved(AstNodeCollection<AttributeSection> attributes_P)
        {
            return attributes_P.SelectMany(attributeSection_L => attributeSection_L.Attributes).Any(attribute_L => ((SimpleType) attribute_L.Type).Identifier == "Log");
        }
    }
}