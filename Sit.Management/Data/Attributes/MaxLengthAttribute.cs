using System;

namespace Sit.Management.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxLengthAttribute : Attribute
    {
        private readonly int _maxLength;

        public MaxLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DbFieldNameAttribute : Attribute
    {
        public DbFieldNameAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public string FieldName
        {
            get;
            private set;
        }
    }
}