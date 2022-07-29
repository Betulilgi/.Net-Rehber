using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration; //web.config'den bağlantı dizesini çağırmak için bir ad alanı ekledik.
using System.Data.SqlClient;
using System.Data;

namespace Director.Controllers
{
    public class HomeController : Controller
    {
        //database bağlantısı
        String ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //EKLEME FONKSİYONU
        public JsonResult InsertFields(string name, string lName, string pNumber, string tc)
        {
            //Hata varsa yakalamak için try-catch kullanılır.
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open(); //veritabanı bağlantısını açma
                SqlCommand cmd = new SqlCommand("insert into person(name,lName, pNumber, tc) values ('" + name + "', '" + lName + "', '" + pNumber + "', '"+ tc + "' )", con);
                cmd.ExecuteNonQuery();  //veritabanındaki verileri kullanmadan DataSet değiştirmek için.
                con.Close();

                return Json("success", JsonRequestBehavior.AllowGet);   //Json işlevi olduğundan dönüş tipi de json olmalıdır.
            }
            catch (Exception ex)
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        //VERİLERİ EKRANDA GÖSTER
        public JsonResult ViewData()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM person", con);
                DataTable dt = new DataTable();
                SqlDataAdapter cmd1 = new SqlDataAdapter(cmd);
                cmd1.Fill(dt);
                string html = "";
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        html += "['" + dt.Rows[i]["id"] + "','" + dt.Rows[i]["name"] + "', '" + dt.Rows[i]["lName"] + "', '" + dt.Rows[i]["pNumber"] + "' , '" + dt.Rows[i]["tc"] + "', " +
                            "'<a onclick=fnEdit(" + dt.Rows[i]["id"] + ")> <b> Düzenle </b></a>  /  <a onclick= fnDelete(" + dt.Rows[i]["id"] + ") <b> Sil </b></a>' ],";
                    }
                }
                con.Close();
                html = html.Substring(0, Math.Max(0, html.Length - 1));

                return Json(new { html }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        //SİLME FONKSİYONU
        public JsonResult DeleteData(string id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from person where id = '" + id + "' ", con);
                cmd.ExecuteNonQuery();
                con.Close();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        //DÜZENLEME FONKSİYONU
        public JsonResult EditData(string id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT name,lName,pNumber,tc,id FROM person WHERE id = '" + id + "' ", con);
                DataTable dt = new DataTable();
                SqlDataAdapter cmd1 = new SqlDataAdapter(cmd);
                cmd1.Fill(dt);

                string[] htmlValues = new string[dt.Columns.Count];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        htmlValues[i] = dt.Rows[0].ItemArray[i].ToString();
                    }
                }
                con.Close();

                return Json(new { htmlValues }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        //GÜNCELLE FONKSİYONU
        public JsonResult UpdateFields(string name, string lName, string pNumber, string tc, int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Update person set name = '" + name + "', lName = '" + lName + "', pNumber = '" + pNumber + "',tc = '" + tc + "' Where id = '" + id+"'", con);
                cmd.ExecuteNonQuery();
                con.Close();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }
    }
}