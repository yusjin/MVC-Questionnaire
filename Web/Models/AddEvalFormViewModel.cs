using ExpressiveAnnotations.ConditionalAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YQ.Web.Models
{
    public class EvalFormJSModel
    {
        [Required]
        public string Name { get; set; }
        public List<EvalQuestionJSModel> Questions { get; set; }
    }

    public class EvalQuestionJSModel
    {
        [Required]
        public string Heading { get; set; }
        public string Description { get; set; }
        [RequiredIfExpression(
            ErrorMessage = "Options are required for multiple choice questions.",
            Expression = "{0} || {1}",
            DependentProperties = new[] { "QuestionType", "QuestionType" },
            TargetValues = new object[] { DB.EvalQuestionType.MSMC, DB.EvalQuestionType.SSMC }
            )]
        public string Options { get; set; }
        [Required]
        public DB.EvalQuestionType QuestionType { get; set; }
        [RequiredIf(
            ErrorMessage = "Sub questions are requires for parent question.",
            DependentProperty = "QuestionType",
            TargetValue = DB.EvalQuestionType.Parent
            )]
        public List<EvalQuestionJSModel> SubQuestions { get; set; }
    }
}