using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace SitefinityWebApp
{
    public class MyDynamicModuleResources : Resource
    {
        /// <summary>
        /// word: Title
        /// </summary>
        [ResourceEntry("Title",
            Value = "Title from MyDynamicModuleResources",
            Description = "word: Title",
            LastModified = "2016/5/14")]
        public string Title
        {
            get
            {
                return this["Title"];
            }
        }

        /// <summary>
        /// word: Content
        /// </summary>
        [ResourceEntry("Content",
            Value = "Content from MyDynamicModuleResources",
            Description = "word: Content",
            LastModified = "2016/5/14")]
        public string Content
        {
            get
            {
                return this["Content"];
            }
        }
    }
}