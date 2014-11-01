using System.Collections.Generic;
using System.Linq;

namespace NetAspect.Doc.Builder.Model
{
    public static class ParameterDescriptionsExtensions
    {
        public static string GetRealParameterName(this List<ParameterDescriptionFactory.ParameterDescription> parameterDescriptions, string parameterName)
        {
            if (parameterDescriptions.Any(p => p.Name == parameterName))
                return parameterName;
            return "The parameter name of the weaved method";
        }
        public static string GetParameterId(this List<ParameterDescriptionFactory.ParameterDescription> parameterDescriptions, string parameterName)
        {
            return GetRealParameterName(parameterDescriptions, parameterName).Replace(" ", "").Replace("+", "");
        }
        public static string GetDescription(this List<ParameterDescriptionFactory.ParameterDescription> parameterDescriptions, string parameterName, bool isInstruction)
        {
            return parameterDescriptions.First(p => p.Name == parameterName && p.InInstruction == isInstruction).Description;
        }
    }

    public class ParameterDescriptionFactory
    {
        public class ParameterDescription
        {
            public ParameterDescription()
            {
                 WhatCanBe = new List<string>();
            }

            public List<string> WhatCanBe { get; set; }

            public string Description { get; set; }
            public string Name { get; set; }
            public bool InInstruction { get; set; }
        }

