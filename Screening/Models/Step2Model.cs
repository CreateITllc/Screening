


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Screening.Models
{
    public class Step2Model
    {

        Dictionary<string, object> dynamicProperties = new Dictionary<string, object>();

        public void AddProperty(string key, object value)
        {
            dynamicProperties[key] = value;
        }

        [Required(ErrorMessage = "Please Enter CompanyCorporateName")]
        public string CompanyName { get; set; }

        public string BusinessStructure { get; set; }

        [Required(ErrorMessage = "Please Enter ContactFirstName")]
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        [Required(ErrorMessage = "Please Enter ContactEmail")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9_\.\-]{2,4})+$", ErrorMessage = "Please Enter Valid Email Address")] 
        public string ContactEmail { get; set; }
        [Required(ErrorMessage = "Please Enter Contactdirectphone")]
        public string Contactdirectphone { get; set; }
        [Required(ErrorMessage = "Please Enter ContactCellPhone")]
        public string ContactCellPhone { get; set; }

        public string Contact2FirstName { get; set; }
        public string Contact2LastName { get; set; }
        public string Contact2Email { get; set; }
        public string Contact2directphone { get; set; }
        public string Contact2CellPhone { get; set; }

        public string CompanyWebsiteAddress { get; set; }

        [Required(ErrorMessage = "Please Enter CorporateFullAddress")]
        public string CorporateFullAddress { get; set; }

        public string Emailcredentialsto { get; set; }
        public string Emailagreementto { get; set; }
        public string Estimatednumberofdrugscreenings { get; set; }
        [Required(ErrorMessage = "Please Enter CorporateFullAddress")]
        public string EstimatedNumberofBackgroundScreenings { get; set; }
        public bool RadialUMassMemorialMedicalCenter { get; set; }
        public bool HonorHealth { get; set; }
        public bool isChecked { get; set; }
      

        public List<Agency> Agency { get; set; }
        
    }
}