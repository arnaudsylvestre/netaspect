using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace NetAspect.Doc.Builder
{
    internal class FindInvocationsVisitor : DepthFirstAstVisitor
    {
        private readonly List<InterceptorDescription> _interceptors;
        private readonly List<ParameterDescription> _parameters;
        private readonly List<PossibilityDescription> _possibilities;
        private readonly TestDescription _test;

        public FindInvocationsVisitor(List<PossibilityDescription> possibilities_P, List<ParameterDescription> parameters, TestDescription test_P, List<InterceptorDescription> interceptors_P)
        {
            _possibilities = possibilities_P;
            _parameters = parameters;
            _test = test_P;
            _interceptors = interceptors_P;
        }

        public override void VisitAttribute(Attribute attribute)
        {
            if (attribute.Type.ToString().Contains("PossibilityDocumentation"))
            {
                List<PrimitiveExpression> arguments = attribute.Arguments.Cast<PrimitiveExpression>().ToList();
                string group = GetValue(arguments[3]);
                var possibilityDescription = new PossibilityDescription
                    {
                        Description = GetValue(arguments[1]),
                        Kind = GetValue(arguments[0]),
                        Title = GetValue(arguments[2]),
                        Group = group
                    };
                _possibilities.Add(possibilityDescription);
            }
            if (attribute.Type.ToString().Contains("ParameterDescription"))
            {
                List<PrimitiveExpression> arguments = attribute.Arguments.Cast<PrimitiveExpression>().ToList();
                _parameters.Add(
                    new ParameterDescription
                        {
                            Name = GetValue(arguments[0]),
                            Description = GetValue(arguments[1]),
                            Kind = GetValue(arguments[2]),
                        });
            }
            if (attribute.Type.ToString().Contains("InterceptorDescription"))
            {
                List<PrimitiveExpression> arguments = attribute.Arguments.Cast<PrimitiveExpression>().ToList();
                _interceptors.Add(
                    new InterceptorDescription
                        {
                            Name = GetValue(arguments[0]),
                            Called = GetValue(arguments[1]),
                        });
            }
            base.VisitAttribute(attribute);
        }

        private static string GetValue(PrimitiveExpression expression)
        {
            return expression.LiteralValue.Replace("\"", "");
        }

        public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
        {
            if (IsWeaved(parameterDeclaration.Attributes))
                _test.Member = "parameter";
            base.VisitParameterDeclaration(parameterDeclaration);
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (IsWeaved(propertyDeclaration.Attributes))
                _test.Member = "property";
            base.VisitPropertyDeclaration(propertyDeclaration);
        }

        public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            if (IsWeaved(fieldDeclaration.Attributes))
                _test.Member = "field";
            base.VisitFieldDeclaration(fieldDeclaration);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            if (constructorDeclaration.Initializer.Arguments.Count == 3)
            {
                List<PrimitiveExpression> arguments = constructorDeclaration.Initializer.Arguments.Cast<PrimitiveExpression>().ToList();
                _test.Kind = GetValue(arguments[1]);
                _test.Possibility = GetValue(arguments[2]);
                _test.Description = GetValue(arguments[0]);
            }

            if (IsWeaved(constructorDeclaration.Attributes))
                _test.Member = "constructor";
            base.VisitConstructorDeclaration(constructorDeclaration);
        }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            _test.CallCode = lambdaExpression.Body.ToString();
            base.VisitLambdaExpression(lambdaExpression);
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration.Name.EndsWith("Attribute"))
            {
                _test.AspectCode = typeDeclaration.ToString();
            }
            if (typeDeclaration.Name == "MyInt")
            {
                _test.ClassToWeaveCode = typeDeclaration.ToString();
            }
            base.VisitTypeDeclaration(typeDeclaration);
        }

        public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (((TypeDeclaration) methodDeclaration.Parent).Name.EndsWith("Attribute"))
            {
                _test.MethodName = methodDeclaration.Name;
                foreach (ParameterDeclaration parameter_L in methodDeclaration.Parameters)
                {
                    _test.AspectParameters.Add(parameter_L.Name);
                }
            }
            if (IsWeaved(methodDeclaration.Attributes))
                _test.Member = "method";
            base.VisitMethodDeclaration(methodDeclaration);
        }

        private bool IsWeaved(AstNodeCollection<AttributeSection> attributes_P)
        {
            return attributes_P.SelectMany(attributeSection_L => attributeSection_L.Attributes).Any(attribute_L => ((SimpleType) attribute_L.Type).Identifier == "Log");
        }
    }
}