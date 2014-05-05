using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace YQ.Web.DB
{
    public class EvalContext : DbContext
    {
        public EvalContext()
            : base("EvalConnection")
        {
        }
        public DbSet<EvalQuestion> Questions { get; set; }
        public DbSet<EvalForm> Forms { get; set; }
        public DbSet<EvalInstance> Instances { get; set; }
        public DbSet<EvalQuestionAnswer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EvalQuestion>().Ignore(q => q.Answer);
            //modelBuilder.Entity<EvalQuestion>().HasOptional(q => q.ParentQuestion);
            //modelBuilder.Entity<EvalQuestion>().HasRequired(f => f.EvalForm).WithRequiredPrincipal().WillCascadeOnDelete();
        }
    }

    public enum EvalQuestionType
    {
        [Description("Multi-Select Multiple Choice")]
        MSMC,
        [Description("Single-Select Multiple Choice")]
        SSMC,
        [Description("Short Answer")]
        Comment,
        [Description("Parent Question")]
        Parent
    }

    public class EvalForm
    {
        public Guid EvalFormId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EvalQuestion> EvalQuestions { get; set; }
        public virtual ICollection<EvalInstance> EvalInstances { get; set; }
    }

    public class EvalQuestion
    {
        public Guid EvalQuestionId { get; set; }
        public int Order { get; set; }
        public int SubOrder { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string OptionString { get; set; }
        public EvalQuestionType QuestionType { get; set; }

        public Guid? ParentQuestionId { get; set; }
        public virtual EvalQuestion ParentQuestion { get; set; }

        public Guid? EvalFormId { get; set; }
        public virtual EvalForm EvalForm { get; set; }

        public virtual ICollection<EvalQuestion> EvalQuestions { get; set; }

        public virtual ICollection<EvalQuestionAnswer> EvalQuestionAnswers { get; set; }

        #region ViewModel Mapping Properties
        public List<string> Options
        {
            get
            {
                if (string.IsNullOrEmpty(OptionString))
                {
                    return new List<string>();
                }

                return OptionString.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n').ToList();
            }
        }
        public string Answer
        {
            get
            {
                if (!EvalQuestionAnswers.Any())
                    return "";

                if (QuestionType == EvalQuestionType.MSMC)
                {
                    return string.Join(", ", EvalQuestionAnswers.Where(a => !string.IsNullOrEmpty(a.Answer)).Select(a => a.Answer));
                }
                else
                {
                    return EvalQuestionAnswers.First().Answer;
                }
            }
        }
        public List<bool> OptionAnswers
        {
            get
            {
                List<bool> result = new List<bool>();

                foreach (string option in Options)
                {
                    result.Add(EvalQuestionAnswers.Any() && EvalQuestionAnswers.Where(a => a.Answer == option).Any());
                }
                return result;
            }
        }
        #endregion
    }

    public class EvalInstance
    {
        public Guid EvalInstanceId { get; set; }
        public Guid EvalFormId { get; set; }
        public virtual EvalForm EvalForm { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class EvalQuestionAnswer
    {
        public Guid EvalQuestionAnswerId { get; set; }
        public Guid EvalQuestionId { get; set; }
        public virtual EvalQuestion EvalQuestion { get; set; }
        public Guid EvalInstanceId { get; set; }
        public virtual EvalInstance EvalInstance { get; set; }
        public string Answer { get; set; }
    }

    public class EvalDatabaseInitializer : DropCreateDatabaseIfModelChanges<EvalContext>
    {
        protected override void Seed(EvalContext context)
        {
            #region test data
            EvalForm f1 = new EvalForm()
            {
                EvalFormId = new Guid("11111111-0000-0000-0000-000000000000"),
                Name = "Test"
            };

            EvalQuestion q1 = new EvalQuestion()
            {
                EvalQuestionId = new Guid("00000000-1111-0000-0000-000000000000"),
                Description = "Test Question 1 Description",
                Heading = "Test Question 1 Heading",
                OptionString = "a\r\nb\r\nc",
                QuestionType = EvalQuestionType.SSMC,
                Order = 1,
                EvalForm = f1
            };
            EvalQuestion q2 = new EvalQuestion()
            {
                EvalQuestionId = new Guid("00000000-2222-0000-0000-000000000000"),
                Description = "Test Question 2 Description",
                Heading = "Test Question 2 Heading",
                OptionString = "aa\r\nbb\r\ncc",
                QuestionType = EvalQuestionType.MSMC,
                Order = 2,
                EvalForm = f1
            };
            EvalQuestion q3 = new EvalQuestion()
            {
                EvalQuestionId = new Guid("00000000-3333-0000-0000-000000000000"),
                Description = "Test Question 3 Description",
                Heading = "Test Question 3 Heading",
                QuestionType = EvalQuestionType.Comment,
                Order = 3,
                EvalForm = f1
            };
            EvalQuestion q21 = new EvalQuestion()
            {
                EvalFormId = null,
                EvalQuestionId = new Guid("00000000-2222-1111-0000-000000000000"),
                Description = "Test Question 2.1 Description",
                Heading = "Test Question 2.1 Heading",
                QuestionType = EvalQuestionType.Comment,
                Order = 1,
                ParentQuestion = q2
            };
            EvalQuestion q22 = new EvalQuestion()
            {
                EvalFormId = null,
                EvalQuestionId = new Guid("00000000-2222-2222-0000-000000000000"),
                Description = "Test Question 2.1 Description",
                Heading = "Test Question 2.1 Heading",
                OptionString = "aaa\r\nbbb\r\nccc",
                QuestionType = EvalQuestionType.MSMC,
                Order = 2,
                ParentQuestion = q2
            };
            #endregion

            context.Questions.Add(q1);
            context.Questions.Add(q2);
            context.Questions.Add(q3);
            context.Questions.Add(q21);
            context.Questions.Add(q22);
            context.Forms.Add(f1);
        }
    }
}