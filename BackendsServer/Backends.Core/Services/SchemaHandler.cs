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
                                    { "_id", BacksDataType.BString},
                                    { "userName", BacksDataType.BString},
                                    { "paswword", BacksDataType.BString},
                                    { "createdAt", BacksDataType.BTime},
                                    { "updatedAt", BacksDataType.BTime},
                                    { "userData", BacksDataType.BObject}
                                }
                            }
                        },
                        {
                            "_Session", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "_id", BacksDataType.BString},
                                    { "sessionToken", BacksDataType.BString},
                                    { "createdAt", BacksDataType.BTime},
                                    { "updatedAt", BacksDataType.BTime},
                                    { "expiredAt", BacksDataType.BTime},
                                    { "installationId", BacksDataType.BString},
                                    { "sessionData", BacksDataType.BString},
                                    { "previleges", BacksDataType.BBoolean},
                                    { "_p_user", BacksDataType.BPointer}
                                }
                            }
                        },
                        {
                            "_Roles", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "_id", BacksDataType.BString},
                                    { "name", BacksDataType.BString},
                                    { "paswword", BacksDataType.BString},
                                    { "createdAt", BacksDataType.BTime},
                                    { "updatedAt", BacksDataType.BTime},
                                    { "userData", BacksDataType.BObject}
                                }
                            }
                        }

                    }

            };
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
