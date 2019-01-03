//-----------------------------------------------------------------------
// <copyright file="Setup.cs" company="Tuneage">
//     (c) 2018 Tuneage
// </copyright>
//-----------------------------------------------------------------------
namespace Tuneage.Setup
{
    using System;

    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    /// <summary>
    /// The setup.
    /// </summary>
    public class Setup
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            // Connect to the local, default instance of SQL Server.   
            var srv = new Server();

            // The connection is established when a property is requested.   
            Console.WriteLine(srv.Information.Version);
        }
    }
}
