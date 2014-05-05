using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YQ.Web.DB;
using YQ.Web.Models;

namespace YQ.Web.Core
{
    public partial class Ops
    {
        public class Eval
        {
            static void RecurrsiveSaveQuestionAnswer(EvalContext context, EvalQuestionVM question, Guid instanceId)
            {
                if (question.QuestionType == EvalQuestionType.Comment)
                {
                    EvalQuestionAnswer answer = new EvalQuestionAnswer()
                    {
                        EvalQuestionAnswerId = Guid.NewGuid(),
                        EvalInstanceId = instanceId,
                        EvalQuestionId = question.EvalQuestionId,
                        Answer = question.Answer
                    };
                    context.Answers.Add(answer);
                }
                else if (question.EvalQuestions == null)
                {
                    for (int oi = 0; oi < question.Options.Count; oi++)
                    {
                        EvalQuestionAnswer answer = new EvalQuestionAnswer()
                        {
                            EvalQuestionAnswerId = Guid.NewGuid(),
                            EvalInstanceId = instanceId,
                            EvalQuestionId = question.EvalQuestionId,
                            Answer = (question.OptionAnswers != null && question.OptionAnswers[oi]) ? question.Options[oi] : question.Answer
                        };
                        context.Answers.Add(answer);
                    }
                }

                if (question.EvalQuestions != null)
                {
                    foreach (var sq in question.EvalQuestions)
                    {
                        RecurrsiveSaveQuestionAnswer(context, sq, instanceId);
                    }
                }
            }

            public static bool SaveInstance(EvalFormVM model)
            {
                EvalContext context = new EvalContext();

                EvalInstance form = new EvalInstance()
                {
                    EvalInstanceId = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow,
                    EvalFormId = model.EvalFormId
                };

                context.Instances.Add(form);

                if (model.EvalQuestions != null)
                {
                    foreach (var sq in model.EvalQuestions)
                    {
                        RecurrsiveSaveQuestionAnswer(context, sq, form.EvalInstanceId);
                    }
                }
                context.SaveChanges();
                return true;
            }

            static void RecurrsiveSaveQuestion(EvalContext context, EvalQuestionJSModel question, EvalForm form, EvalQuestion parent)
            {
                EvalQuestion newq = new EvalQuestion()
                {
                    EvalQuestionId = Guid.NewGuid(),
                    EvalForm = form,
                    ParentQuestion = parent,
                    Description = question.Description,
                    Heading = question.Heading,
                    OptionString = question.Options,
                    QuestionType = question.QuestionType
                };
                context.Questions.Add(newq);

                if (question.SubQuestions != null)
                {
                    foreach (var sq in question.SubQuestions)
                    {
                        RecurrsiveSaveQuestion(context, sq, null, newq);
                    }
                }
            }

            public static bool SaveForm(EvalFormJSModel model)
            {
                EvalContext context = new EvalContext();

                EvalForm form = new EvalForm()
                {
                    EvalFormId = Guid.NewGuid(),
                    Name = model.Name
                };

                context.Forms.Add(form);

                if (model.Questions != null)
                {
                    foreach (var sq in model.Questions)
                    {
                        RecurrsiveSaveQuestion(context, sq, form, null);
                    }
                }
                context.SaveChanges();

                return true;
            }
        }
    }
}