using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace Konfidence.NL.SqlHelper
{
  /// <summary>
	/// Summary description for SQLExecuter.
	/// </summary>
  public class SqlExecuter : IDisposable
  {
    // NB: de SqlExecuter zou singleton gedrag moeten vertonen, maar dit kan niet voor batches.
//    static  private SqlConnection  sqlConnection = null; // TODO : sqlConnection = potential singleton 
//    static  private SqlDataReader  sqlDataReader = null; 
    private SqlConnection  sqlConnection; // TODO : sqlConnection = potential singleton 
    private SqlDataReader  sqlDataReader; 
    private SqlCommand     sqlCommand;
    private bool          _Eof         = true;
    private bool _Disposed;

    private string _UserId;
    private string _Password;
    private string _DataSource;
    private string _InitialCatalog;

    private Mutex _SaveSessionInfoMutex;

    #region Hidden Property MemberFields
    
    private string        _SqlQueryText;
    
    #endregion
    
    public /*RW property*/ string       SqlQueryText    
    {
      get {return _SqlQueryText;}
      set
      {
        _SqlQueryText = value;

        if (sqlDataReader != null)
        {
          this.Close();
        }

        if (sqlCommand == null) 
        {
          sqlCommand = new SqlCommand(_SqlQueryText, sqlConnection);
        }
        else
        {
          sqlCommand.CommandText = _SqlQueryText;
        }
            
        if (sqlConnection.State != ConnectionState.Open) 
        {
          sqlConnection.Open();
        }
      }
    }
    
    public /*RW property*/ string       SqlCommandText  
    {
      get 
      {
        return _SqlQueryText;
      }
      set 
      {
        _SaveSessionInfoMutex.WaitOne();

        SqlQueryText = value;

        sqlCommand.ExecuteNonQuery();

        _SaveSessionInfoMutex.ReleaseMutex();
      }
    }
    
    public /*R  property*/ IDataRecord  ResultSet       
    {
      get 
      { 
        if (sqlCommand != null)
        {
          if (sqlDataReader == null)
          {
            sqlDataReader = sqlCommand.ExecuteReader();  // refill
            _Eof = !sqlDataReader.Read();
          }
        }
        else
        {
          sqlDataReader = null;
        }

        return sqlDataReader;
      }
    }
    
    public /*R  property*/ bool         HasRows         
    {
      get 
      {
        bool hasRows = false;
      
        if (ResultSet != null)
        {
          hasRows = sqlDataReader.HasRows;
        }
      
        return hasRows;
      }
    }    
    
    public /*R  property*/ bool         Eof             
    {
      get
      {
        return _Eof;
      }
    }

    
    public void Next()
    {
      _Eof = !sqlDataReader.Read();

      if (_Eof)
      {
        this.Close();
      }
    }

    
    public SqlExecuter(string userId, string password, string dataSource, string initialCatalog)
    {
			if (userId == null)
				throw new ArgumentNullException("userId");
			if (password == null)
				throw new ArgumentNullException("password");
			if (dataSource == null)
				throw new ArgumentNullException("dataSource");
			if (initialCatalog == null)
				throw new ArgumentNullException("initialCatalog");

			string connectionString = "packet size=4096;persist security info=True";
      bool requestInitialOwnership = true;

      _UserId = userId;
      _Password = password;
      _DataSource = dataSource;
      _InitialCatalog = initialCatalog;

      if (userId.Length == 0 || dataSource.Length == 0)
        return;

      connectionString += ";user id=" + userId;

      if (password.Length > 0)
        connectionString += ";password=" + password;

      connectionString += ";data source=" + dataSource;

      if (initialCatalog.Length > 0)
        connectionString += ";initial catalog=" + initialCatalog;

      connectionString += ";Max Pool Size=75000"; // volgens mij is dit bullshit of anders : pooling=false 
      // probleem wat soms optreedt: pooled connection heeft een timeout, maar wordt gereused, 
      // dit geeft een exception. 
      // MAAR, dit treedt hier altijd bij hetzelfde statement op PageHitClass.SaveSessionInfo() na state=4
      // dit zou ik ook bij andere executes verwachten.

      if (_SaveSessionInfoMutex == null)
      {
        _SaveSessionInfoMutex = new Mutex(requestInitialOwnership, Guid.NewGuid().ToString());
      }
      
      if (sqlConnection == null)
      {
        sqlConnection = new SqlConnection(connectionString);
      }
      else
      {
        if (sqlConnection.ConnectionString != connectionString)
          sqlConnection.ConnectionString = connectionString;
      }

      if (sqlConnection.State != ConnectionState.Open) 
        sqlConnection.Open();
    }
      
    // public void Close()  
    // deze method tijdelijk private gemaakt ivm een bug 
    // de sqlDataReader werd niet geclosed en genilled als de commandText werd vernieuwd
    // een oplossing hiervoor was dat de datareader handmatig van buitenuit werd geclosed 
    // dit was een hack en niet en fix en moet dus niet zo gedaan worden. daarom checken 
    // waarom een close wordt aangeroepen en eventueel aanpassen.
    
    private void Close()
    {
      sqlDataReader.Close();
      sqlDataReader = null;
    }
    
    public SqlExecuter Clone()
    {
      return new SqlExecuter(_UserId, _Password, _DataSource, _InitialCatalog);
    }

		public void FillDataTable(System.Data.DataTable dataTable)
		{
			if (dataTable == null)
				throw new ArgumentNullException("dataTable");

			sqlDataReader.Close();
			sqlDataReader = sqlCommand.ExecuteReader();  // refill
			dataTable.Load(sqlDataReader);
		}
/*    
    ~SqlExecuter()
    {
      sqlConnection.Close();
    }
*/    
    #region IDisposable Members

    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {

      if(!this._Disposed)
      {
        if (_SaveSessionInfoMutex != null)
        {
          _SaveSessionInfoMutex.Close();      
        }
        if (sqlCommand != null)
        {
          sqlCommand.Dispose();
        }
        if (sqlConnection != null)
        {
          sqlConnection.Close();
        }
      }
      _Disposed = true;

    }

    #endregion
  }
}
