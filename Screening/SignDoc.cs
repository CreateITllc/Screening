using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Screening.DataAcess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Screening
{
    public class SignDoc
    {
        GetData objGetData = new GetData();

        public string test()
        {

            // Enter your DocuSign credentials
            string Username = "rohitj2@chetu.com";
            string Password = "Chetu@123";
            string IntegratorKey = "75f3fa5e-1cf2-4910-b3fa-62ec16228956";

            // specify the document we want signed
            string SignTest1File = @"D:\TrueSolutions-HM\ProjectDocuments/vizient_background_screen_services_agreement_copy_2.pdf";

            // Enter recipient (signer) name and email address
            string recipientName = "Umashankar";
            string recipientEmail = "umashankars@chetu.com";

            // instantiate api client with appropriate environment (for production change to www.docusign.net/restapi)
            string basePath = "https://demo.docusign.net/restapi";

            // instantiate a new api client
            ApiClient apiClient = new ApiClient(basePath);

            // set client in global config so we don't need to pass it to each API object
            Configuration.Default.ApiClient = apiClient;

            string authHeader = "{\"Username\":\"" + Username + "\", \"Password\":\"" + Password + "\", \"IntegratorKey\":\"" + IntegratorKey + "\"}";

            Configuration.Default.AddDefaultHeader("X-DocuSign-Authentication", authHeader);

            // we will retrieve this from the login() results
            string accountId = null;

            // the authentication api uses the apiClient (and X-DocuSign-Authentication header) that are set in Configuration object
            AuthenticationApi authApi = new AuthenticationApi();
            // LoginInformation loginInfo = authApi.Login();
            // user might be a member of multiple accounts
            // accountId = loginInfo.LoginAccounts[0].AccountId;

            accountId = DocusignLogin();
            // Read a file from disk to use as a document
            byte[] fileBytes = File.ReadAllBytes(SignTest1File);

            EnvelopeDefinition envDef = new EnvelopeDefinition();
            envDef.EmailSubject = "[DocuSign C# SDK] - Please sign this doc";


            // Add a document to the envelope
            Document doc = new Document();
            doc.DocumentBase64 = System.Convert.ToBase64String(fileBytes);
            doc.Name = "TestFile.pdf";
            doc.DocumentId = "1";

            envDef.Documents = new List<Document>();
            envDef.Documents.Add(doc);


            // Add a recipient to sign the documeent
            Signer signer = new Signer();
            signer.Name = recipientName;
            signer.Email = recipientEmail;
            signer.RecipientId = "1";


            // must set |clientUserId| to embed the recipient
            signer.ClientUserId = "1234";
            // Create a |SignHere| tab somewhere on the document for the recipient to sign
            signer.Tabs = new Tabs();
            signer.Tabs.SignHereTabs = new List<SignHere>();
            SignHere signHere = new SignHere();
            signHere.DocumentId = "1";
            signHere.PageNumber = "23";
            signHere.RecipientId = "1";
            signHere.XPosition = "50";
            signHere.YPosition = "700";

            signer.Tabs.SignHereTabs.Add(signHere);

            envDef.Recipients = new Recipients();
            envDef.Recipients.Signers = new List<Signer>();
            envDef.Recipients.Signers.Add(signer);


            // set envelope status to "sent" to immediately send the signature request
            envDef.Status = "sent";
            // Use the EnvelopesApi to create and send the signature request
            EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope(accountId, envDef);
            RecipientViewRequest viewOptions = new RecipientViewRequest()
            {
                //ReturnUrl = "https://www.docusign.com/devcenter",
                ReturnUrl = "http://localhost:56086/Registration/CompleteProcess",
                ClientUserId = "1234",  // must match clientUserId set in step #2!
                AuthenticationMethod = "email",
                UserName = recipientName,
                Email = recipientEmail,


            };

            // create the recipient view (aka signing URL)
            ViewUrl recipientView = envelopesApi.CreateRecipientView(accountId, envelopeSummary.EnvelopeId, viewOptions);

            // Start the embedded signing session!
            //   System.Diagnostics.Process.Start(recipientView.Url);
            return recipientView.Url;

        }
        public string requestSignatureFromTemplateTest()
        {
            // Enter your DocuSign credentials
            string Username = "rohitj2@chetu.com";
            string Password = "Chetu@123";
            string IntegratorKey = "75f3fa5e-1cf2-4910-b3fa-62ec16228956";

            // specify the document we want signed
            string SignTest1File = @"D:\TrueSolutions-HM\ProjectDocuments/vizient_background_screen_services_agreement_copy_2.pdf";

            // Enter recipient (signer) name and email address
            string recipientName = "Umashankar";
            string recipientEmail = "umashankars@chetu.com";

            // instantiate api client with appropriate environment (for production change to www.docusign.net/restapi)
            string basePath = "https://demo.docusign.net/restapi";

            // instantiate a new api client
            ApiClient apiClient = new ApiClient(basePath);

            // set client in global config so we don't need to pass it to each API object
            Configuration.Default.ApiClient = apiClient;

            string authHeader = "{\"Username\":\"" + Username + "\", \"Password\":\"" + Password + "\", \"IntegratorKey\":\"" + IntegratorKey + "\"}";

            Configuration.Default.AddDefaultHeader("X-DocuSign-Authentication", authHeader);

            // we will retrieve this from the login() results
            string accountId = null;

            // the authentication api uses the apiClient (and X-DocuSign-Authentication header) that are set in Configuration object
            AuthenticationApi authApi = new AuthenticationApi();
            LoginInformation loginInfo = authApi.Login();
            // user might be a member of multiple accounts
            accountId = loginInfo.LoginAccounts[0].AccountId;



            // the document (file) we want signed
            string templateId = "b682959c-c93a-4a56-b4d8-b7d4f7e05320";
            string templateRoleName = "Test";


            EnvelopeDefinition envDef = new EnvelopeDefinition();
            envDef.EmailSubject = "DocuSign C# SDK Please sign this doc";

            // assign recipient to template role by setting name, email, and role name.  Note that the
            // template role name must match the placeholder role name saved in your account template.  
            //TemplateRole tRole = new TemplateRole();
            //tRole.Email = recipientEmail;
            //tRole.Name = recipientName;
            //tRole.RoleName = templateRoleName;
            //tRole.ClientUserId = "1234";
            //tRole.InPersonSignerName = "SAmple";
            CompositeTemplate ct = new CompositeTemplate();
            ServerTemplate st = new ServerTemplate();
            st.Sequence = "1";
            st.TemplateId = "b682959c-c93a-4a56-b4d8-b7d4f7e05320";

            InlineTemplate it = new InlineTemplate();
            it.Sequence = "1";


            // Add a recipient to sign the documeent
            Signer signer = new Signer();
            signer.Email = recipientEmail;
            signer.Name = recipientName;
            signer.RecipientId = "1";
            signer.RoleName = "SignMock";
            // Create a |SignHere| tab somewhere on the document for the recipient to sign
            signer.Tabs = new Tabs();
            signer.Tabs.SignHereTabs = new List<SignHere>();
            SignHere signHere = new SignHere();
            signHere.DocumentId = "1";
            signHere.PageNumber = "1";
            signHere.RecipientId = "1";
            signHere.XPosition = "100";
            signHere.YPosition = "100";
            signer.ClientUserId = "1234";

            signer.Tabs.SignHereTabs.Add(signHere);

            it.Recipients = new Recipients();
            it.Recipients.Signers = new List<Signer>();
            it.Recipients.Signers.Add(signer);


            List<ServerTemplate> stt = new List<ServerTemplate>();
            stt.Add(st);
            ct.ServerTemplates = stt;

            List<InlineTemplate> lin = new List<InlineTemplate>();
            lin.Add(it);
            ct.InlineTemplates = lin;


            List<CompositeTemplate> lsct = new List<CompositeTemplate>();
            lsct.Add(ct);
            lsct.Add(AddDoc());
            envDef.CompositeTemplates = lsct;

            // set envelope status to "sent" to immediately send the signature request
            envDef.Status = "sent";

            // |EnvelopesApi| contains methods related to creating and sending Envelopes (aka signature requests)
            EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope(accountId, envDef);


            EnvelopeDocumentsResult docsList = envelopesApi.ListDocuments(accountId, envelopeSummary.EnvelopeId);



            int docCount = docsList.EnvelopeDocuments.Count;
            // MemoryStream docStream = (MemoryStream)envelopesApi.GetDocument(accountId, envelopeSummary.EnvelopeId, docsList.EnvelopeDocuments[0].DocumentId);

            RecipientViewRequest viewOptions = new RecipientViewRequest()
            {
                //ReturnUrl = "https://www.docusign.com/devcenter",
                ReturnUrl = "http://localhost:56086/Registration/CompleteProcess",
                ClientUserId = "1234",  // must match clientUserId set in step #2!
                AuthenticationMethod = "email",
                UserName = recipientName,
                Email = recipientEmail,


            };

            // create the recipient view (aka signing URL)
            ViewUrl viewUrl = envelopesApi.CreateRecipientView(accountId, envelopeSummary.EnvelopeId, viewOptions);




            //ConsoleViewRequest consoleViewRequest = new ConsoleViewRequest();
            //consoleViewRequest.EnvelopeId = envelopeSummary.EnvelopeId;

            //ViewUrl viewUrl = envelopesApi.CreateConsoleView(accountId, consoleViewRequest);
            //  ViewUrl viewUrl2 = envelopesApi.CreateRecipientView(accountId, envelopeSummary.EnvelopeId, recipientView);

            return viewUrl.Url;

        }
        public string DocusignLogin()
        {
            // Enter your DocuSign credentials
            string Username = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
            string Password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
            string IntegratorKey = System.Configuration.ConfigurationSettings.AppSettings["IntegrationKey"];
            // instantiate api client with appropriate environment (for production change to www.docusign.net/restapi)
            //   string basePath = "https://demo.docusign.net/restapi";
            //https://www.docusign.net/restapi
            string basePath = System.Configuration.ConfigurationSettings.AppSettings["BasePath"];
            // instantiate a new api client
            ApiClient apiClient = new ApiClient(basePath);

            // set client in global config so we don't need to pass it to each API object
            Configuration.Default.ApiClient = apiClient;

            string authHeader = "{\"Username\":\"" + Username + "\", \"Password\":\"" + Password + "\", \"IntegratorKey\":\"" + IntegratorKey + "\"}";

            Configuration.Default.AddDefaultHeader("X-DocuSign-Authentication", authHeader);

            // we will retrieve this from the login() results
            string accountId = null;

            // the authentication api uses the apiClient (and X-DocuSign-Authentication header) that are set in Configuration object
            AuthenticationApi authApi = new AuthenticationApi();
            LoginInformation loginInfo = authApi.Login();
            // user might be a member of multiple accounts
            accountId = loginInfo.LoginAccounts[0].AccountId;
            apiClient = new ApiClient(loginInfo.LoginAccounts[0].BaseUrl);

            return accountId;

        }
        public string CreateEnvelop(string templateId)
        {
            string accountID = DocusignLogin();
            // string templateId = "fc6e1f7c-9632-40cd-89a2-c8f6026aa56d";
            string templateRoleName = "Optimum";
            //efeafc02 - d0ae - 4497 - 8647 - d6e5d27431b0

            string recipientName = "Umashankar";
            string recipientEmail = "umashankars@chetu.com";

            EnvelopeDefinition envDef = new EnvelopeDefinition();
            envDef.EmailSubject = "DocuSign C# SDK Please sign this doc";

            // assign recipient to template role by setting name, email, and role name.  Note that the
            // template role name must match the placeholder role name saved in your account template.  
            TemplateRole tRole = new TemplateRole();
            tRole.Email = recipientEmail;
            tRole.Name = recipientName;
            tRole.RoleName = templateRoleName;

            List<TemplateRole> rolesList = new List<TemplateRole>() { tRole };

            // add the role to the envelope and assign valid templateId from your account
            envDef.TemplateRoles = rolesList;
            envDef.TemplateId = templateId;


            // set envelope status to "sent" to immediately send the signature request
            envDef.Status = "sent";


            // MemoryStream docStream = (MemoryStream)envelopesApi.GetDocument(accountID, envelopeSummary.EnvelopeId, docsList.EnvelopeDocuments[0].DocumentId);

            // Add a recipient to sign the documeent
            Signer signer = new Signer();
            signer.Name = recipientName;
            signer.Email = recipientEmail;
            //  signer.RecipientId = "1";


            // must set |clientUserId| to embed the recipient
            signer.ClientUserId = "1234";
            // Create a |SignHere| tab somewhere on the document for the recipient to sign
            signer.Tabs = new Tabs();
            signer.Tabs.SignHereTabs = new List<SignHere>();
            SignHere signHere = new SignHere();
            signHere.DocumentId = "1";
            signHere.PageNumber = "23";
            //   signHere.RecipientId = "1";
            signHere.XPosition = "50";
            signHere.YPosition = "700";

            signer.Tabs.SignHereTabs.Add(signHere);

            envDef.Recipients = new Recipients();
            envDef.Recipients.Signers = new List<Signer>();
            envDef.Recipients.Signers.Add(signer);

            // create the recipient view (aka signing URL)
            //ViewUrl recipientView = envelopesApi.CreateRecipientView(accountID, envelopeSummary2.EnvelopeId, viewOptions);


            // |EnvelopesApi| contains methods related to creating and sending Envelopes (aka signature requests)
            EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope(accountID, envDef);

            EnvelopeDocumentsResult docsList = envelopesApi.ListDocuments(accountID, envelopeSummary.EnvelopeId);

            int docCount = docsList.EnvelopeDocuments.Count;


            // ConsoleViewRequest consoleViewRequest = new ConsoleViewRequest();
            // consoleViewRequest.EnvelopeId = envelopeSummary.EnvelopeId;

            //ViewUrl viewUrl = envelopesApi.CreateConsoleView(accountID, consoleViewRequest);

            RecipientViewRequest viewOptions = new RecipientViewRequest()
            {
                //ReturnUrl = "https://www.docusign.com/devcenter",
                ReturnUrl = "http://localhost:56086/Registration/CompleteProcess",
                //   ClientUserId = "1234",  // must match clientUserId set in step #2!
                AuthenticationMethod = "email",
                UserName = recipientName,
                Email = recipientEmail,


            };

            // create the recipient view (aka signing URL)
            ViewUrl viewUrl = envelopesApi.CreateRecipientView(accountID, envelopeSummary.EnvelopeId, viewOptions);

            return viewUrl.Url;
        }
        public CompositeTemplate AddDoc()
        {
            CompositeTemplate ct = new CompositeTemplate();
            ServerTemplate st = new ServerTemplate();
            st.Sequence = "2";
            st.TemplateId = "e1297f1c-8f3e-4dfd-8f94-ec7bee56bcd6";

            InlineTemplate it = new InlineTemplate();
            it.Sequence = "2";


            // Add a recipient to sign the documeent
            Signer signer = new Signer();
            signer.Email = "umashankars@chetu.com";
            signer.Name = "Umashankar";
            signer.RecipientId = "2";
            signer.RoleName = "SignDoc";
            signer.ClientUserId = "1234";
            // Create a |SignHere| tab somewhere on the document for the recipient to sign
            signer.Tabs = new Tabs();
            signer.Tabs.SignHereTabs = new List<SignHere>();
            SignHere signHere = new SignHere();
            signHere.DocumentId = "2";
            signHere.PageNumber = "1";
            signHere.RecipientId = "2";
            signHere.XPosition = "100";
            signHere.YPosition = "100";
            signer.Tabs.SignHereTabs.Add(signHere);

            it.Recipients = new Recipients();
            it.Recipients.Signers = new List<Signer>();
            it.Recipients.Signers.Add(signer);


            List<ServerTemplate> stt = new List<ServerTemplate>();
            stt.Add(st);
            ct.ServerTemplates = stt;

            List<InlineTemplate> lin = new List<InlineTemplate>();
            lin.Add(it);
            ct.InlineTemplates = lin;

            return ct;
            //List<CompositeTemplate> lsct = new List<CompositeTemplate>();
            //lsct.Add(ct);

        }
        public string CreateEnvelopFromTemplate()
        {
            string accountID = DocusignLogin();
            string templateId = "b682959c-c93a-4a56-b4d8-b7d4f7e05320";

            //efeafc02 - d0ae - 4497 - 8647 - d6e5d27431b0

            string recipientName = "Umashankar";
            string recipientEmail = "umashankars@chetu.com";

            EnvelopeDefinition envDef = new EnvelopeDefinition();
            envDef.EmailSubject = "DocuSign C# SDK Please sign this doc";

            // assign recipient to template role by setting name, email, and role name.  Note that the
            // template role name must match the placeholder role name saved in your account template.  
            TemplateRole tRole = new TemplateRole();
            tRole.Email = recipientEmail;
            tRole.Name = recipientName;
            tRole.RoleName = "SignMock";

            List<TemplateRole> rolesList = new List<TemplateRole>() { tRole };

            // add the role to the envelope and assign valid templateId from your account
            envDef.TemplateRoles = rolesList;
            envDef.TemplateId = templateId;


            // set envelope status to "sent" to immediately send the signature request
            envDef.Status = "sent";

            // |EnvelopesApi| contains methods related to creating and sending Envelopes (aka signature requests)
            EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope(accountID, envDef);

            RecipientViewRequest viewOptions = new RecipientViewRequest()
            {
                //ReturnUrl = "https://www.docusign.com/devcenter",
                ReturnUrl = "http://localhost:56086/Registration/CompleteProcess",
                //   ClientUserId = "1234",  // must match clientUserId set in step #2!
                AuthenticationMethod = "email",
                UserName = recipientName,
                Email = recipientEmail,


            };

            // create the recipient view (aka signing URL)
            ViewUrl viewUrl = envelopesApi.CreateRecipientView(accountID, envelopeSummary.EnvelopeId, viewOptions);

            //ConsoleViewRequest consoleViewRequest = new ConsoleViewRequest();
            //consoleViewRequest.EnvelopeId = envelopeSummary.EnvelopeId;

            //ViewUrl viewUrl = envelopesApi.CreateConsoleView(accountID, consoleViewRequest);
            return viewUrl.Url;
        }
        public List<CompositeTemplate> AddListDocument(List<Models.AgencyChkValue> lstAgencyChkValue, string signerName, string signerEmail)
        {
            List<CompositeTemplate> lstCompositeTemplate = new List<CompositeTemplate>();
            int Counter = 1;
            foreach (var agency in lstAgencyChkValue)
            {
                CompositeTemplate objcompositeTemplate = new CompositeTemplate();
                ServerTemplate objServerTemplate = new ServerTemplate();
                objServerTemplate.Sequence = Counter.ToString();
                objServerTemplate.TemplateId = objGetData.GetTemplate(agency.AgencyID);
                InlineTemplate objInlineTemplate = new InlineTemplate();
                objInlineTemplate.Sequence = Counter.ToString();

                // Add a recipient to sign the documeent
                Signer objsigner = new Signer();
                objsigner.Email = signerEmail;
                objsigner.Name = signerName;
                objsigner.RecipientId = Counter.ToString();
                objsigner.RoleName = "SignDoc";
                objsigner.ClientUserId = signerName;


                //   // objsignerate a |SignHere| tab somewhere on the document for the recipient to sign
                //objsigner.Tabs = new Tabs();
                //objsigner.Tabs.SignHereTabs = new List<SignHere>();
                //SignHere objSignHere = new SignHere();
                //objSignHere.DocumentId = Counter.ToString();
                //objSignHere.PageNumber = "1";
                //objSignHere.RecipientId = "2";
                //objSignHere.XPosition = "300";
                //objSignHere.YPosition = "100";
                //objsigner.Tabs.SignHereTabs.Add(objSignHere);

                objInlineTemplate.Recipients = new Recipients();

                objInlineTemplate.Recipients.Signers = new List<Signer>();
                objInlineTemplate.Recipients.Signers.Add(objsigner);


                List<ServerTemplate> lstServerTemplate = new List<ServerTemplate>();
                lstServerTemplate.Add(objServerTemplate);
                objcompositeTemplate.ServerTemplates = lstServerTemplate;

                List<InlineTemplate> lstInlineTemplate = new List<InlineTemplate>();
                lstInlineTemplate.Add(objInlineTemplate);
                objcompositeTemplate.InlineTemplates = lstInlineTemplate;

                lstCompositeTemplate.Add(objcompositeTemplate);
                Counter++;
            }

            return lstCompositeTemplate;
        }
        public string GeneratingDocuments(List<Models.AgencyChkValue> lstAgencyChkValue, string signerName, string signerEmail)
        {
            string accountID = DocusignLogin();

            EnvelopeDefinition objEnvelopeDefinition = new EnvelopeDefinition();
            objEnvelopeDefinition.EmailSubject = "Please sign this doc";


            objEnvelopeDefinition.CompositeTemplates = AddListDocument(lstAgencyChkValue, signerName, signerEmail);

            // set envelope status to "sent" to immediately send the signature request
            objEnvelopeDefinition.Status = "sent";

            // |EnvelopesApi| contains methods related to creating and sending Envelopes (aka signature requests)
            EnvelopesApi envelopesApi = new EnvelopesApi();

            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope(accountID, objEnvelopeDefinition);

            RecipientViewRequest viewOptions = new RecipientViewRequest()
            {
                ReturnUrl = System.Configuration.ConfigurationSettings.AppSettings["ReturnURl"],
                ClientUserId = signerName,  // must match clientUserId set in step #2!
                AuthenticationMethod = "email",
                UserName = signerName,
                Email = signerEmail,
            };

            // create the recipient view (aka signing URL)
            ViewUrl viewUrl = envelopesApi.CreateRecipientView(accountID, envelopeSummary.EnvelopeId, viewOptions);
            return viewUrl.Url;

        }
    }
}