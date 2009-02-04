// MyClass.cs created with MonoDevelop
// User: xavier at 18:45 19/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections.Generic;
using Mono.Data;

namespace Supos.Core
{
	public class SuposDbProvider
	{
		private IDbConnection connection;
		
		private IDbDataAdapter categoriesAdapter;
		public IDbDataAdapter CategoriesAdapter
		{
			get { return categoriesAdapter; }
		}
		
		private IDbDataAdapter taxesAdapter;
		public IDbDataAdapter TaxesAdapter
		{
			get { return taxesAdapter; }
		}
		
		private IDbDataAdapter productsAdapter;
		public IDbDataAdapter ProductsAdapter
		{
			get { return productsAdapter; }
		}
		
		private IDbDataAdapter customersAdapter;
		public IDbDataAdapter CustomersAdapter
		{
			get { return customersAdapter; }
		}
		
		private IDbDataAdapter paymentsAdapter;
		public IDbDataAdapter PaymentsAdapter
		{
			get { return paymentsAdapter; }
		}
		
		private IDbDataAdapter ordersAdapter;
		public IDbDataAdapter OrdersAdapter
		{
			get { return ordersAdapter; }
		}
		
		private IDbDataAdapter orderdetailsAdapter;
		public IDbDataAdapter OrderDetailsAdapter
		{
			get { return orderdetailsAdapter; }
		}
		
		private IDbDataAdapter metaAdapter;
		public IDbDataAdapter MetaAdapter
		{
			get { return metaAdapter; }
		}
		
		public SuposDbProvider(DbSettings config)
		{
			string constr = "factory=";
			switch(config.DbType) {
			case "SQLite" :
				constr += "Sqlite;";
				constr += "Data Source=";
				constr += config.Database;
				break;
			case "PostgreSQL" :
				constr += "Npgsql;";
				constr += "Server=";
				constr += config.Server;
				constr += ";Port=";
				constr += config.Port;
				constr += ";User ID=";
				constr += config.User;
				constr += ";Password=";
				constr += config.Password;
				constr += ";Database=";
				constr += config.Database;
				break;
			default :
				break;
			}
				
			connection = ProviderFactory.CreateConnection(constr);
			CreateAdapters();
		}

