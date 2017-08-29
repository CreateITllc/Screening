using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;

namespace Screening.DataAcess
{
    public class GetData
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
            con = new SqlConnection(constr);
        }
        //To get list of question   
        public List<Models.QuestionVM> GetAllQuestions()
        {
            connection();
            List<Models.QuestionVM> QuestionList = new List<Models.QuestionVM>();
            SqlCommand cmd = new SqlCommand("select * from tblQuestions", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                QuestionList.Add(

                    new Models.QuestionVM
                    {
                        Question = Convert.ToString(dr["Question"]),
                        QuestionId = Convert.ToInt32(dr["QuestionId"])
                    }
                    );
            }
            return QuestionList;
        }
        public void SaveInformation( Models.Step2Model objStep2Model, List<Models.AgencyChkValue> lstAgencyChkValue)
        {
            connection();
            SqlTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                string mysql = "INSERT INTO TBLUSERS (USERNAME,IsActive,CreatorID,CreatedDate) VALUES ('" + objStep2Model.ContactFirstName + "',1,1,'" + DateTime.UtcNow + "'); SELECT CAST(scope_identity() AS int) ";
                SqlCommand cmdInsertUser = new SqlCommand(mysql, con, trans);
                int userID = (int)cmdInsertUser.ExecuteScalar();

                //mysql = "INSERT INTO TBLUSERQUESTIONANSWERS (USERID,QUESTIONID,ANSWER) VALUES ('" + userID + "','" + objSetp1Model.Question1 + "','" + objSetp1Model.Answer1 + "')  ";
                //SqlCommand cmdInsertQuestion1 = new SqlCommand(mysql, con, trans);
                //cmdInsertQuestion1.ExecuteNonQuery();

                //mysql = "INSERT INTO TBLUSERQUESTIONANSWERS (USERID,QUESTIONID,ANSWER) VALUES ('" + userID + "','" + objSetp1Model.Question1 + "','" + objSetp1Model.Answer1 + "') ";
                //SqlCommand cmdInsertQuestion2 = new SqlCommand(mysql, con, trans);
                //cmdInsertQuestion1.ExecuteNonQuery();

                //mysql = "INSERT INTO TBLUSERQUESTIONANSWERS (USERID,QUESTIONID,ANSWER) VALUES ('" + userID + "','" + objSetp1Model.Question1 + "','" + objSetp1Model.Answer1 + "')";
                //SqlCommand cmdInsertQuestion3 = new SqlCommand(mysql, con, trans);
                //cmdInsertQuestion1.ExecuteNonQuery();

                 mysql = "INSERT INTO [tblApplicants] ([USERID],[CompanyName],[BusinessStructure],[ContactFirstName],[ContactLastName],[ContactEmail],[ContactDirectPhone],[ContactCellPhone],[Contact2FirstName],[Contact2LastName],[Contact2Email],[Contact2DirectPhone],[Contact2CellPhone],[CompanyWebsiteAddress],[CorporateFULLAddress],[Emailcredentialsto],[Emailagreementto],[Estimatednumberofdrugscreenings],[EstimatedNumberofAnnualScreening])";
                mysql = mysql + " VALUES ('" + userID + "','" + objStep2Model.CompanyName + "','" + objStep2Model.BusinessStructure + "','" + objStep2Model.ContactFirstName + "','" + objStep2Model.ContactLastName + "','" + objStep2Model.ContactEmail + "','" + objStep2Model.Contactdirectphone + "','" + objStep2Model.ContactCellPhone + "','" + objStep2Model.Contact2FirstName + "','" + objStep2Model.Contact2LastName + "','" + objStep2Model.Contact2Email + "','" + objStep2Model.Contact2directphone + "','" + objStep2Model.Contact2CellPhone + "','" + objStep2Model.CompanyWebsiteAddress + "','" + objStep2Model.CorporateFullAddress + "','" + objStep2Model.Emailcredentialsto + "','" + objStep2Model.Emailagreementto + "','" + objStep2Model.Estimatednumberofdrugscreenings + "','" + objStep2Model.EstimatedNumberofBackgroundScreenings + "')";

                SqlCommand cmdApplicant = new SqlCommand(mysql, con, trans);
                cmdApplicant.ExecuteNonQuery();

                foreach (var agencyChecks in lstAgencyChkValue)
                {
                    mysql = "INSERT INTO tblAgencyUser (USERID,AgencyID,VALUE) VALUES ('" + userID + "','" + agencyChecks.AgencyID + "','" + agencyChecks.Value + "')  ";
                    SqlCommand cmdAgency = new SqlCommand(mysql, con, trans);
                    cmdAgency.ExecuteNonQuery();
                }
                trans.Commit();
                con.Close();
                ExportData(userID);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                //throw;
            }
        }
        private const string CsvContentType = "application/ms-excel";
        public void ExportData(int userID)
        {

            connection();
            List<Models.QuestionVM> QuestionList = new List<Models.QuestionVM>();
            SqlCommand cmd = new SqlCommand("select a.* from tblusers u,tblApplicants a where u.userID='" + userID + "'and u.UserId=a.UserID", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            DataTable dt2 = new DataTable();
            SqlCommand cmdAgencyDetail = new SqlCommand("SELECT T.AgencyName,A.Value FROM tblAgency T,tblAgencyUser A WHERE T.AgencyId=A.AgencyID AND A.UserID='" + userID + "'", con);
            SqlDataAdapter daAgency = new SqlDataAdapter(cmdAgencyDetail);
            daAgency.Fill(dt2);
            DataTable dtTranspose = new DataTable();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt.Columns.Add(dt2.Rows[i][0].ToString());
                dt.Rows[0][dt2.Rows[i][0].ToString()] = dt2.Rows[i][1].ToString();
            }
            con.Close();
            Stream dataStream = ConvertToCsvString(dt);
            sendEMail(ConfigurationSettings.AppSettings["MailUserName"], ConfigurationSettings.AppSettings["ReceipentMailID"], "New Applicant Registration", "New Applicant has been registered through 2020 Screeing " + Environment.NewLine + " Please Verify the attached csv file.", ConfigurationSettings.AppSettings["SmtpServer"], Convert.ToInt32(ConfigurationSettings.AppSettings["Port"]), ConfigurationSettings.AppSettings["MailPassword"], dataStream, "text/csv", "NewRegisteredData.csv");
        }
        public bool sendEMail(string SenderAdd, string RecieveAdd, string subject, string Body, string Smtp, int portno, string Password, Stream tableStram, string CsvContentType, string attname)
        {
            try
            {
                tableStram.Position = 0;
                MailMessage mM = new MailMessage();
                mM.From = new MailAddress(SenderAdd);
                mM.To.Add(RecieveAdd);
                mM.Subject = subject;
                mM.Attachments.Add(new Attachment(tableStram, attname, "text/csv"));
                //add the body of the email
                mM.Body = Body;
                mM.IsBodyHtml = true;
                //SMTP client
                SmtpClient sC = new SmtpClient(Smtp);
                //port number for Gmail mail
                sC.Port = portno;
                //credentials to login in to Gmail account
                sC.Credentials = new NetworkCredential(SenderAdd, Password);
                //enabled SSL
                sC.EnableSsl = true;
                //Send an email
                sC.Send(mM);
                mM.Dispose();
                //MessageBox.Show("EMAIL SENT . ", "Broker");
                return true;

            }//end of try block
            catch (Exception ex)
            {

                return false; ;

            }//end of catch
        }//end of Email Method
        public bool IsEmailExists(string email)
        {
            connection();
            SqlCommand cmd = new SqlCommand("select email from tblUsers where email='" + email + "'", con);
            con.Open();
            var a = cmd.ExecuteScalar();
            if (cmd.ExecuteScalar() == null) { return false; }
            else { return true; }
        }
        public bool IsUsernameExists(string userName)
        {
            connection();
            SqlCommand cmd = new SqlCommand("select username from tblUsers where username='" + userName + "'", con);
            con.Open();
            var a = cmd.ExecuteScalar();
            if (cmd.ExecuteScalar() == null) { return false; }
            else { return true; }
        }
        public Stream ConvertToCsvString(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                //add separator
                sb.Append(dt.Columns[k].ColumnName + ',');
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
                }
                //append new line
                sb.Append("\r\n");
            }
            Byte[] returnArray = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            string returnstring = System.Convert.ToBase64String(returnArray);
            Stream stream = new MemoryStream(returnArray);
            return stream;

            // return sb.ToString() ;
        }
        public Models.Step2Model GetAgency()
        {
            connection();
            List<Models.Agency> agencyLst = new List<Models.Agency>();
            SqlCommand cmd = new SqlCommand("select * from tblAgency", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                agencyLst.Add(
                     new Models.Agency
                     {
                         AgencyId = Convert.ToInt32(dr["AgencyId"]),
                         AgencyName = Convert.ToString(dr["AgencyName"]),
                         IsVisible = (bool)dr["IsVisible"]
                     }
                     );
            }
            Models.Step2Model objStep2Model = new Models.Step2Model();
            objStep2Model.Agency = agencyLst;
            return objStep2Model;
        }

        public  string GetTemplate(int ID)
        {
            connection();
            List<Models.QuestionVM> QuestionList = new List<Models.QuestionVM>();
            SqlCommand cmd = new SqlCommand("select template from tblagency where AgencyID='" + ID + "'  ", con);
            con.Open();
            return Convert.ToString(cmd.ExecuteScalar());

        }
    }

}