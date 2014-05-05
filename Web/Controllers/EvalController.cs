using System.Linq;
using System.Web.Mvc;
using YQ.Web.Core;
using YQ.Web.DB;
using YQ.Web.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
namespace YQ.Web.Controllers
{
    public class EvalController : Controller
    {
        EvalContext _db = new EvalContext();

        public ActionResult FillForm(Guid? id)
        {
            EvalForm form = _db.Forms.Where(i => i.EvalFormId == (id ?? Guid.Empty)).FirstOrDefault();

            if (form == null)
                return RedirectToAction("FormList");

            return View(Mapper.DynamicMap<EvalFormVM>(form));
        }

        [HttpPost]
        public ActionResult FillForm(EvalFormVM model)
        {
            if (ModelState.IsValid)
            {
                if (Ops.Eval.SaveInstance(model))
                {
                    return RedirectToAction("FormList");
                }
                ModelState.AddModelError("", "Cannot Save Form");
            }
            return View(model);
        }

        public ActionResult AddForm() {
            ViewBag.EvalQuestionJSModel = new EvalQuestionJSModel() { SubQuestions = new List<EvalQuestionJSModel>() };
            ViewBag.EvalQuestionTypes = typeof(DB.EvalQuestionType).ToList();
            return View(new EvalFormJSModel() { Questions = new List<EvalQuestionJSModel>() });
        }

        [HttpPost]
        public ActionResult AddForm(EvalFormJSModel model)
        {
            ViewBag.EvalQuestionJSModel = new EvalQuestionJSModel() { SubQuestions = new List<EvalQuestionJSModel>() };
            ViewBag.EvalQuestionTypes = typeof(DB.EvalQuestionType).ToList();
            if (ModelState.IsValid)
            {
                if (Ops.Eval.SaveForm(model))
                {
                    return RedirectToAction("FormList");
                }
                ModelState.AddModelError("", "Cannot Save Form");
            }
            return View(model);
        }

        public ActionResult FormList()
        {
            return View(_db.Forms.ToList().Select(f => Mapper.DynamicMap<EvalFormVM>(f)));
        }

        public ActionResult InstanceList(Guid? id)
        {
            IEnumerable<EvalInstance> instances = _db.Instances.Where(i => i.EvalFormId == (id ?? Guid.Empty));

            if (instances.Any())
                return View(instances.Select(i => Mapper.DynamicMap<EvalInstanceVM>(i)));

            return RedirectToAction("FormList");
        }

        public ActionResult InstanceDetails(Guid? id)
        {
            EvalInstance instance = _db.Instances.Where(i => i.EvalInstanceId == (id ?? Guid.Empty)).FirstOrDefault();

            if (instance == null)
                return RedirectToAction("FormList");

            return View(Mapper.DynamicMap<EvalFormVM>(instance.EvalForm));
        }
	}
}