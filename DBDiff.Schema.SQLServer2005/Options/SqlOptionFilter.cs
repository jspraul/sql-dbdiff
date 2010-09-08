
using System.Collections.ObjectModel;
using System;

namespace DBDiff.Schema.SQLServer.Generates.Options
{
    
    public class SqlOptionFilter
    {
        
        private Collection<SqlOptionFilterItem> items;

        public SqlOptionFilter()
        {
            items = new Collection<SqlOptionFilterItem>();
            Items.Add(new SqlOptionFilterItem(Enums.ObjectType.Table,"dbo.dtproperties"));
        }

        public Collection<SqlOptionFilterItem> Items
        {
            get { return items; }
        }

    }
}
