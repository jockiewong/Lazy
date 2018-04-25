using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lazy.EFCore
{
    /// <summary>
    /// 实体元数据
    /// </summary>
    public class EntityMetaData
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 表架构,默认dbo
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 所有的字段
        /// </summary>
        public List<FieldMetaData> Fields { get; set; }
    }
}
