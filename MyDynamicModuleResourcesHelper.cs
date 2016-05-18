using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Web.UI.ContentUI.Config;
using Telerik.Sitefinity.Web.UI.ContentUI.Views.Backend.Master.Config;

namespace SitefinityWebApp
{
    public class MyDynamicModuleResourcesHelper
    {
        protected static List<ContentViewDefinitionElement> GetContentViewElements(ConfigManager configManager, out ContentViewConfig config)
        {
            config = configManager.GetSection<ContentViewConfig>();
            var contentBackendDefinitionKeys = config.ContentViewControls.Keys.Where(c => c.StartsWith(dynamicTypes)).ToList();

            var viewsCollection = new List<ContentViewDefinitionElement>();
            foreach (var viewDefinitonKey in contentBackendDefinitionKeys)
            {
                var viewDefiniton = config.ContentViewControls[viewDefinitonKey];
                var views = viewDefiniton.ViewsConfig.Values.Where(v => v.ViewName.EndsWith(backendInsert) || v.ViewName.EndsWith(backendEdit)).ToList();
                viewsCollection.AddRange(views);
            }

            return viewsCollection;
        }

        public static void Install()
        {
            // Get only the resource class declared properties with ResourceEntry attribute
            string[] resourceFields = typeof(MyDynamicModuleResources).GetProperties(System.Reflection.BindingFlags.Public 
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.DeclaredOnly)
                .Where(prop => Attribute.IsDefined(prop, typeof(ResourceEntryAttribute)))
                .Select(f => f.Name)
                .ToArray();

            var configManager = ConfigManager.GetManager();
            using (new ElevatedModeRegion(configManager))
            {
                ContentViewConfig config;
                var viewElements = MyDynamicModuleResourcesHelper.GetContentViewElements(configManager, out config);
                bool needSaveChanges = false;
                foreach (var viewElement in viewElements)
                {
                    var detailView = viewElement as DetailFormViewElement;
                    var sectionKeys = detailView.Sections.Keys;
                    foreach (var sectionKey in sectionKeys)
                    {
                        var fields = detailView.Sections[sectionKey].Fields;
                        var fieldKeys = fields.Keys;
                        foreach (var fieldKey in fieldKeys)
                        {
                            var field = fields[fieldKey];
                            if (resourceFields.Contains(field.FieldName))
                            {
                                // Handle setting the ResourceClassId condition
                                if (field.ResourceClassId != MyDynamicModuleResourcesHelper.myDynamicModuleResourcesClassName)
                                {
                                    field.ResourceClassId = MyDynamicModuleResourcesHelper.myDynamicModuleResourcesClassName;
                                    needSaveChanges = true;
                                }
                            }
                        }
                    }

                    if (needSaveChanges)
                    {
                        // Save the section at this point to use the actual element section (in this case the DynamicModulesConfig, which is internal)
                        configManager.SaveSection(viewElement.Section);
                        needSaveChanges = false;
                    }
                }
            }
            configManager.Dispose();
        }

        public static readonly string dynamicTypes = "Telerik.Sitefinity.DynamicTypes.Model";
        public static readonly string backendInsert = "BackendInsertView";
        public static readonly string backendEdit = "BackendEditView";
        public static readonly string myDynamicModuleResourcesClassName = typeof(MyDynamicModuleResources).Name;  
    }
}