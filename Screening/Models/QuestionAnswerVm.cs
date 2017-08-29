using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Screening.Models
{
    public class QuestionAnswerVm
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public QuestionVM tblQuestion { get; set; }
        public string Answer { get; set; }
        public int UserID { get; set; }
    }
}