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

		private void CreateCategoriesAdapter()
		{
			IDbCommand catcmd=connection.CreateCommand();
			catcmd.CommandText="SELECT Id, Name, Icon FROM Categories";
			categoriesAdapter=ProviderFactory.CreateDataAdapter(catcmd);
			
			IDbCommand categoriesinsert= connection.CreateCommand();
			categoriesinsert.CommandText= "INSERT INTO Categories(Id, Name, Icon) VALUES(:ID, :NAME, :ICON)";

			IDbCommand categoriesupdate= connection.CreateCommand();
			categoriesupdate.CommandText= "UPDATE Categories SET Id=:ID, Name=:NAME, Icon=:ICON WHERE Id=:ID";
			
			IDbCommand categoriesdelete= connection.CreateCommand();
			categoriesdelete.CommandText= "DELETE FROM Categories WHERE Id=:ID";
			
			categoriesAdapter.TableMappings.Add("Table", "Categories");

			IDbDataParameter paramid = categoriesinsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.Int64;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramname = categoriesinsert.CreateParameter();
			paramname.ParameterName= ":NAME";
			paramname.DbType= DbType.String;
			paramname.SourceColumn= "Name";
			paramname.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramicon = categoriesinsert.CreateParameter();
			paramicon.ParameterName= ":ICON";
			paramicon.DbType= DbType.String;
			paramicon.SourceColumn= "Icon";
			paramicon.SourceVersion=DataRowVersion.Current;
			
			categoriesinsert.Parameters.Add(paramid);
			categoriesinsert.Parameters.Add(paramname);
			categoriesinsert.Parameters.Add(paramicon);
			
			categoriesupdate.Parameters.Add(paramid);
			categoriesupdate.Parameters.Add(paramname);
			categoriesupdate.Parameters.Add(paramicon);
			
			categoriesdelete.Parameters.Add(paramid);
			
			categoriesAdapter.InsertCommand = categoriesinsert;
			categoriesAdapter.UpdateCommand = categoriesupdate;
			categoriesAdapter.DeleteCommand = categoriesdelete;
		}

		private void CreateTaxesAdapter()
		{			
			IDbCommand taxcmd=connection.CreateCommand();
			taxcmd.CommandText="SELECT Id, Name, Rate, Overrideable FROM Taxes";
			taxesAdapter=ProviderFactory.CreateDataAdapter(taxcmd);
						
			IDbCommand taxesinsert= connection.CreateCommand();
			taxesinsert.CommandText= "INSERT INTO Taxes(Id, Name, Rate, Overrideable) VALUES(:ID, :NAME, :RATE, :OVERRIDEABLE)";

			IDbCommand taxesupdate= connection.CreateCommand();
			taxesupdate.CommandText= "UPDATE Taxes SET Id=:ID, Name=:NAME, Rate=:RATE WHERE Id=:ID";
			
			IDbCommand taxesdelete= connection.CreateCommand();
			taxesdelete.CommandText= "DELETE FROM Taxes WHERE Id=:ID";
			
			taxesAdapter.TableMappings.Add("Table", "Taxes");
			
			IDbDataParameter paramid = taxesinsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.Int64;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramname = taxesinsert.CreateParameter();
			paramname.ParameterName= ":NAME";
			paramname.DbType= DbType.String;
			paramname.SourceColumn= "Name";
			paramname.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramrate = taxesinsert.CreateParameter();
			paramrate.ParameterName= ":RATE";
			paramrate.DbType= DbType.Decimal;
			paramrate.SourceColumn= "Rate";
			paramrate.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramover = taxesinsert.CreateParameter();
			paramover.ParameterName= ":OVERRIDEABLE";
			paramover.DbType= DbType.Boolean;
			paramover.SourceColumn= "Overrideable";
			paramover.SourceVersion=DataRowVersion.Current;
			
			taxesinsert.Parameters.Add(paramid);
			taxesinsert.Parameters.Add(paramname);
			taxesinsert.Parameters.Add(paramrate);			
			taxesinsert.Parameters.Add(paramover);
			
			taxesupdate.Parameters.Add(paramid);
			taxesupdate.Parameters.Add(paramname);
			taxesupdate.Parameters.Add(paramrate);
			taxesupdate.Parameters.Add(paramover);
			
			taxesdelete.Parameters.Add(paramid);
			
			taxesAdapter.InsertCommand = taxesinsert;
			taxesAdapter.UpdateCommand = taxesupdate;
			taxesAdapter.DeleteCommand = taxesdelete;
		}
		
		private void CreateProductsAdapter()
		{
			IDbCommand prodcmd=connection.CreateCommand();
			prodcmd.CommandText="SELECT Id, Name, Icon, Price, CategoryId, DefaultTaxId FROM Products";
			productsAdapter=ProviderFactory.CreateDataAdapter(prodcmd);
									
			IDbCommand prodinsert= connection.CreateCommand();
			prodinsert.CommandText= "INSERT INTO Products(Id, Name, Icon, Price, CategoryID, DefaultTaxID) VALUES(:ID, :NAME, :ICON, :PRICE, :CATEGORYID, :DEFAULTTAXID)";

			IDbCommand produpdate= connection.CreateCommand();
			produpdate.CommandText= "UPDATE Products SET Id=:ID, Name=:NAME, Icon=:ICON, Price=:PRICE, CategoryId=:CATEGORYID WHERE Id=:ID";
			
			IDbCommand proddelete= connection.CreateCommand();
			proddelete.CommandText= "DELETE FROM Products WHERE Id=:ID";
			
			productsAdapter.TableMappings.Add("Table", "Products");
			
			IDbDataParameter paramid = prodinsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.Int64;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramname = prodinsert.CreateParameter();
			paramname.ParameterName= ":NAME";
			paramname.DbType= DbType.String;
			paramname.SourceColumn= "Name";
			paramname.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramicon = prodinsert.CreateParameter();
			paramicon.ParameterName= ":ICON";
			paramicon.DbType= DbType.String;
			paramicon.SourceColumn= "Icon";
			paramicon.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramprice = prodinsert.CreateParameter();
			paramprice.ParameterName= ":PRICE";
			paramprice.DbType= DbType.Decimal;
			paramprice.SourceColumn= "Price";
			paramprice.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramcat = prodinsert.CreateParameter();
			paramcat.ParameterName= ":CATEGORYID";
			paramcat.DbType= DbType.Boolean;
			paramcat.SourceColumn= "CategoryId";
			paramcat.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramtax = prodinsert.CreateParameter();
			paramtax.ParameterName= ":DEFAULTTAXID";
			paramtax.DbType= DbType.Boolean;
			paramtax.SourceColumn= "DefaultTaxId";
			paramtax.SourceVersion=DataRowVersion.Current;
			
			prodinsert.Parameters.Add(paramid);
			prodinsert.Parameters.Add(paramname);
			prodinsert.Parameters.Add(paramicon);			
			prodinsert.Parameters.Add(paramprice);
			prodinsert.Parameters.Add(paramcat);			
			prodinsert.Parameters.Add(paramtax);
			
			produpdate.Parameters.Add(paramid);
			produpdate.Parameters.Add(paramname);
			produpdate.Parameters.Add(paramicon);
			produpdate.Parameters.Add(paramprice);
			prodinsert.Parameters.Add(paramcat);			
			prodinsert.Parameters.Add(paramtax);
			
			proddelete.Parameters.Add(paramid);
			
			productsAdapter.InsertCommand = prodinsert;
			productsAdapter.UpdateCommand = produpdate;
			productsAdapter.DeleteCommand = proddelete;
		}
		
		private void CreateCustomersAdapter()
		{
			IDbCommand custselect=connection.CreateCommand();
			custselect.CommandText="SELECT Id, Name FROM Customers";
			customersAdapter=ProviderFactory.CreateDataAdapter(custselect);

			IDbCommand custinsert= connection.CreateCommand();
			custinsert.CommandText= "INSERT INTO Customers(Id, Name) VALUES(:ID, :NAME)";

			IDbCommand custupdate= connection.CreateCommand();
			custupdate.CommandText= "UPDATE Customers SET Id=:ID, Name=:NAME WHERE Id=:ID";
			
			IDbCommand custdelete= connection.CreateCommand();
			custdelete.CommandText= "DELETE FROM Customers WHERE Id=:ID";
			
			customersAdapter.TableMappings.Add("Table", "Customers");
			
			IDbDataParameter paramid = custinsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.Int64;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramname = custinsert.CreateParameter();
			paramname.ParameterName= ":NAME";
			paramname.DbType= DbType.String;
			paramname.SourceColumn= "Name";
			paramname.SourceVersion=DataRowVersion.Current;
			
			custinsert.Parameters.Add(paramid);
			custinsert.Parameters.Add(paramname);
			
			custupdate.Parameters.Add(paramid);
			custupdate.Parameters.Add(paramname);
			
			custdelete.Parameters.Add(paramid);
			
			customersAdapter.InsertCommand = custinsert;
			customersAdapter.UpdateCommand = custupdate;
			customersAdapter.DeleteCommand = custdelete;
		}
		
		private void CreatePaymentsAdapter()
		{
			IDbCommand paycmd=connection.CreateCommand();
			paycmd.CommandText="SELECT Id, Name, Allowed FROM Payments";
			paymentsAdapter=ProviderFactory.CreateDataAdapter(paycmd);
			
			IDbCommand payinsert= connection.CreateCommand();
			payinsert.CommandText= "INSERT INTO Payments(Id, Name, Allowed) VALUES(:ID, :NAME, :ALLOWED)";

			IDbCommand payupdate= connection.CreateCommand();
			payupdate.CommandText= "UPDATE Payments SET Id=:ID, Name=:NAME, Allowed=:ALLOWED WHERE Id=:ID";
			
			IDbCommand paydelete= connection.CreateCommand();
			paydelete.CommandText= "DELETE FROM Payments WHERE Id=:ID";
			
			paymentsAdapter.TableMappings.Add("Table", "Payments");
			
			IDbDataParameter paramid = payinsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.Int64;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramname = payinsert.CreateParameter();
			paramname.ParameterName= ":NAME";
			paramname.DbType= DbType.String;
			paramname.SourceColumn= "Name";
			paramname.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramallowed = payinsert.CreateParameter();
			paramallowed.ParameterName= ":ALLOWED";
			paramallowed.DbType= DbType.Boolean;
			paramallowed.SourceColumn= "Allowed";
			paramallowed.SourceVersion=DataRowVersion.Current;
			
			payinsert.Parameters.Add(paramid);
			payinsert.Parameters.Add(paramname);
			payinsert.Parameters.Add(paramallowed);
			
			payupdate.Parameters.Add(paramid);
			payupdate.Parameters.Add(paramname);
			payupdate.Parameters.Add(paramallowed);
			
			paydelete.Parameters.Add(paramid);
			
			paymentsAdapter.InsertCommand = payinsert;
			paymentsAdapter.UpdateCommand = payupdate;
			paymentsAdapter.DeleteCommand = paydelete;
		}
		
		private void CreateOrdersAdapter()
		{
			IDbCommand orderscmd= connection.CreateCommand();
			orderscmd.CommandText="SELECT Id, CustomerId, PaymentId, TaxId FROM Orders";
			ordersAdapter= ProviderFactory.CreateDataAdapter(orderscmd);
			
			IDbCommand ordersinsert= connection.CreateCommand();
			ordersinsert.CommandText= "INSERT INTO Orders(Id, CustomerId, PaymentId, TaxId) VALUES(:ID, :CUSTOMERID, :PAYMENTID, :TAXID)";
			
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
		}
		
		private void CreateOrderDetailsAdapter()
		{
			IDbCommand orderdetailscmd=connection.CreateCommand();
			orderdetailscmd.CommandText="SELECT Id, OrderId, ProductId, TaxId, Quantity, Price FROM OrderDetails";
			orderdetailsAdapter=ProviderFactory.CreateDataAdapter(orderdetailscmd);
			
			IDbCommand orderdetailsinsert= connection.CreateCommand();
			orderdetailsinsert.CommandText= "INSERT INTO OrderDetails(Id, OrderId, ProductId, TaxId, Quantity, Price) VALUES(:ID, :ORDERID, :PRODUCTID, :TAXID, :QUANTITY, :PRICE)";
			
			IDbCommand orderdetailsupdate= connection.CreateCommand();
			orderdetailsupdate.CommandText= "UPDATE OrderDetails SET Id=:ID, OrderId=:ORDERID, ProductId=:PRODUCTID, TaxId=:TAXID, Quantity=:QUANTITY, Price=:PRICE WHERE Id=:ID";

			IDbCommand orderdetailsdelete= connection.CreateCommand();
			orderdetailsdelete.CommandText= "DELETE FROM OrderDetails WHERE Id=:ID";
			
			orderdetailsAdapter.TableMappings.Add("Table", "OrderDetails");
			
			IDbDataParameter paramid = orderdetailsinsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.Int64;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramorder = orderdetailsinsert.CreateParameter();
			paramorder.ParameterName= ":ORDERRID";
			paramorder.DbType= DbType.String;
			paramorder.SourceColumn= "OrderId";
			paramorder.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramprod = orderdetailsinsert.CreateParameter();
			paramprod.ParameterName= ":PRODUCTID";
			paramprod.DbType= DbType.Int64;
			paramprod.SourceColumn= "ProductId";
			paramprod.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramtax = orderdetailsinsert.CreateParameter();
			paramtax.ParameterName= ":TAXID";
			paramtax.DbType= DbType.Int64;
			paramtax.SourceColumn= "TaxId";
			paramtax.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramquant = orderdetailsinsert.CreateParameter();
			paramquant.ParameterName= ":QUANTITY";
			paramquant.DbType= DbType.Int64;
			paramquant.SourceColumn= "Quantity";
			paramquant.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramprice = orderdetailsinsert.CreateParameter();
			paramprice.ParameterName= ":PRICE";
			paramprice.DbType= DbType.Decimal;
			paramprice.SourceColumn= "Price";
			paramprice.SourceVersion=DataRowVersion.Current;
			
			orderdetailsinsert.Parameters.Add(paramid);
			orderdetailsinsert.Parameters.Add(paramorder);
			orderdetailsinsert.Parameters.Add(paramprod);
			orderdetailsinsert.Parameters.Add(paramtax);
			orderdetailsinsert.Parameters.Add(paramquant);
			orderdetailsinsert.Parameters.Add(paramprice);
			
			orderdetailsupdate.Parameters.Add(paramid);
			orderdetailsupdate.Parameters.Add(paramorder);
			orderdetailsupdate.Parameters.Add(paramprod);
			orderdetailsupdate.Parameters.Add(paramtax);
			orderdetailsupdate.Parameters.Add(paramquant);
			orderdetailsupdate.Parameters.Add(paramprice);
			
			orderdetailsdelete.Parameters.Add(paramid);
			
			orderdetailsAdapter.InsertCommand = orderdetailsinsert;
			orderdetailsAdapter.UpdateCommand = orderdetailsupdate;
			orderdetailsAdapter.DeleteCommand = orderdetailsdelete;
		}
		
		private void CreateMetaAdapter()
		{
			IDbCommand metacmd=connection.CreateCommand();
			metacmd.CommandText="SELECT Id, Property, Value FROM Meta";
			metaAdapter=ProviderFactory.CreateDataAdapter(metacmd);
			
			IDbCommand metainsert= connection.CreateCommand();
			metainsert.CommandText= "INSERT INTO Meta(Id, Property, Value) VALUES(:ID, :PROPERY, :VALUE)";
			
			IDbCommand metaupdate= connection.CreateCommand();
			metaupdate.CommandText= "UPDATE Meta SET Id=:ID, Property=:PROPERTY, Value=:VALUE WHERE Id=:ID";

			IDbCommand metadelete= connection.CreateCommand();
			metadelete.CommandText= "DELETE FROM Meta WHERE Id=:ID";
			
			metaAdapter.TableMappings.Add("Table", "Meta");
			
			IDbDataParameter paramid = metainsert.CreateParameter();
			paramid.ParameterName= ":ID";
			paramid.DbType= DbType.Int64;
			paramid.SourceColumn= "Id";
			paramid.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramproperty = metainsert.CreateParameter();
			paramproperty.ParameterName= ":PROPERTY";
			paramproperty.DbType= DbType.String;
			paramproperty.SourceColumn= "Property";
			paramproperty.SourceVersion=DataRowVersion.Current;
			
			IDbDataParameter paramvalue = metainsert.CreateParameter();
			paramvalue.ParameterName= ":VALUE";
			paramvalue.DbType= DbType.String;
			paramvalue.SourceColumn= "Value";
			paramvalue.SourceVersion=DataRowVersion.Current;
			
			metainsert.Parameters.Add(paramid);
			metainsert.Parameters.Add(paramproperty);
			metainsert.Parameters.Add(paramvalue);
			
			metainsert.Parameters.Add(paramid);
			metainsert.Parameters.Add(paramproperty);
			metainsert.Parameters.Add(paramvalue);
			
			metainsert.Parameters.Add(paramid);
			
			metaAdapter.InsertCommand = metainsert;
			metaAdapter.UpdateCommand = metaupdate;
			metaAdapter.DeleteCommand = metadelete;
			
		}
		
		private void CreateAdapters()
		{
			this.CreateCategoriesAdapter();
			this.CreateTaxesAdapter();
			this.CreateProductsAdapter();
			this.CreateCustomersAdapter();
			this.CreatePaymentsAdapter();
			this.CreateOrdersAdapter();
			this.CreateOrderDetailsAdapter();
			this.CreateMetaAdapter();
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
