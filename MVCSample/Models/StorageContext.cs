using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSample.Models
{
    public class StorageContext: BlueMarble.Shared.Azure.Storage.Table.StorageContext
    {
        public StorageContext(Microsoft.WindowsAzure.Storage.CloudStorageAccount StorageAccount)
            : base(StorageAccount)
        {
        }

        public StorageContext()
            : base(new Microsoft.WindowsAzure.Storage.CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    Properties.Settings.Default.StorageAccountName,
                    Properties.Settings.Default.StorageAccountKey), true))
        {
        }

        public override void InitializeTables()
        {
            base.InitializeTables();
        }
    }
}