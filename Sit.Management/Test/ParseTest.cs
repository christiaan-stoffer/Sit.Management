using System;
using System.Data.SqlClient;
using System.Diagnostics;
using NUnit.Framework;
using Sit.Management.Data;
using Sit.Management.Data.Schema;

namespace Sit.Management.Test
{
    [TestFixture]
    public class ParseTest
    {
        public class Contact : IContact
        {
            public string Initials
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public Guid Id
            {
                get;
                set;
            }

            public int? Number
            {
                get;
                set;
            }
        }

        [Test]
        public void ParserTest()
        {
            var c = new Contact();

            c.Id = Guid.NewGuid();
            c.Name = "Test";
            c.Initials = "WJM";

            var now = DateTime.Now;
            var parser = ParserFactory.GetParserForIDbCommand<IContact>();

            Trace.WriteLine("ParseIntoIDbCommand created in: " + (DateTime.Now - now).TotalSeconds + " seconds");

            var cmd = new SqlCommand();
            parser(cmd, c);

            Assert.AreEqual(3, cmd.Parameters.Count);

        }
    }
}