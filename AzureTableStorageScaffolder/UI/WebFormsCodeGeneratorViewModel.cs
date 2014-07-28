using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EnvDTE;
using EnvDTE80;
using AzureTableStorageScaffolder;
using Microsoft.AspNet.Scaffolding;
using AzureTableStorageScaffolder.Extensions;

namespace AzureTableStorageScaffolder.UI
{
    internal class WebFormsCodeGeneratorViewModel : ViewModel<WebFormsCodeGeneratorViewModel>
    {
        private ObservableCollection<ModelType> _dbContextTypeCollection;
        private ObservableCollection<ModelType> _modelTypeCollection;
        private List<String> _projectPaths;

        private readonly CodeGenerationContext _context;
        private readonly ModelType _addNewDataContextItem =
            new ModelType(typeName: null) { DisplayName = Resources.WebFormsCodeGeneratorViewModel_AddNewDbContext };

        public WebFormsCodeGeneratorViewModel(CodeGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
            _useMasterPage = true;
            _overwrite = false;

            _generateController = true;
            _generateApiController = true;
            _generateStorageContext = true;
            _generateViews = true;
            _generateScripts = true;
        }

        private DelegateCommand _okCommand;

        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new DelegateCommand(_ =>
                    {
                        Validate(propertyName: null);

                        if (!HasErrors)
                        {
                            if (DbContextModelType == null || DbContextModelType == _addNewDataContextItem)
                            {
                                OnPromptForNewDataContext();
                            }

                            if (DbContextModelType != null && DbContextModelType != _addNewDataContextItem)
                            {
                                OnClose(result: true);
                            }
                        }
                    });
                }

