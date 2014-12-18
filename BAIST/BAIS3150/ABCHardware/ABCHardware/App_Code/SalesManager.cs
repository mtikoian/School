﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	public class SalesManager
	{
		public int AddSale(SalesReceipt receipt)
		{
			int saleNumber = 0;
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("AddSale", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;

					var dateParam = new SqlParameter("@SaleDate", receipt.Date);
					dateParam.SqlDbType = System.Data.SqlDbType.Date;
					dateParam.Direction = System.Data.ParameterDirection.Input;
					command.Parameters.Add(dateParam);

					var salesPersonParam = new SqlParameter("@SalesPerson", receipt.SalesPerson);
					salesPersonParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					salesPersonParam.Direction = System.Data.ParameterDirection.Input;
					command.Parameters.Add(salesPersonParam);

					var customerParam = new SqlParameter("@CustomerId", receipt.Customer.Id);
					customerParam.SqlDbType = System.Data.SqlDbType.Int;
					customerParam.Direction = System.Data.ParameterDirection.Input;
					command.Parameters.Add(customerParam);

					var subtotalParam = new SqlParameter("@Subtotal", receipt.Subtotal);
					subtotalParam.SqlDbType = System.Data.SqlDbType.Money;
					subtotalParam.Direction = System.Data.ParameterDirection.Input;
					command.Parameters.Add(subtotalParam);

					var gstParam = new SqlParameter("@GST", receipt.GST);
					gstParam.SqlDbType = System.Data.SqlDbType.Money;
					gstParam.Direction = System.Data.ParameterDirection.Input;
					command.Parameters.Add(gstParam);

					var totalParam = new SqlParameter("@Total", receipt.Total);
					totalParam.SqlDbType = System.Data.SqlDbType.Money;
					totalParam.Direction = System.Data.ParameterDirection.Input;
					command.Parameters.Add(totalParam);

					connection.Open();
					var transaction = connection.BeginTransaction();

					saleNumber = int.Parse(command.ExecuteScalar().ToString());

					foreach (var salesItem in receipt.Items)
					{
						AddSaleItem(connection, salesItem, saleNumber);
					}
				
					transaction.Commit();
					connection.Close();
				}
			}

			return saleNumber;
		}

		public void AddSaleItem(SqlConnection connection, SalesItem salesItem, int saleNumber)
		{
			using (var command = new SqlCommand("AddSalesItem", connection))
			{
				command.CommandType = System.Data.CommandType.StoredProcedure;

				var saleNumberParam = new SqlParameter("@SalesNumber", saleNumber);
				saleNumberParam.SqlDbType = System.Data.SqlDbType.Int;
				saleNumberParam.Direction = System.Data.ParameterDirection.Input;
				command.Parameters.Add(saleNumberParam);

				var itemCodeParam = new SqlParameter("@ItemCode", salesItem.Item.Code);
				itemCodeParam.SqlDbType = System.Data.SqlDbType.NVarChar;
				itemCodeParam.Direction = System.Data.ParameterDirection.Input;
				command.Parameters.Add(itemCodeParam);

				var quantityParam = new SqlParameter("@Quantity", salesItem.Quantity);
				quantityParam.SqlDbType = System.Data.SqlDbType.Int;
				quantityParam.Direction = System.Data.ParameterDirection.Input;
				command.Parameters.Add(quantityParam);

				var totalParam = new SqlParameter("@ItemTotal", salesItem.ItemTotal);
				totalParam.SqlDbType = System.Data.SqlDbType.Money;
				totalParam.Direction = System.Data.ParameterDirection.Input;
				command.Parameters.Add(totalParam);

				command.ExecuteNonQuery();
			}
		}
	}
}