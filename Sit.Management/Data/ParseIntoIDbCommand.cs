using System.Data;

namespace Sit.Management.Data
{
    /// <summary>
    ///   ParseIntoIDbCommand delegate. Can parse data into DbParameters for an IDbCommand.
    /// </summary>
    /// <typeparam name="TData"> The type of the contract (interface) of the Data. </typeparam>
    /// <param name="command"> The IDbCommand </param>
    /// <param name="data"> The actual object containing the data </param>
    public delegate void ParseIntoIDbCommand<in TData>(IDbCommand command, TData data);
}