                return _okCommand;
            }
        }

        public event Action<WebFormsNewDataContextViewModel> PromptForNewDataContextTypeName;

        private void OnPromptForNewDataContext()
        {
            if (PromptForNewDataContextTypeName != null)
            {
                var viewModel = new WebFormsNewDataContextViewModel(DefaultDataContextTypeName);

                PromptForNewDataContextTypeName(viewModel);

                if (!viewModel.Canceled)
                {
                    var newDbContextModelType = new ModelType(viewModel.DbContextTypeName);
                    DataContextTypeCollection.Add(newDbContextModelType);
                    DbContextModelType = newDbContextModelType;
                }
                else
                {
                    DbContextModelType = null;
                    DbContextModelTypeName = null;
                }
            }
        }

        public event Action<bool> Close;

        private void OnClose(bool result)
        {
            if (Close != null)
            {
                Close(result);
            }
        }

        private ModelType _modelType;

        public ModelType ModelType
        {
            get { return _modelType; }
            set
            {
                Validate();

                if (value == _modelType)
                {
                    return;
                }

                _modelType = value;
                OnPropertyChanged();
            }
        }

        private string _modelTypeName;

        public string ModelTypeName
        {
            get { return _modelTypeName; }
            set
            {
                Validate();

                if (value == _modelTypeName)
                {
                    return;
                }

                _modelTypeName = value;
                if (ModelType != null)
                {
                    if (ModelType.DisplayName.StartsWith(_modelTypeName, StringComparison.Ordinal))
                    {
                        _modelTypeName = ModelType.DisplayName;
                    }
                    else
                    {
                        ModelType = null;
                    }
                }
                OnPropertyChanged();
            }
        }

        private ModelType _dbContextModelType;

        public ModelType DbContextModelType
        {
            get { return _dbContextModelType; }
            set
            {
                Validate();

                if (value == _dbContextModelType)
                {
                    return;
                }

                _dbContextModelType = value;

                if (_dbContextModelType == _addNewDataContextItem)
                {
                    // OnPromptForNewDataContext();
                    return;
                }

                OnPropertyChanged();
            }
        }

        private string _dbContextModelTypeName;

        public string DbContextModelTypeName
        {
            get { return _dbContextModelTypeName; }
            set
            {
                Validate();

                if (value == _dbContextModelTypeName)
                {
                    return;
                }

                _dbContextModelTypeName = value;
                if (DbContextModelType != null)
                {
                    if (DbContextModelType.DisplayName.StartsWith(_dbContextModelTypeName, StringComparison.Ordinal))
                    {
                        _dbContextModelTypeName = DbContextModelType.DisplayName;
                    }
                    else
                    {
                        DbContextModelType = null;
                    }
                }
                OnPropertyChanged();
            }
        }


        private bool _useMasterPage;

        public bool UseMasterPage
        {
            get { return _useMasterPage; }
            set
            {
                Validate();

                if (value == _useMasterPage)
                {
                    return;
                }

                _useMasterPage = value;
                OnPropertyChanged();
                OnPropertyChanged(m => m.UseMasterPage);
            }
        }



        private string _desktopMasterPage;

        public string DesktopMasterPage
        {
            get { return _desktopMasterPage; }
            set
            {
                Validate();

                if (value == _desktopMasterPage)
                {
                    return;
                }

                _desktopMasterPage = value;

                OnPropertyChanged();
            }
        }

        private bool _overwrite;

        public bool Overwrite
        {
            get { return _overwrite; }
            set
            {
                Validate();

                if (value == _overwrite)
                {
                    return;
                }

                _overwrite = value;
                OnPropertyChanged();
            }
        }

        private bool _generateScripts;

        public bool GenerateScripts
        {
            get { return _generateScripts; }
            set
            {
                Validate();

                if (value == _generateScripts)
                {
                    return;
                }

                _generateScripts = value;
                OnPropertyChanged();
            }
        }

        private bool _generateController;

        public bool GenerateController
        {
            get { return _generateController; }
            set
            {
                Validate();

                if (value == _generateController)
                {
                    return;
                }

                _generateController = value;
                OnPropertyChanged();
            }
        }

        private bool _generateApiController;

        public bool GenerateApiController
        {
            get { return _generateApiController; }
            set
            {
                Validate();

                if (value == _generateApiController)
                {
                    return;
                }

                _generateApiController = value;
                OnPropertyChanged();
            }
        }

        private bool _generateStorageContext;

        public bool GenerateStorageContext
        {
            get { return _generateStorageContext; }
            set
            {
                Validate();

                if (value == _generateStorageContext)
                {
                    return;
                }

                _generateStorageContext = value;
                OnPropertyChanged();
            }
        }

        private bool _generateViews;

        public bool GenerateViews
        {
            get { return _generateViews; }
            set
            {
                Validate();

                if (value == _generateViews)
                {
                    return;
                }

                _generateViews = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ModelType> ModelTypeCollection
        {
            get
            {
                if (_modelTypeCollection == null)
                {
                    ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
                    Project project = _context.ActiveProject;


                    var modelTypes = codeTypeService
                                        .GetAllCodeTypes(project)
                                        .Where(codeType => codeType.IsValidAzureTableEntity())
                                        .Select(codeType => new ModelType(codeType));
                    _modelTypeCollection = new ObservableCollection<ModelType>(modelTypes);
                }
                return _modelTypeCollection;
            }

            set
            {
                _modelTypeCollection = value;
            }
        }


        private bool IsReallyValidWebProjectEntityType(CodeType codeType)
        {
            return !IsAbstract(codeType);
        }

        private bool IsAbstract(CodeType codeType) {
            CodeClass2 codeClass2 = codeType as CodeClass2;
            if (codeClass2 != null) {
                return codeClass2.IsAbstract;
            } else {
                return false;
            }
        }


        public ObservableCollection<ModelType> DataContextTypeCollection
        {
            get
            {
                if (_dbContextTypeCollection == null)
                {
                    ICodeTypeService codeTypeService = GetService<ICodeTypeService>();
                    Project project = _context.ActiveProject;

                    var modelTypes = codeTypeService
                                        .GetAllCodeTypes(project)
                                        .Where(codeType => codeType.IsValidAzureStorageContext())
                                        .Select(codeType => new ModelType(codeType));

                    _dbContextTypeCollection = new ObservableCollection<ModelType>(modelTypes);
                    //_dbContextTypeCollection.Insert(0, _addNewDataContextItem);
                }
                return _dbContextTypeCollection;
            }
        }

        public string DefaultDataContextTypeName
        {
            get
            {
                var project = _context.ActiveProject;

                return String.Format(CultureInfo.InvariantCulture, "{0}.Models.{1}Context", project.GetDefaultNamespace(), project.Name);
            }
        }

        protected override void Validate([CallerMemberName]string propertyName = "")
        {
            string currentPropertyName;

            // ModelType
            currentPropertyName = PropertyName(m => m.ModelType);
            if (ShouldValidate(propertyName, currentPropertyName))
            {
                ClearError(currentPropertyName);
                
                if (ModelTypeCollection.Where(m => m.Selected).Count() == 0)
                {
                    AddError(currentPropertyName, WebFormsScaffolderDialogResources.Error_ModelTypeRequired);
                }
            }

            // DesktopMasterPage
            currentPropertyName = PropertyName(m => m.DesktopMasterPage);
            if (ShouldValidate(propertyName, currentPropertyName))
            {
                ClearError(currentPropertyName);
            }
        }

        // Do a breadth first search for project paths.
        // Does it make sense to move this logic to architecture? As part of UI consistency feature, there will be
        // a file picker dialog for picking the layouts and this implementation should go away.
        private List<string> ProjectPaths
        {
            get
            {
                if (_projectPaths == null)
                {
                    var project = _context.ActiveProject;
                    var projectRoot = project.GetFullPath();
                    _projectPaths = new List<String>();

                    var projectItems = new Stack<ProjectItems>();
                    projectItems.Push(project.ProjectItems);

                    while (projectItems.Any())
                    {
                        var currentItems = projectItems.Pop();
                        foreach (ProjectItem item in currentItems)
                        {
                            if (item.Kind == VsConstants.VSProjectItemKindPhysicalFile)
                            {
                                var fullPath = item.GetFullPath();
                                _projectPaths.Add(fullPath.Substring(projectRoot.Length));
                            }
                            if (item.Kind == VsConstants.VSProjectItemKindPhysicalFolder)
                            {
                                projectItems.Push(item.ProjectItems);
                            }
                        }
                    }
                }
                return _projectPaths;
            }
        }

        private TService GetService<TService>() where TService : class
        {
            return (TService)_context.ServiceProvider.GetService(typeof(TService));
        }
    }
}
