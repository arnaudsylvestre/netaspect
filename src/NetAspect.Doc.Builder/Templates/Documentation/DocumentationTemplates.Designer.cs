﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetAspect.Doc.Builder.Templates.Documentation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DocumentationTemplates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DocumentationTemplates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NetAspect.Doc.Builder.Templates.Documentation.DocumentationTemplates", typeof(DocumentationTemplates).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to             &lt;p&gt;The &quot;method weaving&quot; will modify the code of the method to add behavior. It is possible to add behaviors :&lt;/p&gt;
        ///            &lt;ul&gt;
        ///               &lt;li&gt;&lt;b&gt;Before&lt;/b&gt; the method is executed&lt;/li&gt;
        ///               &lt;li&gt;&lt;b&gt;After&lt;/b&gt; the method is executed when no exception is raised&lt;/li&gt;
        ///               &lt;li&gt;&lt;b&gt;When exception occurs&lt;/b&gt; in the method&lt;/li&gt;
        ///               &lt;li&gt;&lt;b&gt;On finally&lt;/b&gt; : at the end of the method when an exception is raised or not&lt;/li&gt;
        ///            &lt;/ul&gt;
        ///            &lt;p&gt;For each  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ParagraphDescriptionMethodInterceptor {
            get {
                return ResourceManager.GetString("ParagraphDescriptionMethodInterceptor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These aspects are called when the aspect is put on a &lt;b&gt;$subParagraph.Member&lt;/b&gt;.
        /// </summary>
        internal static string SubParagraphDescriptionMethodInterceptor {
            get {
                return ResourceManager.GetString("SubParagraphDescriptionMethodInterceptor", resourceCulture);
            }
        }
    }
}
