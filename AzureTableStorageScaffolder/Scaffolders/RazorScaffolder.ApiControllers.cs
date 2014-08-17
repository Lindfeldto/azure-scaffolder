using AzureTableStorageScaffolder.UI;
using AzureTableStorageScaffolder.Utils;
using BlueMarble.Shared.Helpers;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using Microsoft.AspNet.Scaffolding.NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AzureTableStorageScaffolder.Scaffolders
{
    public partial class RazorScaffolder 
    {
        // Generates all of the Web Forms Pages (Default Insert, Edit, Delete),
        private void AddApiControllers(
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

            var webForms = new[] { "ApiController" };

            // Now add each view
            foreach (string webForm in webForms)
            {
                AddApiControllerTemplates(
                    selectionRelativePath: selectionRelativePath,
                    modelType: modelType,
                    dbContextNamespace: dbContextNamespace,
                    dbContextTypeName: dbContextTypeName,
                    webFormsName: webForm,
                    overwrite: overwriteViews);
            }
        }

        private void AddApiControllerTemplates(
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
            string outputFolderPath = Path.Combine("Controllers", "Api");
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

            // Scaffold aspx page and code behind
            foreach (string webForm in webFormsTemplates)
            {
                Project project = Context.ActiveProject;
                var templatePath = Path.Combine(webForm);
                string outputPath = Path.Combine(outputFolderPath, modelType.Name + webForm);

                var defaultNamespace = GetDefaultNamespace();
                AddFileFromTemplate(project,
                    outputPath,
                    templateName: templatePath,
                    templateParameters: new Dictionary<string, object>()
                    {
                        {"RelativePath", relativePath},
                        {"DefaultNamespace", defaultNamespace},
                        {"Namespace", modelNameSpace},
                        {"ScaffoldProperties", scaffoldProperties},
                        {"RelatedProperties", relatedProperties},
                        {"RelatedPropertyTypes", relatedPropertyTypes},
                        {"RelatedDataSources", relatedDataSources},
                        {"ViewDataType", modelType},
                        {"ViewDataTypeName", modelType.Name},
                        {"PluralizedName", GetPluralizedName(modelType.Name)},
                        {"DbContextNamespace", dbContextNamespace},
                        {"DbContextTypeName", dbContextTypeName}
                    },
                    skipIfExists: !overwrite);
            }
        }
    }
}