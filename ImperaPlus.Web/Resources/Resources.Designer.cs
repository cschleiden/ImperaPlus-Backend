﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImperaPlus.Web.Resources {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///    A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        internal Resources() {
        }
        
        /// <summary>
        ///    Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ImperaPlus.Web.Resources.Resources", typeof(Resources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///    Overrides the current thread's CurrentUICulture property for all
        ///    resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to Welcome to Impera! 
        ///
        ///Please verify you account by going to {0}, after that you can log in.
        ///
        ///-Impera Team.
        /// </summary>
        public static string EmailConfirmationBody {
            get {
                return ResourceManager.GetString("EmailConfirmationBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to Impera Account Confirmation.
        /// </summary>
        public static string EmailConfirmationSubject {
            get {
                return ResourceManager.GetString("EmailConfirmationSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to You have requested to reset your password. To generate a new password please go to {0}. If you did not attept to reset your password please ignore this email.
        ///
        ///-Impera Team.
        /// </summary>
        public static string ResetPasswordBody {
            get {
                return ResourceManager.GetString("ResetPasswordBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to Impera Account Password Reset.
        /// </summary>
        public static string ResetPasswordSubject {
            get {
                return ResourceManager.GetString("ResetPasswordSubject", resourceCulture);
            }
        }
    }
}