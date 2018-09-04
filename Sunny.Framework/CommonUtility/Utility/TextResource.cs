using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Resources;
using System.ComponentModel;

namespace CommonUtility.Utility
{
    internal class TextResource
    {
        // Fields
        private static CultureInfo resourceCulture;
        private static ResourceManager resourceMan;

        // Methods
        internal TextResource()
        {
        }

        // Properties
        internal static string ArrayCannotBeEmpty
        {
            get
            {
                return ResourceManager.GetString("ArrayCannotBeEmpty", resourceCulture);
            }
        }

        internal static string CannotBeNull
        {
            get
            {
                return ResourceManager.GetString("CannotBeNull", resourceCulture);
            }
        }

        internal static string CannotBeNullOrEmpty
        {
            get
            {
                return ResourceManager.GetString("CannotBeNullOrEmpty", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
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

        internal static string NotSupportThisType
        {
            get
            {
                return ResourceManager.GetString("NotSupportThisType", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    ResourceManager manager = new ResourceManager("Liger.Common.Resources.TextResource", typeof(TextResource).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }
    }

}
