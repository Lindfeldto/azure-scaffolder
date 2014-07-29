azure-scaffolder
================

Enables scaffolding in MVC5 of Microsoft Azure Table Storage entities.

The Blue Marble Software (Pty) Ltd. Azure Table Storage Scaffolder is a Visual Studio Scaffolding extension that allows scaffolding in ASP MVC projects for Microsoft Azure Table entities.

This is the first release, and many more will come.

The code for this extension is available on GitHub at  https://github.com/bluemarblesoftware/azure-scaffolder.

In order to use this scaffolder, you will need to create Azure Table entities that inherit from BlueMarble.Shared.Azure.Table.Entity, see the sample above.

You will also need to create a StorageContext class that inherits from BlueMarble.Shared.Azure.Table.StorageContext, see sample above.

To create relationships between two tables, use the RelatedTableAttribute, as per the sample above.

The resulting code will have the following components:
- ApiController: All data interactions are done through an WebApi 2.0 controller. 
- Controller: Only the required code to deal with showing the views. 
- Views: Create, Update, Detail, Delete, Index views are created. Views use Telerik KendoUI components. •Create: Uses KendoUI MVVM components with an HTTP Post to upload the data 
  - Update: Uses KendoUI MVVM components with an HTTP Get to retrieve the data, and an HTTP Put to upload the data 
  - Detail: Uses KendoUI MVVM components with an HTTP Get to retrieve the data 
  - Delete: Uses KendoUI MVVM components with an HTTP Get to retrieve the data 
  - Index: Uses DataTables.net components with an HTTP Get to retrieve the data 

- Scripts: A comprehensive data access script is generated for each table, and included on views where applicable. 
- Models: A partial StorageContext is created containing code relevant to each Microsoft Azure Table entity. 
