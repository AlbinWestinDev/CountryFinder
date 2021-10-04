using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{



    public class SaveEntity : TableEntity
    {

        public SaveEntity()
        {

        }
        public SaveEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
            Created = DateTime.Now;
        }

        public DateTime Created { get; set; }

        public string CountryName { get; set; }
     
    }
}
