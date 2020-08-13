using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Contact_List_Demo.Models;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace Contact_List_Demo.Controllers
{
    public class ContactsController : Controller
    {
        private db_ContactListEntities db = new db_ContactListEntities();

        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                //===== UPLOADS FOLDER - create folder if doesn't exist
                string path = Server.MapPath("/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                //===== VALIDATE EXCEL & SET CONNECTION STRING
                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                string conString;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                    default:
                        ViewBag.InvalidImport = "IMPORT ONLY EXCEL FILES";
                        return View("Import");
                }
                
                //===== SAVE EXCEL to uploads folder, create connection string with filepath
                postedFile.SaveAs(filePath);
                conString = string.Format(conString, filePath); // connection string has a {0} placeholder for filePath

                //===== EXCEL READ - create new table object, get first sheet name, read data from first sheet, place in table object 
                DataTable dt = new DataTable();
                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //--- Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //--- Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT FirstName, LastName, Email, Phone From [" + sheetName + "] WHERE [FirstName] IS NOT NULL AND [LastName] IS NOT NULL";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                //===== FORMAT PHONE/EMAIL - remove all non-numeric phone number digits, remove leading 1, make no longer than 10 digits
                foreach (DataRow dr in dt.Rows)
                {
                    //----- Remove non-numeric digits
                    string drPhone = dr["Phone"].ToString();
                    string tempPhone;
                    if (!int.TryParse(drPhone, out _) || drPhone != "")
                    {
                        tempPhone = "";
                        char[] chars = drPhone.ToCharArray();
                        foreach (char c in chars)
                        {
                            if (char.IsDigit(c)) tempPhone += c.ToString();
                        }
                        drPhone = tempPhone;
                    }
                    //----- Remove leading "1"
                    if (drPhone.StartsWith("1")) drPhone = drPhone.Substring(1, drPhone.Length-1);
                    //----- Set max phone length = 10
                    if (drPhone.Length > 10) drPhone = drPhone.Substring(0, 10);
                    //----- SET phone
                    dr["Phone"] = drPhone;

                    //----- VALIDATE EMAIL
                    if (!dr["Email"].ToString().Contains(".") || !dr["Email"].ToString().Contains("@"))
                    {
                        dr["Email"] = null;
                    }
                }

                //===== SQL INSERT DATA
                conString = ConfigurationManager.ConnectionStrings["db_ContactListEntities_Manual"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //--- Set the database table name
                        sqlBulkCopy.DestinationTableName = "dbo.Contacts";

                        //--- [OPTIONAL]: Map the Excel columns with that of the database table (Excel, DB)
                        sqlBulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                        sqlBulkCopy.ColumnMappings.Add("LastName", "LastName");
                        sqlBulkCopy.ColumnMappings.Add("Phone", "Phone");
                        sqlBulkCopy.ColumnMappings.Add("Email", "Email");

                        //--- DB Write
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
