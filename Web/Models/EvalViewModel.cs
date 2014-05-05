using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YQ.Web.DB;

namespace YQ.Web.Models
{
    public class EvalFormVM
    {
        public Guid EvalFormId { get; set; }
        public string Name { get; set; }
        public List<EvalQuestionVM> EvalQuestions { get; set; }
        public List<EvalInstance> EvalInstances { get; set; }
    }

    public class EvalQuestionVM
    {
        public Guid EvalQuestionId { get; set; }
        public Guid? EvalFormId { get; set; }
        //public int Order { get; set; }
        //public int SubOrder { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Answer { get; set; }
        public List<bool> OptionAnswers { get; set; }
        public List<string> Options { get; set; }
        public EvalQuestionType QuestionType { get; set; }
        public List<EvalQuestionVM> EvalQuestions { get; set; }
    }

    public class EvalInstanceVM
    {
        public Guid EvalInstanceId { get; set; }
        public Guid EvalFormId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}