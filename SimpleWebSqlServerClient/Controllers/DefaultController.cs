using SimpleWebSqlServerClient.Data;
using SimpleWebSqlServerClient.Models;
using System;
using System.Configuration;
using System.Net;
using System.Web.Mvc;

namespace SimpleWebSqlServerClient.Controllers
{
    public class DefaultController : Controller
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: Default
        [Route("/")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/")]
        public ActionResult Index(QueryModel req)
        {
            var hostname = Dns.GetHostName();
            string ip = Request.UserHostAddress;
            _logger.Info($"hostname: [{hostname}], ip: [{ip}], query: [{req.Query}]");
            if (string.IsNullOrEmpty(req.Query))
            {
                _logger.Info($"Empty Query, hostname: [{hostname}], ip: [{ip}], query: [{req.Query}]");
                ViewBag.ErrMsg = "Empty Query";
                return View(); 
            }

            try
            {
                var prohibitedWordArr = ConfigurationManager.AppSettings["ProhibitedWords"].ToLower().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                foreach(var prohibitedWord in prohibitedWordArr)
                {
                    if (req.Query.ToLower().Contains(prohibitedWord))
                    {
                        _logger.Info($"user attempt to execute prohibited action: [{prohibitedWord}], hostname: [{hostname}], ip: [{ip}], query: [{req.Query}]");
                        ViewBag.ErrMsg = $"[{prohibitedWord.ToUpper()}] operation is not allowed";
                        return View();
                    }
                }

                var adoNetExec = new AdoNetExec();
                var ds = adoNetExec.GetDataSet(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString, req.Query);
                ViewBag.DataSet = ds;
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error($"ex: [{ex}], hostname: [{hostname}], ip: [{ip}], query: [{req.Query}]");
                ViewBag.ErrMsg = "Exception";
                return View();
            }
        }
    }

}