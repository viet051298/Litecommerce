using LiteCommerce.BussinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> ListOfCountry(bool allSelect = true) {
            List<SelectListItem> listCountries = new List<SelectListItem>();
            if (allSelect)
            {
                listCountries.Add(new SelectListItem() { Value = "", Text = "--All Country--" });
            }

            List<Countries> listCountry = CatalogBLL.Country_List();
            foreach(var item in listCountry)
            {
                listCountries.Add(new SelectListItem() { Value = item.Country, Text = item.Country });
            }
            return listCountries;
        }

        public static List<SelectListItem> ListOfCategory(bool allSelect = true)
        {
            List<SelectListItem> listCategory = new List<SelectListItem>();
            if (allSelect)
            {
                listCategory.Add(new SelectListItem() { Value = "", Text = "--All Category--" });
            }

            List<Category> categories = CatalogBLL.Category_List(1, 100, "");
            foreach (var item in categories)
            {
                listCategory.Add(new SelectListItem() { Value = item.CategoryID.ToString(), Text = item.CategoryName });
            }
            return listCategory;
        }

        public static List<SelectListItem> ListOfSupplier(bool allSelect = true)
        {
            List<SelectListItem> listSupplier = new List<SelectListItem>();
            if (allSelect)
            {
                listSupplier.Add(new SelectListItem() { Value = "", Text = "--All Suppliers--" });
            }

            List<Supplier> suppliers = CatalogBLL.Supplier_List(1, 100, "");
            foreach (var item in suppliers)
            {
                listSupplier.Add(new SelectListItem() { Value = item.SupplierID.ToString(),Text = item.CompanyName });
            }
            return listSupplier;
        }

        public static List<SelectListItem> ListOfTitle()
        {
            List<SelectListItem> listTitle = new List<SelectListItem>();
            listTitle.Add(new SelectListItem() { Value = "Sales Representative", Text = "Sales Representative" });
            listTitle.Add(new SelectListItem() { Value = "Vice President, Sales", Text = "Vice President, Sales" });
            listTitle.Add(new SelectListItem() { Value = "Sales Manager", Text = "Sales Manager" });
            listTitle.Add(new SelectListItem() { Value = "Inside Sales Coordinator", Text = "Inside Sales Coordinator" });
            return listTitle;
        }

        public static List<SelectListItem> ListOfSupplierID(bool allSelect = true)
        {
            List<SelectListItem> listSupplierID = new List<SelectListItem>();
            if (allSelect)
            {
                listSupplierID.Add(new SelectListItem() { Value = "", Text = "--All Suppliers--" });
            }

            List<Supplier> supplierID = CatalogBLL.Supplier_List(1, 100, "");
            foreach (var item in supplierID)
            {
                listSupplierID.Add(new SelectListItem() { Value = item.SupplierID.ToString(), Text = item.SupplierID.ToString() });
            }
            return listSupplierID;
        }
        public static List<SelectListItem> ListOfCategoryID(bool allSelect = true)
        {
            List<SelectListItem> listCategoryID = new List<SelectListItem>();
            if (allSelect)
            {
                listCategoryID.Add(new SelectListItem() { Value = "", Text = "--All CategoryID--" });
            }

            List<Category> categoryID = CatalogBLL.Category_List(1, 100, "");
            foreach (var item in categoryID)
            {
                listCategoryID.Add(new SelectListItem() { Value = item.CategoryID.ToString(), Text = item.CategoryID.ToString() });
            }
            return listCategoryID;
        }
    }
}