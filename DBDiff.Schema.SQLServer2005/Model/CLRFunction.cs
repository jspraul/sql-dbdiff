using System;
using System.Collections.Generic;

using DBDiff.Schema.Model;

namespace DBDiff.Schema.SQLServer.Generates.Model
{
    public class CLRFunction : CLRCode
    {
        private List<Parameter> parameters;
        private Parameter returnType;
        private bool isAggrFunc;
        public CLRFunction(ISchemaBase parent)
            : base(parent, Enums.ObjectType.CLRFunction, Enums.ScripActionType.AddFunction, Enums.ScripActionType.DropFunction)
        {
            parameters = new List<Parameter>();
            returnType = new Parameter();
            isAggrFunc = false;
        }

        public List<Parameter> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public bool IsAggregateFunction
        {
            get { return isAggrFunc; }
            set { isAggrFunc = value; }
        }

        public Parameter ReturnType
        {
            get { return returnType; }
        }

        public override string ToSql()
        {
            string sql = "CREATE " + (isAggrFunc ? "AGGREGATE " : "FUNCTION ") + FullName + "";
            string param = "";
            parameters.ForEach(item => param += item.ToSql() + ",");
            if (!String.IsNullOrEmpty(param))
            {
                param = param.Substring(0, param.Length - 1);
                sql += " (" + param + ")\r\n";
            }
            else
                sql += "()\r\n";
            sql += "RETURNS " + returnType.ToSql() + " ";
            if (!isAggrFunc) sql += "WITH EXECUTE AS " + AssemblyExecuteAs + "\r\n"+ "AS\r\n";
            sql += "EXTERNAL NAME [" + AssemblyName + "]" + (AssemblyClass == "" ? "" : ".[" + AssemblyClass + "]") + (AssemblyMethod == "" ? "" : ".[" + AssemblyMethod + "]") + "\r\n";
            sql += "GO\r\n";
            return sql;
        }

        public override SQLScriptList ToSqlDiff()
        {
            SQLScriptList list = new SQLScriptList();

            if (this.HasState(Enums.ObjectStatusType.DropStatus))
                list.Add(Drop());
            if (this.HasState(Enums.ObjectStatusType.CreateStatus))
                list.Add(Create());
            if (this.Status == Enums.ObjectStatusType.AlterStatus)
            {
                list.AddRange(Rebuild());
            }
            list.AddRange(this.ExtendedProperties.ToSqlDiff());
            return list;
        }
    }
}
