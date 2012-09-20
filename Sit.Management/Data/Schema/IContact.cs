using System;
using Sit.Management.Data.Attributes;

namespace Sit.Management.Data.Schema
{
    [Table("Contact")]
    public interface IContact
    {
        [MaxLength(10)]
        string Initials
        {
            get;
            set;
        }

        [MaxLength(50)]
        string Name
        {
            get;
            set;
        }

        Guid Id
        {
            get;
            set;
        }

        int? Number
        {
            get;
            set;
        }
    }
}