		private void CreateAdapters()
		{
			// Categories
			IDbCommand catcmd=connection.CreateCommand();
			catcmd.CommandText="SELECT Id, Name, Icon FROM Categories";
			categoriesAdapter=ProviderFactory.CreateDataAdapter(catcmd);
			categoriesAdapter.TableMappings.Add("Table", "Categories");
			// Taxes
			IDbCommand taxcmd=connection.CreateCommand();
			taxcmd.CommandText="SELECT Id, Name, Rate, Overrideable FROM Taxes";
			taxesAdapter=ProviderFactory.CreateDataAdapter(taxcmd);
			taxesAdapter.TableMappings.Add("Table", "Taxes");
			// Products
			IDbCommand prodcmd=connection.CreateCommand();
			prodcmd.CommandText="SELECT Id, Name, Icon, Price, CategoryId, DefaultTaxId FROM Products";
			productsAdapter=ProviderFactory.CreateDataAdapter(prodcmd);
			productsAdapter.TableMappings.Add("Table", "Products");
			// Customers
			IDbCommand custcmd=connection.CreateCommand();
			custcmd.CommandText="SELECT Id, Name FROM Customers";
			customersAdapter=ProviderFactory.CreateDataAdapter(custcmd);
			customersAdapter.TableMappings.Add("Table", "Customers");
			// Payments
			IDbCommand paycmd=connection.CreateCommand();
			paycmd.CommandText="SELECT Id, Name, Allowed FROM Payments";
			paymentsAdapter=ProviderFactory.CreateDataAdapter(paycmd);
			paymentsAdapter.TableMappings.Add("Table", "Payments");
			
			// Orders
			IDbCommand orderscmd= connection.CreateCommand();
			orderscmd.CommandText="SELECT Id, CustomerId, PaymentId, TaxId FROM Orders";
			ordersAdapter= ProviderFactory.CreateDataAdapter(orderscmd);
			IDbCommand ordersinsert= connection.CreateCommand();
			ordersinsert.CommandText= "INSERT INTO Orders(Id, CustomerId, PaymentId, TaxId) VALUES(:ID, :CUSTOMERID, :PAYMENTID, :TAXID)";
			ordersinsert.Connection=connection;
			IDbCommand ordersupdate= connection.CreateCommand();
			ordersupdate.CommandText= "UPDATE Orders SET Id=:ID, CustomerId=:CUSTOMERID, PaymentId=:PAYMENTID, TaxId=:TAXID WHERE Id=:ID";
			IDbCommand ordersdelete= connection.CreateCommand();
			ordersdelete.CommandText= "DELETE FROM Orders WHERE Id=:ID";
			
			ordersAdapter.TableMappings.Add("Table", "Orders");

			IDbDataParameter paramid = ordersinsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.String;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramcust = ordersinsert.CreateParameter();
			paramcust.ParameterName= ":CUSTOMERID";
			paramcust.DbType= DbType.Int64;
			paramcust.SourceColumn= "CustomerId";
			paramcust.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter parampay = ordersinsert.CreateParameter();
			parampay.ParameterName= ":PAYMENTID";
			parampay.DbType= DbType.Int64;
			parampay.SourceColumn= "PaymentId";
			parampay.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramtax = ordersinsert.CreateParameter();
			paramtax.ParameterName= ":TAXID";
			paramtax.DbType= DbType.Int64;
			paramtax.SourceColumn= "TaxId";
			paramtax.SourceVersion=DataRowVersion.Current;
			
			ordersinsert.Parameters.Add(paramid);
			ordersinsert.Parameters.Add(paramcust);
			ordersinsert.Parameters.Add(parampay);
			ordersinsert.Parameters.Add(paramtax);
			
			ordersupdate.Parameters.Add(paramid);
			ordersupdate.Parameters.Add(paramcust);
			ordersupdate.Parameters.Add(parampay);
			ordersupdate.Parameters.Add(paramtax);
			
			ordersdelete.Parameters.Add(paramid);
			
			ordersAdapter.InsertCommand = ordersinsert;
			ordersAdapter.UpdateCommand = ordersupdate;
			ordersAdapter.DeleteCommand = ordersdelete;
			
			
			// OrderDetails
			IDbCommand orderdetailscmd=connection.CreateCommand();
			orderdetailscmd.CommandText="SELECT Id, OrderId, ProductId, TaxId, Quantity, Price FROM OrderDetails";
			orderdetailsAdapter=ProviderFactory.CreateDataAdapter(orderdetailscmd);
			orderdetailsAdapter.TableMappings.Add("Table", "OrderDetails");
			// Meta
			IDbCommand metacmd=connection.CreateCommand();
			metacmd.CommandText="SELECT Id, Property, Value FROM Meta";
			metaAdapter=ProviderFactory.CreateDataAdapter(metacmd);
			metaAdapter.TableMappings.Add("Table", "Meta");
		}
		
		public void Fill(SuposDataSet ds)
		{
			CategoriesAdapter.Fill(ds);
			Util.SetAutoincrementSeed( ds.Categories, "Id");
			TaxesAdapter.Fill(ds);
			Util.SetAutoincrementSeed( ds.Taxes, "Id");
			ProductsAdapter.Fill(ds);
			Util.SetAutoincrementSeed( ds.Products, "Id");
			CustomersAdapter.Fill(ds);
			Util.SetAutoincrementSeed( ds.Customers, "Id");
			PaymentsAdapter.Fill(ds);
			Util.SetAutoincrementSeed( ds.Payments, "Id");
			OrdersAdapter.Fill(ds);
			OrderDetailsAdapter.Fill(ds);
			MetaAdapter.Fill(ds);
			Util.SetAutoincrementSeed( ds.Meta, "Id");
		}
			
	}
}
