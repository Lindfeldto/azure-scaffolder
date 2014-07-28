using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.NuGet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using AzureTableStorageScaffolder.UI;
using AzureTableStorageScaffolder;

namespace AzureTableStorageScaffolder.Scaffolders
{    

    // This is where everything with the scaffolder is kicked off. The factory
    // returns a WebFormsScaffolder when a project meets the requirements.

    [Export(typeof(CodeGeneratorFactory))]
    public class RazorScaffolderFactory : CodeGeneratorFactory
    {
        public RazorScaffolderFactory()
            : base(CreateCodeGeneratorInformation())
        {

        }

        public override ICodeGenerator CreateInstance(CodeGenerationContext context)
        {
            return new RazorScaffolder(context, Information);
        }
      
        // We support CSharp WAPs targetting at least .Net Framework 4.5 or above.
        // We DON'T currently support VB
        public override bool IsSupported(CodeGenerationContext codeGenerationContext)
        {
            if (ProjectLanguage.CSharp.Equals(codeGenerationContext.ActiveProject.GetCodeLanguage()) )
            {
                FrameworkName targetFramework = codeGenerationContext.ActiveProject.GetTargetFramework();
                return (targetFramework != null) &&
                        String.Equals(".NetFramework", targetFramework.Identifier, StringComparison.OrdinalIgnoreCase) &&
                        targetFramework.Version >= new Version(4, 5);
            }

            return false;
        }

        private static CodeGeneratorInformation CreateCodeGeneratorInformation()
        {
            return new CodeGeneratorInformation(
                displayName: Resources.WebFormsScaffolder_Name,
                description: Resources.WebFormsScaffolder_Description,
                author: "Blue Marble Software (Pty) Ltd.",
                version: new Version(0, 1, 0, 0),
                id: typeof(RazorScaffolder).Name,
                icon: null,
                gestures: null,
                categories: new[] { "Common/Azure Storage" }
            );              
        }
    }
}