        public static List<ParameterDescription> Create()
        {
            var parameterDescriptions = new List<ParameterDescription>()
                {
                    BuildInstanceParameterDescription(),
                    BuildMethodParameterDescription(),
                    BuildParametersParameterDescription(),
                    BuildColumnNumberParameterInMethodDescription(),
                    BuildLineNumberParameterInMethodDescription(),
                    BuildFileNameParameterInMethodDescription(),
                    BuildFilePathParameterInMethodDescription(),
                    BuildParameterNameParameterDescription(),
                    BuildNewPropertyValueParameterDescription(),
                    BuildCallerInstanceParameterDescription(),
                    BuildNewFieldValueParameterDescription(),
                    BuildrParameterDescription(),
                    BuildrParameterValueDescription(),
                    BuildCallerParametersParameterDescription(),
                    BuildExceptionParameterDescription(),
                    BuildResultParameterDescription(),
                    BuildFieldParameterDescription(),
                    BuildPropertyValueParameterDescription(),
                    BuildFieldValueParameterDescription(),
                    BuildColumnNumberParameterInInstructionDescription(),
                    BuildLineNumberParameterInInstructionDescription(),
                    BuildFilePathParameterInInstructionDescription(),
                    BuildFileNameParameterInInstructionDescription(),
                };
            return parameterDescriptions;
        }
        private static ParameterDescription BuildParameterNameParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the value of the parameter with the same name of the weaved method",
                Name = "The parameter name of the weaved method",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the same type as the type of the parameter in the weaved method",
                        }
            };
        }
        private static ParameterDescription BuildNewPropertyValueParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the value of the value that will be affected to the property",
                Name = "newPropertyValue",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the same type as the property",
                        },
                InInstruction = true,
                        
            };
        }
        private static ParameterDescription BuildNewFieldValueParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the value of the value that will be affected to the field",
                Name = "newFieldValue",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the same type as the field",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildPropertyValueParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the value of the property",
                Name = "propertyValue",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the same type as the property",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildFieldValueParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the value of the field",
                Name = "fieldValue",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the same type as the field",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildFieldParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get some information about the field",
                Name = "field",
                WhatCanBe = new List<string>()
                        {
                            "It must be of System.Reflection.FieldInfo type",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildResultParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the return value of the weaved method",
                Name = "result",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the same type as the return type of the weaved method",
                        }
            };
        }
        private static ParameterDescription BuildExceptionParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the exception thrown in the weaved method",
                Name = "exception",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.Exception type",
                        }
            };
        }
        private static ParameterDescription BuildCallerParametersParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to have the parameters of the method that will call the weaved member",
                Name = "callerParameters",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.Object[] type",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildrParameterValueDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to have the value of the weaved parameter",
                Name = "parameterValue",
                WhatCanBe = new List<string>()
                        {
                            "It can be declared as object",
                            "It can be declared with the same type as the parameter type",
                        }
            };
        }
        private static ParameterDescription BuildrParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get information about the parameter",
                Name = "parameter",
                WhatCanBe = new List<string>()
                        {
                            "It must be of System.Reflection.ParameterInfo type",
                        }
            };
        }

        private static ParameterDescription BuildColumnNumberParameterInMethodDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the column of the first instruction in the weaved method",
                Name = "columnNumber",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.Int32 type",
                        }
            };
        }
        private static ParameterDescription BuildLineNumberParameterInMethodDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the line of the first instruction in the weaved method",
                Name = "lineNumber",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.Int32 type",
                        }
            };
        }
        private static ParameterDescription BuildFileNameParameterInMethodDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the file name of the weaved method",
                Name = "fileName",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.String type",
                        }
            };
        }
        private static ParameterDescription BuildFilePathParameterInMethodDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the file path of the weaved method",
                Name = "filePath",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.String type",
                        }
            };
        }
        private static ParameterDescription BuildColumnNumberParameterInInstructionDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the column of the instruction that call our member",
                Name = "columnNumber",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.Int32 type",
                        },
                InInstruction = true,
            };
        }

        private static ParameterDescription BuildLineNumberParameterInInstructionDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the line of the instruction that call our member",
                Name = "lineNumber",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.Int32 type",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildFileNameParameterInInstructionDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the file name of the instruction that call our membe",
                Name = "fileName",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.String type",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildFilePathParameterInInstructionDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the file path of the instruction that call our membe",
                Name = "filePath",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.String type",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildParametersParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get the values of the parameters of the weaved method",
                Name = "parameters",
                WhatCanBe = new List<string>()
                        {
                            "It must be declared with the System.Object[] type",
                        }
            };
        }

        private static ParameterDescription BuildMethodParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get some information about the weaved method",
                Name = "method",
                WhatCanBe = new List<string>()
                        {
                            "It must be of System.Reflection.MethodInfo type",
                        }
            };
        }
        private static ParameterDescription BuildPropertyInMethodParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get some information about the current property",
                Name = "property",
                WhatCanBe = new List<string>()
                        {
                            "It must be of System.Reflection.PropertyInfo type",
                        }
            };
        }
        private static ParameterDescription BuildPropertyInInstructionParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get some information about the property",
                Name = "property",
                WhatCanBe = new List<string>()
                        {
                            "It must be of System.Reflection.PropertyInfo type",
                        },
                InInstruction = true,
            };
        }
        private static ParameterDescription BuildConstructorParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to get some information about the weaved constructor",
                Name = "constructor",
                WhatCanBe = new List<string>()
                        {
                            "It must be of System.Reflection.ConstructorInfo type",
                        }
            };
        }

        private static ParameterDescription BuildInstanceParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to have the <b>this</b> of the weaved member",
                Name = "instance",
                WhatCanBe = new List<string>()
                        {
                            "It can be declared as object",
                            "It can be declared with the real type object",
                        }
            };
        }

        private static ParameterDescription BuildCallerInstanceParameterDescription()
        {
            return new ParameterDescription()
            {
                Description = "this parameter is used to have the instance of the object that call the weaved member",
                Name = "callerInstance",
                WhatCanBe = new List<string>()
                        {
                            "It can be declared as object",
                            "It can be declared with the real type object",
                        },
                InInstruction = true,
            };
        }
    }

    public class AvailableParametersSectionModel
    {
        private readonly Dictionary<string, string> parameterDescriptions;

        public AvailableParametersSectionModel(List<ParameterModel> parameters, Dictionary<string, string> parameterDescriptions)
        {
            Parameters = parameters;
            this.parameterDescriptions = parameterDescriptions;
        }


        public List<ParameterModel> Parameters { get; set; }



        
    }
}