using DBDiff.Schema.Model;
using System;

namespace DBDiff.Schema.SQLServer.Generates.Model
{

    
    public class Default:SQLServerSchemaBase 
    {
        private string value;

        public Default(ISchemaBase parent)
            : base(parent, Enums.ObjectType.Default)
        {
        }

        public Default Clone(ISchemaBase parent)
        {
            Default item = new Default(parent);
            item.Id = this.Id;
            item.Name = this.Name;
            item.Owner = this.Owner;
            item.Value = this.Value;
            return item;
        }
        
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string ToSQLAddBind()
        {
            string sql = "";
            sql += "EXEC sp_bindefault N'" + Name + "', N'" + this.Parent.Name + "'\r\nGO\r\n";
            return sql;
        }

        public string ToSQLAddUnBind()
        {
            string sql = "";
            sql += "EXEC sp_unbindefault @objname=N'" + this.Parent.Name + "'\r\nGO\r\n";
            return sql;
        }

        public override string ToSqlAdd()
        {
            return ToSql();
        }

        public override string ToSqlDrop()
        {
            return "DROP DEFAULT " + FullName + "\r\nGO\r\n";
        }

        public override string ToSql()
        {
            //return "CREATE DEFAULT " + FullName + " AS "+value+"\r\nGO\r\n";
            return value + "\r\nGO\r\n";
        }

        /// <summary>
        /// Devuelve el schema de diferencias del Schema en formato SQL.
        /// </summary>
        public override SQLScriptList ToSqlDiff()
        {
            SQLScriptList listDiff = new SQLScriptList();

            if (this.Status == Enums.ObjectStatusType.DropStatus)
            {
                listDiff.Add(ToSqlDrop(), 0, Enums.ScripActionType.DropRule);
            }
            if (this.Status == Enums.ObjectStatusType.CreateStatus)
            {
                listDiff.Add(ToSql(), 0, Enums.ScripActionType.AddRule);
            }
            if (this.Status == Enums.ObjectStatusType.AlterStatus)
            {
                listDiff.Add(ToSqlDrop(), 0, Enums.ScripActionType.DropRule);
                listDiff.Add(ToSql(), 0, Enums.ScripActionType.AddRule);
            }
            return listDiff;
        }

        public bool Compare(Default obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (Name != obj.Name) return false;
            //if (Owner != obj.Owner) return false;
            if (Value != obj.Value) return false;
            return true;
        }
    }
}
