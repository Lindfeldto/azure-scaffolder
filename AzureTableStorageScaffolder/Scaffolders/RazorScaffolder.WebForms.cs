using AzureTableStorageScaffolder.UI;
using AzureTableStorageScaffolder.Utils;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using Microsoft.AspNet.Scaffolding.NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using BlueMarble.Shared.Azure.Storage.Table;
using BlueMarble.Shared.Helpers;

namespace AzureTableStorageScaffolder.Scaffolders
{
    public partial class RazorScaffolder
    {
        // Generates all of the Web Forms Pages (Default Insert, Edit, Delete),
        private void AddWebFormsPages(
            Project project,
            string selectionRelativePath,
            string dbContextNamespace,
            string dbContextTypeName,
            CodeType modelType,
            bool overwriteViews = true
        )
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            var webForms = new[] { "Index", "Create", "Edit", "Delete", "Details", "Resources" };

            // Now add each view
            foreach (string webForm in webForms)
            {
                AddWebFormsViewTemplates(
                    selectionRelativePath: selectionRelativePath,
                    modelType: modelType,
                    dbContextNamespace: dbContextNamespace,
                    dbContextTypeName: dbContextTypeName,
                    webFormsName: webForm,
                    overwrite: overwriteViews);
            }
        }

        private void AddWebFormsViewTemplates(
            string selectionRelativePath,
                                CodeType modelType,
                                string dbContextNamespace,
                                string dbContextTypeName,
                                string webFormsName,
                                bool overwrite = false
        )
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (String.IsNullOrEmpty(webFormsName))
            {
                throw new ArgumentException(Resources.WebFormsViewScaffolder_EmptyActionName, "webFormsName");
            }

            // Add folder for views. This is necessary to display an error when the folder already exists but
            // the folder is excluded in Visual Studio: see https://github.com/Superexpert/WebFormsScaffolding/issues/18
            string outputFolderPath = Path.Combine("Views", modelType.Name);
            AddFolder(Context.ActiveProject, outputFolderPath);

            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            string relativePath = outputFolderPath.Replace(@"\", @"/");

            List<string> webFormsTemplates = new List<string>();
            webFormsTemplates.AddRange(new string[] { webFormsName });

            Dictionary<string, ScaffoldProperty> completeScaffoldProperties = GetScaffoldProperties(modelType);

            var scaffoldProperties = new Dictionary<string, string>();
            var relatedProperties = new Dictionary<string, string>();
            var relatedPropertyTypes = new Dictionary<string, string>();
            var relatedDataSources = new Dictionary<string, string>();
            var spacedPropertyNames = new Dictionary<string, string>();

            foreach (var scaffoldProperty in completeScaffoldProperties)
            {
                scaffoldProperties.Add(scaffoldProperty.Key, scaffoldProperty.Value.TypeFullName);

                spacedPropertyNames.Add(scaffoldProperty.Key, System.Text.RegularExpressions.Regex.Replace(scaffoldProperty.Value.TypeFullName, "([a-z]|[A-Z]{2,})([A-Z])", @"$1 $2"));

                if (scaffoldProperty.Value.RelatedPropertyType != null)
                {
                    relatedProperties.Add(scaffoldProperty.Key, GetPluralizedName(scaffoldProperty.Key));

                    relatedPropertyTypes.Add(scaffoldProperty.Key, scaffoldProperty.Value.RelatedPropertyType.Name);

                    relatedDataSources.Add(scaffoldProperty.Key, GetPluralizedName(scaffoldProperty.Value.RelatedPropertyType.Name));
                }
            }

            var spacedModelName = System.Text.RegularExpressions.Regex.Replace(modelType.Name, "([a-z]|[A-Z]{2,})([A-Z])", @"$1 $2");
            var pluralizedSpacedModelName = GetPluralizedName(spacedModelName);

            // Scaffold aspx page and code behind
            foreach (string webForm in webFormsTemplates)
            {
                Project project = Context.ActiveProject;
                var templatePath = Path.Combine(webForm);
                string outputPath = Path.Combine(outputFolderPath, webForm);

                var defaultNamespace = GetDefaultNamespace() + "." + modelType.Name;
                var resourceNamespace = GetDefaultNamespace() + ".Views." + modelType.Name;
                AddFileFromTemplate(project,
                    outputPath,
                    templateName: templatePath,
                    templateParameters: new Dictionary<string, object>()
                    {
                        {"RelativePath", relativePath},
                        {"DefaultNamespace", defaultNamespace},
                        {"ResourceNamespace", resourceNamespace},
                        {"Namespace", modelNameSpace},
                        {"ScaffoldProperties", scaffoldProperties},
                        {"RelatedProperties", relatedProperties},
                        {"RelatedPropertyTypes", relatedPropertyTypes},
                        {"RelatedDataSources", relatedDataSources},
                        {"SpacedPropertyNames", spacedPropertyNames},
                        {"SpacedModelName", spacedModelName},
                        {"PluralizedSpacedModelName", pluralizedSpacedModelName},
                        {"ViewDataType", modelType},
                        {"ViewDataTypeName", modelType.Name},
                        {"PluralizedName", GetPluralizedName(modelType.Name)},
                        {"DbContextNamespace", dbContextNamespace},
                        {"DbContextTypeName", dbContextTypeName},
                    },
                    skipIfExists: !overwrite);

                var projectItem = RecurseProjectItems(project.ProjectItems, "", outputPath);

                if (webForm == "Resources")
                {
                    foreach (Property property in projectItem.Properties)
                    {
                        if (property.Name == "CustomTool")
                            property.Value = "PublicResXFileCodeGenerator";
                    }
                }
            }
        }

        private static ProjectItem RecurseProjectItems(ProjectItems projectItems, string basePath, string outputPath)
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                if ((basePath + "\\" + projectItem.Name).StartsWith(outputPath))
                    return projectItem;

                var recursedProjectItem = RecurseProjectItems(projectItem.ProjectItems, (string.IsNullOrWhiteSpace(basePath)) ? projectItem.Name : basePath + "\\" + projectItem.Name, outputPath);

                if (recursedProjectItem != null)
                    return recursedProjectItem;
            }

            return null;
        }

        private static List<string> s_ExcludedScaffoldProperties = new List<string>()
        {
            "PartitionKey",
            "RowKey",
            "Timestamp",
            "ETag",
            "Id",
            "PublicId",
            "CreatedDateTime",
            "DisablePropertyResolverCache",
            "DisableCompiledSerializers"
        };
    }
}