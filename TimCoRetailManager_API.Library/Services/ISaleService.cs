﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_API.Library.Models;

namespace TimCoRetailManager_API.Library.Services
{
    public interface ISaleService
    {
        Task InsertOne(SaleDTO saleDto, string userId);
        Task<List<SaleUserViewModel>> FindAllSalesWithUsers();
    }

    public class SaleService : ISaleService
    {
        private readonly IConfiguration _config;

        public SaleService(IConfiguration config)
        {
            _config = config;
        }

        public async Task InsertOne(SaleDTO saleDto, string userId)
        {
            //IDb db = new Db();

            IProductService productService = new ProductService(_config);
            IConfigService configService = new ConfigService();
            var tax = configService.GetTax();

            // Create sale details
            var details = new List<SaleDetail>();
            foreach (var d in saleDto.SaleDetails)
            {
                var detail = new SaleDetail { ProductId = d.ProductId, Qty = d.Qty };

                var product = await productService.FindOne(d.ProductId);
                if (product == null)
                    throw new Exception("Product not found");

                detail.SellingPrice = product.RetailPrice * d.Qty;

                if (product.Taxable)
                    detail.Tax = detail.SellingPrice * tax / 100;

                details.Add(detail);
            }

            // Create sale
            var sale = new Sale { Subtotal = details.Sum(s => s.SellingPrice), Tax = details.Sum(s => s.Tax), UserId = userId };
            sale.Total = sale.Subtotal + sale.Tax;

            /*
            // Save sale to db
            await db.SaveAsync("dbo.sp_AddSale", sale, "TimCoRetailManager_DB");

            // Get back the sale id
            var saleId = (await db.LoadAsync<int, dynamic>("sp_GetSale", new { sale.UserId, sale.SaleDate }, "TimCoRetailManager_DB")).FirstOrDefault();

            // Add the sale id to each sale detail and save to db
            foreach (var d in details)
            {
                d.SaleId = saleId;
                await db.SaveAsync("dbo.sp_AddSaleDetail", d, "TimCoRetailManager_DB");
            }
            */

            // Using a transaction instead
            using (IDb db = new Db(_config))
            {
                try
                {
                    db.StartTransact("TimCoRetailManager_DB");

                    await db.SaveTransactAsync("dbo.sp_AddSale", sale);
                    var saleId = (await db.LoadTransactAsync<int, dynamic>("sp_GetSale", new { sale.UserId, sale.SaleDate })).FirstOrDefault();
                    foreach (var d in details)
                    {
                        d.SaleId = saleId;
                        await db.SaveTransactAsync("dbo.sp_AddSaleDetail", d);
                    }

                    db.Commit();
                }
                catch (Exception)
                {
                    db.Rollback();
                    throw;
                }
            }
        }

        public async Task<List<SaleUserViewModel>> FindAllSalesWithUsers()
        {
            IDb db = new Db(_config);

            return await db.LoadAsync<SaleUserViewModel, dynamic>("dbo.sp_GetSalesWithUsers", new { }, "TimCoRetailManager_DB");
        }
    }
}
