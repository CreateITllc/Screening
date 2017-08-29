using Screening.DataAcess;
using Screening.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Screening.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        GetData objGetData = new GetData();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Step1()
        {
            // objGetData.ExportData(39);
            //SignDoc objSignDoc = new SignDoc();
            //objSignDoc.test();
            try
            {
                Session["Step1Detail"] = null;
                Session["Step2Detail"] = null;
                Session["AgencyChkValue"] = null;
                ViewBag.Question = objGetData.GetAllQuestions();
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return View();
                //  throw;
            }
        }
        [HttpPost]
        public ActionResult Step1Post(Step1Model objStep1Model)
        {


            Session["Step1Detail"] = objStep1Model;
            return RedirectToAction("CompanyDetail");
            //if (Captch.ValidateCaptcha(Request["g-recaptcha-response"]))
            //{

            //}
            //  return RedirectToAction("CompanyDetail");
        }
        public ActionResult CompanyDetail()
        {
            //if (Session["Step1Detail"] == null)
            //{
            //    return RedirectToAction("Step1");
            //}
            Models.Step2Model objStep2Model = objGetData.GetAgency();
            return View(objStep2Model);
        }
        [HttpPost]
        public ActionResult CompanyDetail(Step2Model objStep2Model, FormCollection FRM)
        {
            GetData objGetData = new GetData();
            List<AgencyChkValue> lstAgencyChkValue = new List<AgencyChkValue>();
            var agencyChk = Request.Form.AllKeys.Where(c => c.StartsWith("Chk")).ToList();
            if (agencyChk.Count == 0)
            {
                ViewBag.Message = "Please select aleast one agency";
                Models.Step2Model objStep2Model2 = objGetData.GetAgency();
                return View(objStep2Model2);
            }
            //if (Session["Step1Detail"] == null)
            //{
            //    return RedirectToAction("Step1");
            //}
            foreach (var Chkbox in agencyChk)
            {
                //  objStep2Model.AddProperty(Chkbox.Remove(0,3), "True");
                lstAgencyChkValue.Add(new AgencyChkValue { AgencyID = Convert.ToInt32(Chkbox.Remove(0, 3)), Value = "True" });
            }
            TempData["AgencyChkValue"] = lstAgencyChkValue;
            //Step1Model objStep1Model = (Step1Model)Session["Step1Detail"];
            Session["Step2Detail"] = objStep2Model;
            Step2Model objStep2Model1 = (Step2Model)Session["Step2Detail"];
            Session["AgencyChkValue"] = lstAgencyChkValue;
            //  objGetData.SaveInformation(objStep1Model, objStep2Model, lstAgencyChkValue);
            return RedirectToAction("SignInDocs");
        }
        public ActionResult CompleteProcess()
        {
            Step2Model objStep2Model = (Step2Model)Session["Step2Detail"];
            List<AgencyChkValue> lstAgencyChkValue = (List<AgencyChkValue>)Session["AgencyChkValue"];
            objGetData.SaveInformation(objStep2Model, lstAgencyChkValue);
            Session["Step1Detail"] = null;
            Session["Step2Detail"] = null;
            Session["AgencyChkValue"] = null;
            return View();
        }

        public ActionResult Complete()
        {
            return RedirectToAction("Step1");
        }
        public JsonResult IsEmailExists(string email)
        {
            return Json(!objGetData.IsEmailExists(email), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsUserNameExists(string userName)
        {
            return Json(!objGetData.IsUsernameExists(userName), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SignInDocs()
        {
            //if (Session["Step1Detail"] == null)
            //{
            //    return RedirectToAction("Step1");
            //}
            if (Session["AgencyChkValue"] == null)
            {
                return RedirectToAction("CompanyDetail");
            }
            List<AgencyChkValue> lstAgencyChkValue = (List<AgencyChkValue>)Session["AgencyChkValue"];
            //Step1Model objStep1Model = (Step1Model)Session["Step1Detail"];
            Step2Model objStep2Model = (Step2Model)Session["Step2Detail"];
            SignDoc doc = new SignDoc();
            //string url = doc.CreateEnvelopFromTemplate();
            string url = doc.GeneratingDocuments(lstAgencyChkValue, objStep2Model.ContactFirstName, objStep2Model.ContactEmail);
            // string url=  doc.CreateEnvelop("fc6e1f7c-9632-40cd-89a2-c8f6026aa56d");
            //string url = SignDoc.test();
            ViewBag.Url = url;
            return View();
        }
    }

}