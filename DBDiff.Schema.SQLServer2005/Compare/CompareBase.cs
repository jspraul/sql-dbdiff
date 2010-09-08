using System.Collections.Generic;

using DBDiff.Schema.Model;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Compare
{
    internal abstract class CompareBase<T> where T:ISchemaBase
    {
        protected virtual void DoUpdate<Root>(SchemaList<T, Root> CamposOrigen, T node) where Root:ISchemaBase
        {
            //node.Status = Enums.ObjectStatusType.AlterStatus;//New!!! Need for test!
        }

        protected virtual void DoNew<Root>(SchemaList<T, Root> CamposOrigen, T node) where Root : ISchemaBase
        {
            T newNode = node;//.Clone(CamposOrigen.Parent);
            newNode.Parent = CamposOrigen.Parent;
            newNode.Status = Enums.ObjectStatusType.CreateStatus;
            CamposOrigen.Add(newNode);
        }

        protected void DoDelete(T node)
        {
            node.Status = Enums.ObjectStatusType.DropStatus;
        }

        public void GenerateDiferences<Root>(SchemaList<T, Root> CamposOrigen, SchemaList<T, Root> CamposDestino) 
            where Root : ISchemaBase
        {
            if (CamposOrigen.Parent.GetType() == typeof(Database))
            {
                if (((Database)((ISchemaBase)CamposOrigen.Parent)).Info.Version == DatabaseInfo.VersionNumber.SQLServer2000 || ((Database)((ISchemaBase)CamposDestino.Parent)).Info.Version == DatabaseInfo.VersionNumber.SQLServer2000)
                {
                    string type = CamposOrigen.GetType().ToString();
                    string[] typeNon2000 = { typeof(SchemaList<Assembly, Database>).ToString(), typeof(SchemaList<CLRFunction, Database>).ToString(), typeof(SchemaList<DBDiff.Schema.SQLServer.Generates.Model.Schema, Database>).ToString(), typeof(SchemaList<CLRStoreProcedure, Database>).ToString(), typeof(SchemaList<XMLSchema, Database>).ToString(), typeof(SchemaList<Synonym, Database>).ToString(), typeof(SchemaList<PartitionFunction, Database>).ToString(), typeof(SchemaList<PartitionScheme, Database>).ToString() };
                    foreach (string s in typeNon2000)
                    {
                        if (type == s) return;
                    }
                }
            }
            bool has = true;
            int DestinoIndex = 0;
            int OrigenIndex = 0;
            int DestinoCount = CamposDestino.Count;
            int OrigenCount = CamposOrigen.Count;
            T node;
            
            while (has)
            {
                has = false;
                if (DestinoCount > DestinoIndex)
                {
                    node = CamposDestino[DestinoIndex];
                    if (!CamposOrigen.Exists(node.FullName))
                        DoNew<Root>(CamposOrigen, node);
                    else
                        DoUpdate<Root>(CamposOrigen, node);

                    DestinoIndex++;
                    has = true;
                }

                if (OrigenCount > OrigenIndex)
                {
                    node = CamposOrigen[OrigenIndex];
                    if (!CamposDestino.Exists(node.FullName))
                        DoDelete(node);

                    OrigenIndex++;
                    has = true;
                }
            }
        }

        protected static void CompareExtendedProperties(ISQLServerSchemaBase origen, ISQLServerSchemaBase destino)
        {
            
            /*List<ExtendedProperty> dropList = (from node in origen.ExtendedProperties
                                               where !destino.ExtendedProperties.Exists(item => item.Name.Equals(node.Name, StringComparison.CurrentCultureIgnoreCase))
                                               select node).ToList<ExtendedProperty>();*/

            List<ExtendedProperty> dropList =new List<ExtendedProperty>();
            foreach(ExtendedProperty n in origen.ExtendedProperties)
            {
                if (!destino.ExtendedProperties.Exists(n.Name)) dropList.Add(n);
            }

            /*List < ExtendedProperty > addList = (from node in destino.ExtendedProperties
                                              where !origen.ExtendedProperties.Exists(item => item.Name.Equals(node.Name, StringComparison.CurrentCultureIgnoreCase))
                                               select node).ToList<ExtendedProperty>();*/
            List<ExtendedProperty> addList = new List<ExtendedProperty>();
            foreach (ExtendedProperty n in destino.ExtendedProperties)
            {
                if (!origen.ExtendedProperties.Exists(n.Name)) addList.Add(n);
            }
            dropList.ForEach(item => { item.Status = Enums.ObjectStatusType.DropStatus; });
            addList.ForEach(item => { item.Status = Enums.ObjectStatusType.CreateStatus; });
            origen.ExtendedProperties.AddRange(addList);
        }

        /*private static bool EqualsName(ExtendedProperty s, String name)
        {
            String name;
            if (s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

    }
}
