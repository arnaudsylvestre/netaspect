﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetAspect.Doc.Builder.Templates {
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
    internal class Templates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Templates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NetAspect.Doc.Builder.Templates.Templates", typeof(Templates).Assembly);
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
        ///   Looks up a localized string similar to #set( $H = &apos;#&apos; )
        ///$page.SetParameterDescription(&apos;instance&apos;, &apos;this parameter is used to have the &lt;b&gt;this&lt;/b&gt; of the weaved member&apos;)
        ///$page.SetParameterDescription(&apos;method&apos;, &apos;this parameter is used to get some information about the weaved method&apos;)
        ///
        ///&lt;div class=&quot;container bs-docs-container&quot;&gt;
        ///      &lt;div class=&quot;row&quot;&gt;
        ///		&lt;div class=&quot;col-md-12&quot; role=&quot;main&quot;&gt;
        ///
        ///	$page.GeneratePutAttributesSection();
        ///	$page.GenerateInterceptorsSection();
        ///	$page.GenerateLifeCyclesSection();
        ///
        ///
        ///	
        ///
        ///
        ///		&lt;/div&gt;
        ///	&lt;/div&gt;
        ///&lt;/div&gt;
        ///.
        /// </summary>
        internal static string DocumentationPage {
            get {
                return ResourceManager.GetString("DocumentationPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///  &lt;div class=&quot;container bs-docs-container&quot;&gt;
        ///	
        ///	&lt;div class=&quot;row&quot;&gt;
        ///      
        ///	  &lt;div class=&quot;span9&quot; role=&quot;main&quot;&gt;
        ///
        ///
        ///
        ///        &lt;div class=&quot;bs-docs-section&quot;&gt;
        ///          &lt;div class=&quot;page-header&quot;&gt;
        ///            &lt;h1&gt;What to weave ?&lt;/h1&gt;
        ///          &lt;/div&gt;
        ///		  &lt;p&gt;Here is a sample class to divide two numbers. We want to log when there is an exception. This class is in a C# project named &lt;b&gt;Math.csproj&lt;/b&gt;&lt;/p&gt;
        ///		  &lt;pre class=&quot;prettyprint&quot;&gt;
        ///$page.CodeWithoutAspect&lt;/pre&gt;
        ///          &lt;div class=&quot;page-header&quot;&gt;
        ///      [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GettingStartedPage {
            get {
                return ResourceManager.GetString("GettingStartedPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     &lt;div class=&quot;jumbotron&quot;&gt;
        ///      &lt;div class=&quot;container&quot;&gt;
        ///        &lt;h1&gt;NetAspect&lt;/h1&gt;
        ///        &lt;p&gt;An open source AOP framework for .Net and .Net Compact Framework&lt;/p&gt;
        ///        &lt;p&gt;&lt;a class=&quot;btn btn-primary btn-lg&quot; href=&quot;NetAspect.html&quot; role=&quot;button&quot;&gt;Learn more &amp;raquo;&lt;/a&gt;&lt;/p&gt;
        ///      &lt;/div&gt;
        ///    &lt;/div&gt;
        ///
        ///    &lt;div class=&quot;container&quot;&gt;
        ///      &lt;!-- Example row of columns --&gt;
        ///      &lt;div class=&quot;row&quot;&gt;
        ///        &lt;div class=&quot;col-md-4&quot;&gt;
        ///          &lt;h2&gt;Simple configuration&lt;/h2&gt;
        ///          &lt;p&gt;NetAspect try to be the mos [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HomePage {
            get {
                return ResourceManager.GetString("HomePage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #set( $H = &apos;#&apos; )
        ///
        ///&lt;div class=&quot;bs-docs-section&quot;&gt;
        ///				&lt;div class=&quot;page-header&quot;&gt;
        ///					&lt;h1 id=&quot;dropdowns&quot;&gt;Interceptors&lt;/h1&gt;
        ///				&lt;/div&gt;
        ///				&lt;p&gt;An interceptor is a &lt;b&gt;method&lt;/b&gt; declared in the &lt;b&gt;aspect&lt;/b&gt; and which will be called at certain moment after the weaving.&lt;br/&gt;Some &lt;b&gt;parameters&lt;/b&gt; can be added to the interceptor to get more information&lt;/p&gt;
        ///				&lt;p&gt;The following interceptors are available :&lt;/p&gt;
        ///#foreach ($member in $section.Members)
        ///				&lt;h2 class=&quot;list-group-item-heading&quot;&gt;For an aspect put o [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InterceptorsSection {
            get {
                return ResourceManager.GetString("InterceptorsSection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;div class=&quot;page-header&quot;&gt;
        ///               &lt;h1 id=&quot;kinds&quot;&gt;Life cycles&lt;/h1&gt;
        ///            &lt;/div&gt;
        ///            &lt;p&gt;An aspect is a class and that&apos;s why it is instantiated. There is 3 kinds of aspect life cycles&lt;/p&gt;
        ///
        ///
        ///			&lt;div class=&quot;list-group&quot;&gt;
        ///  &lt;a href=&quot;#TransientSample&quot; data-toggle=&quot;collapse&quot;  class=&quot;list-group-item list-group-item-info&quot;&gt;
        ///    &lt;h4 class=&quot;list-group-item-heading&quot;&gt;Transient&lt;/h4&gt;
        ///    &lt;p class=&quot;list-group-item-text&quot;&gt;The aspect is instantiated each time you entered in the weaved member.&lt;/p&gt;
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LifeCyclesSection {
            get {
                return ResourceManager.GetString("LifeCyclesSection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///  &lt;div class=&quot;container bs-docs-container&quot;&gt;
        ///	
        ///	&lt;div class=&quot;row&quot;&gt;
        ///      
        ///	  &lt;div class=&quot;span9&quot; role=&quot;main&quot;&gt;
        ///
        ///
        ///
        ///        &lt;div class=&quot;bs-docs-section&quot;&gt;
        ///          &lt;div class=&quot;page-header&quot;&gt;
        ///            &lt;h1&gt;What is AOP ?&lt;/h1&gt;
        ///          &lt;/div&gt;
        ///		  &lt;p&gt;Here is a sample class to divide two numbers. We want to log when there is an exception. This class is in a C# project named &lt;b&gt;Math.csproj&lt;/b&gt;&lt;/p&gt;
        ///
        ///          &lt;div class=&quot;page-header&quot;&gt;
        ///            &lt;h1&gt;Prepare your project&lt;/h1&gt;
        ///          &lt;/div&gt;
        ///       [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string NetAspectPage {
            get {
                return ResourceManager.GetString("NetAspectPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
        ///&lt;html lang=&quot;en&quot;&gt;
        ///   &lt;head&gt;
        ///      &lt;meta charset=&quot;utf-8&quot;&gt;
        ///      &lt;meta http-equiv=&quot;X-UA-Compatible&quot; content=&quot;IE=edge&quot;&gt;
        ///      &lt;meta name=&quot;viewport&quot; content=&quot;width=device-width, initial-scale=1.0&quot;&gt;
        ///      &lt;meta name=&quot;description&quot; content=&quot;&quot;&gt;
        ///      &lt;meta name=&quot;author&quot; content=&quot;&quot;&gt;
        ///      &lt;link rel=&quot;shortcut icon&quot; href=&quot;../../docs-assets/ico/favicon.png&quot;&gt;
        ///      &lt;title&gt;NetAspect&lt;/title&gt;
        ///      &lt;link href=&quot;bootstrap-3.2.0/dist/css/bootstrap.css&quot; rel=&quot;stylesheet&quot;&gt;
        ///      &lt;link href=&quot;jumbotron.cs [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PageContainer {
            get {
                return ResourceManager.GetString("PageContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #set( $H = &apos;#&apos; )
        ///
        ///            &lt;div class=&quot;page-header&quot;&gt;
        ///               &lt;h1 id=&quot;kinds&quot;&gt;Put aspects&lt;/h1&gt;
        ///            &lt;/div&gt;
        ///            &lt;p&gt;In NetAspect, aspects can be put on fields/properties/methods/constructors. There is 2 ways to put an aspect on a member :&lt;/p&gt;
        ///
        ///			&lt;div class=&quot;list-group&quot;&gt;
        ///  &lt;a href=&quot;#AttributesSample&quot; data-toggle=&quot;collapse&quot;  class=&quot;list-group-item list-group-item-info&quot;&gt;
        ///    &lt;h4 class=&quot;list-group-item-heading&quot;&gt;By attributes&lt;/h4&gt;
        ///    &lt;p class=&quot;list-group-item-text&quot;&gt;Just consider  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PutAttributesSection {
            get {
                return ResourceManager.GetString("PutAttributesSection", resourceCulture);
            }
        }
    }
}
