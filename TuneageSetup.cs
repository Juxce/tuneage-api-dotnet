using System;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

public class Setup
{
	public Setup()
	{
	    {
	        //Connect to the local, default instance of SQL Server.   
	        Server srv;
	        srv = new Server();
	        //The connection is established when a property is requested.   
	        Console.WriteLine(srv.Information.Version);
	    }
	    //The connection is automatically disconnected when the Server variable goes out of scope.

    }
}
