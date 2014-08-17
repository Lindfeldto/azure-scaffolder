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

namespace AzureTableStorageScaffolder.Scaffolders
{
    public partial class RazorScaffolder 
    {
        // Generates all of the Web Forms Pages (Default Insert, Edit, Delete),
        private void AddStorageContexts(
            Project project,
            string selectionRelativePath,
            string dbContextNamespace,
            string dbContextTypeName,
            CodeType modelType,
            bool useMasterPage,
            string masterPage = null,
            bool overwriteViews = true
        )
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            var webForms = new[] { "StorageContext", "StorageContext.KeyHelpers" };

            // Now add each view
            foreach (string webForm in webForms)
            {
                AddStorageContextTemplates(
                    selectionRelativePath: selectionRelativePath,
                    modelType: modelType,
                    dbContextNamespace: dbContextNamespace,
                    dbContextTypeName: dbContextTypeName,
                    webFormsName: webForm,
                    overwrite: overwriteViews);
            }
        }

        private void AddStorageContextTemplates(
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

            string outputFolderPath = Path.Combine("Models");
            AddFolder(Context.ActiveProject, outputFolderPath);

            string modelNameSpace = modelType.Namespace != null ? modelType.Namespace.FullName : String.Empty;
            string relativePath = outputFolderPath.Replace(@"\", @"/");

            List<string> webFormsTemplates = new List<string>();
            webFormsTemplates.AddRange(new string[] { webFormsName });

            // Scaffold aspx page and code behind
            foreach (string webForm in webFormsTemplates)
            {
                Project project = Context.ActiveProject;
                var templatePath = Path.Combine(webForm);
                string outputPath = Path.Combine(outputFolderPath, webForm + "." + modelType.Name);

                var defaultNamespace = GetDefaultNamespace();
                AddFileFromTemplate(project,
                    outputPath,
                    templateName: templatePath,
                    templateParameters: new Dictionary<string, object>()
                    {
                        {"RelativePath", relativePath},
                        {"DefaultNamespace", defaultNamespace},
                        {"Namespace", modelNameSpace},
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