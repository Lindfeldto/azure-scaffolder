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
        // Top-level method that generates all of the scaffolding output from the templates.
        // Shows a busy wait mouse cursor while working.
        public override void GenerateCode()
        {
            var project = Context.ActiveProject;
            var selectionRelativePath = GetSelectionRelativePath();

            if (_codeGeneratorViewModel == null)
            {
                throw new InvalidOperationException(Resources.WebFormsScaffolder_ShowUIAndValidateNotCalled);
            }

            Cursor currentCursor = Mouse.OverrideCursor;
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                GenerateCode(project, selectionRelativePath, this._codeGeneratorViewModel);
            }
            finally
            {
                Mouse.OverrideCursor = currentCursor;
            }
        }

        // Collects the common data needed by all of the scaffolded output and generates:
        // 1) Dynamic Data Field Templates
        // 2) Web Forms Pages
        private void GenerateCode(Project project, string selectionRelativePath, WebFormsCodeGeneratorViewModel codeGeneratorViewModel)
        {
            foreach (var codeType in codeGeneratorViewModel.ModelTypeCollection.Where(m => m.Selected))
            {
                // Get Model Type
                var modelType = codeType.CodeType;

                // Get the dbContext
                string dbContextTypeName = codeGeneratorViewModel.DbContextModelType.TypeName;
                ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
                CodeType dbContext = codeTypeService.GetCodeType(project, dbContextTypeName);

                // Get the dbContext namespace
                string dbContextNamespace = dbContext.Namespace != null ? dbContext.Namespace.FullName : String.Empty;

                if (codeGeneratorViewModel.GenerateViews)
                    // Add Web Forms Pages from Templates
                    AddWebFormsPages(
                        project,
                        selectionRelativePath,
                        dbContextNamespace,
                        dbContextTypeName,
                        modelType,
                        codeGeneratorViewModel.Overwrite
                   );

                if (codeGeneratorViewModel.GenerateController)
                    // Add Controllers from Templates
                    AddControllers(
                        project,
                        selectionRelativePath,
                        dbContextNamespace,
                        dbContextTypeName,
                        modelType,
                        codeGeneratorViewModel.Overwrite
                   );

                if (codeGeneratorViewModel.GenerateApiController)
                    // Add Controllers from Templates
                    AddApiControllers(
                        project,
                        selectionRelativePath,
                        dbContextNamespace,
                        dbContextTypeName,
                        modelType,
                        codeGeneratorViewModel.Overwrite
                   );

                if (codeGeneratorViewModel.GenerateStorageContext)
                    // Add Storage Contexts from Templates
                    AddStorageContexts(
                        project,
                        selectionRelativePath,
                        dbContextNamespace,
                        dbContextTypeName,
                        modelType,
                        codeGeneratorViewModel.Overwrite
                   );

                if (codeGeneratorViewModel.GenerateScripts)
                    // Add Storage Contexts from Templates
                    AddScripts(
                        project,
                        selectionRelativePath,
                        dbContextNamespace,
                        dbContextTypeName,
                        modelType,
                        codeGeneratorViewModel.Overwrite
                   );
            }
        }
    }
}