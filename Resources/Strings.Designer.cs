

namespace GraphicsDemo.Resources
{
    using System;
    
    /// <summary>
    /// Клас за strongly-typed достъп до ресурсите.
    /// </summary>
    /// <remarks>
    /// Автоматично генериран клас за достъп до локализирани стрингове.
    /// Студент: Dimitar Klianev, F112194
    /// </remarks>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings
    {
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings()
        {
        }
        
        /// <summary>
        /// Връща кеширания ResourceManager за достъп до ресурсите.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GraphicsDemo.Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        /// Задава или получава текущата култура за ресурсите.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        /// Заглавие на приложението
        /// </summary>
        internal static string AppTitle
        {
            get
            {
                return ResourceManager.GetString("AppTitle", resourceCulture);
            }
        }
        
        /// <summary>
        /// Информация за студента
        /// </summary>
        internal static string StudentInfo
        {
            get
            {
                return ResourceManager.GetString("StudentInfo", resourceCulture);
            }
        }
    }
}
