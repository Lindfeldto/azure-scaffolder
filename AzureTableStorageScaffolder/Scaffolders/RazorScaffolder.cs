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
using AzureTableStorageScaffolder.Extensions;

namespace AzureTableStorageScaffolder.Scaffolders
{
    // This class performs all of the work of scaffolding. The methods are executed in the
    // following order:
    // 1) ShowUIAndValidate() - displays the Visual Studio dialog for setting scaffolding options
    // 2) Validate() - validates the model collected from the dialog
    // 3) GenerateCode() - if all goes well, generates the scaffolding output from the templates
    public partial class RazorScaffolder : CodeGenerator
    {
        private WebFormsCodeGeneratorViewModel _codeGeneratorViewModel;

        internal RazorScaffolder(CodeGenerationContext context, CodeGeneratorInformation information)
            : base(context, information)
        {
        }

        // Shows the Visual Studio dialog that collects scaffolding options
        // from the user.
        // Passing the dialog to this method so that all scaffolder UIs
        // are modal is still an open question and tracked by bug 578173.
        public override bool ShowUIAndValidate()
        {
            _codeGeneratorViewModel = new WebFormsCodeGeneratorViewModel(Context);

            WebFormsScaffolderDialog window = new WebFormsScaffolderDialog(_codeGeneratorViewModel);
            bool? isOk = window.ShowModal();

            if (isOk == true)
            {
                Validate();
            }

            return (isOk == true);
        }

        // Validates the model returned by the Visual Studio dialog.
        // We always force a Visual Studio build so we have a model
        // that we can use with the Entity Framework.
        private void Validate()
        {
            foreach (ModelType _modelType in _codeGeneratorViewModel.ModelTypeCollection.Where(m => m.Selected))
            {
                CodeType modelType = _modelType.CodeType;
                ModelType dbContextType = _codeGeneratorViewModel.DbContextModelType;
                string dbContextTypeName = (dbContextType != null)
                    ? dbContextType.TypeName
                    : null;

                if (modelType == null)
                {
                    throw new InvalidOperationException(Resources.WebFormsScaffolder_SelectModelType);
                }

                if (dbContextType == null || String.IsNullOrEmpty(dbContextTypeName))
                {
                    throw new InvalidOperationException(Resources.WebFormsScaffolder_SelectDbContextType);
                }

                // always force the project to build so we have a compiled
                // model that we can use with the Entity Framework
                var visualStudioUtils = new VisualStudioUtils();
                visualStudioUtils.BuildProject(Context.ActiveProject);

                Type reflectedModelType = GetReflectionType(modelType.FullName);
                if (reflectedModelType == null)
                {
                    throw new InvalidOperationException(Resources.WebFormsScaffolder_ProjectNotBuilt);
                }
            }
        }

        // Called to ensure that the project was compiled successfully
        private Type GetReflectionType(string typeName)
        {
            return GetService<IReflectedTypesService>().GetType(Context.ActiveProject, typeName);
        }

        // We are just pulling in some dependent nuget packages
        // to meet "Web Application Project" experience in this change.
        // There are some open questions regarding the experience for
        // webforms scaffolder in the case of an empty project.
        // Those details need to be worked out and
        // depending on that, we would modify the list of packages below
        // or conditions which determine when they are installed etc.
        public override IEnumerable<NuGetPackage> Dependencies
        {
            get
            {
                List<NuGetPackage> t = new List<NuGetPackage>();
                
                t.Add(new NuGetPackage("BlueMarble.Shared.Azure",
                                       "1.0.53",
                                       new NuGetSourceRepository()));

                return (IEnumerable<NuGetPackage>)t;

                //return null;// GetService<IEntityFrameworkService>().Dependencies;
            }
        }

        private TService GetService<TService>() where TService : class
        {
            return (TService)ServiceProvider.GetService(typeof(TService));
        }

        // Returns the relative path of the folder selected in Visual Studio or an empty
        // string if no folder is selected.
        protected string GetSelectionRelativePath()
        {
            return Context.ActiveProjectItem == null ? String.Empty : ProjectItemUtils.GetProjectRelativePath(Context.ActiveProjectItem);
        }

        // If a Visual Studio folder is selected then returns the folder's namespace, otherwise
        // returns the project namespace.
        protected string GetDefaultNamespace()
        {
            //return Context.ActiveProjectItem == null
            //    ? Context.ActiveProject.GetDefaultNamespace()
            //    : Context.ActiveProjectItem.GetDefaultNamespace();

            return Context.ActiveProject.GetDefaultNamespace();
        }

        // Create a dictionary that maps foreign keys to related models. We only care about associations
        // with a single key (so we can display in a DropDownList)
        protected IDictionary<string, RelatedModelMetadata> GetRelatedModelDictionary(ModelMetadata efMetadata)
        {
            var dict = new Dictionary<string, RelatedModelMetadata>();

            foreach (var relatedEntity in efMetadata.RelatedEntities)
            {
                if (relatedEntity.ForeignKeyPropertyNames.Count() == 1)
                {
                    dict[relatedEntity.ForeignKeyPropertyNames[0]] = relatedEntity;
                }
            }
            return dict;
        }

        private static string GetPluralizedName(string Name)
        {
            var pluralizationService = System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(System.Threading.Thread.CurrentThread.CurrentUICulture);
            string pluralizedName = pluralizationService.Pluralize(Name);

            if (pluralizedName.EndsWith("us"))
                pluralizedName = pluralizedName + "es";

            return pluralizedName;
        }

        private static Dictionary<string, ScaffoldProperty> GetScaffoldProperties(CodeType modelType)
        {
            Dictionary<string, ScaffoldProperty> scaffoldProperties = new Dictionary<string, ScaffoldProperty>();

            foreach (CodeElement child in modelType.GetPublicMembers())
            {
                if (child is EnvDTE.CodeProperty)
                {
                    if (!s_ExcludedScaffoldProperties.Contains(child.Name))
                    {
                        var scaffoldProperty = new ScaffoldProperty()
                        {
                            TypeFullName = (child as EnvDTE.CodeProperty).Type.AsFullName
                        };

                        foreach (CodeAttribute codeAttribute in (child as EnvDTE.CodeProperty).Attributes)
                        {
                            if (codeAttribute.FullName == typeof(BlueMarble.Shared.Azure.Storage.Table.RelatedTableAttribute).FullName)
                            {
                                scaffoldProperty.RelatedPropertyType = codeAttribute.GetValueFromAttribute<Type>("Type") as Type;
                            }
                        }

                        scaffoldProperties.Add(child.Name, scaffoldProperty);
                    }
                }
            }

            return scaffoldProperties;
        }
    }
}