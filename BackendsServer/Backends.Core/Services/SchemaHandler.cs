using Backends.Core.DataEngine;
using Backends.Core.Extension;
using Backends.Core.Model;
using BackendsCommon.Types;
using BackendsCommon.Types.BacksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Services
{
    public class SchemaHandler
    {
        public static BacksProjectSchema CreateDefaultSchema(string projectId)
        {
            return new BacksProjectSchema()
            {
                AppId = projectId,
                EntityColumnTypeMapping = new Dictionary<string, EntitiesSchema>()
                    {
                        {
                            "_User", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "Id", BacksDataType.BString},
									{ "AppId", BacksDataType.BString},
									{ "UserName", BacksDataType.BString},
                                    { "Password", BacksDataType.BString},
                                    { "CreatedAt", BacksDataType.BTime},
                                    { "UpdatedAt", BacksDataType.BTime},
                                    //{ "Data", BacksDataType.BObject}
                                }
                            }
                        },
                        {
                            "_Session", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "Id", BacksDataType.BString},
									{ "AppId", BacksDataType.BString},
									{ "Token", BacksDataType.BString},
                                    { "CreatedAt", BacksDataType.BTime},
                                    { "UpdatedAt", BacksDataType.BTime},
                                    { "ExpiresAt", BacksDataType.BTime},
                                    //{ "InstallationId", BacksDataType.BString},
                                    //{ "Data", BacksDataType.BString},
                                    //{ "Previleges", BacksDataType.BBoolean},
                                    { "PUser", BacksDataType.BPointer}
                                }
                            }
                        },
                        {
                            "_Roles", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "Id", BacksDataType.BString},
									{ "AppId", BacksDataType.BString},
									{ "Name", BacksDataType.BString},
                                    { "Paswword", BacksDataType.BString},
                                    { "CreatedAt", BacksDataType.BTime},
                                    { "UpdatedAt", BacksDataType.BTime},
                                    //{ "Data", BacksDataType.BObject}
                                }
                            }
                        }

                    }

            };
        }

	    //public Dictionary<string, string> GetSchema(Dictionary<string, object> data)
		public EntitiesSchema GetSchema(Dictionary<string, object> data)
	    {
		    var columnTypeMapping = new EntitiesSchema()
		    {
			    ColumnTypeMapping = new Dictionary<string, string>()
				{
					{ "Id", BacksDataType.BString},
					{ "Name", BacksDataType.BString},
					{ "CreatedAt", BacksDataType.BTime}

				}
			};

			foreach (var item in data)
			{
				columnTypeMapping.ColumnTypeMapping[item.Key]= TypeConverter(item.Value);
			}

		    return columnTypeMapping;
	    }
		
		private string TypeConverter(object param)
	    {
			Type t = param.GetType();
		    if (t.Equals(typeof(string)))
			    return BacksDataType.BString;

		    else if (t.Equals(typeof(long)))
				return BacksDataType.BInt;
		    else if (t.Equals(typeof(DateTime)))
			    return BacksDataType.BTime;
		    else 
			    return BacksDataType.BString;

		}

        //private readonly IRepositoryAsync _repo;

        public SchemaHandler(IRepositoryAsync repo)
        {
            //_repo = repo;
        }

      /*  public bool IsSchemaValid<T>(BacksProjectSchema schema, T obj)  where T: BacksObject
        {
            if (schema == null)
                return false;

            EntitiesSchema entitySchema = schema.EntityColumnTypeMapping
                                                .Where(entity => entity.Key == obj.Name)
                                                .Select(kvp => kvp.Value).FirstOrDefault();

			var dictObject = obj.AsBacksTypeDictionary();


			return true; 
        }*/
    }
}
