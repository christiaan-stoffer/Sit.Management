using System;

namespace Sit.Management.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        private string _tableName;

        public TableAttribute(string tableName)
        {
            _tableName = tableName;
        }
    }
}