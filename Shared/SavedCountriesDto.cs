using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class SavedCountriesDto
    {
        public SavedCountriesDto()
        {
            Created = DateTime.Now;
        }
        public string Name { get; set; }
        public DateTime Created { get; set; }

    }


    public class SaveEntity : TableEntity
    {
        public SaveEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

    

        public string CountryName { get; set; }
     
    }
}
