using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class DailyMealPlan : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public DailyMealPlan()
        {
            _context = new FitnessDBDataContext();
        }

        public void AddPlanAlimentarZilnic(List<Retete> retete, int userID)
        {
            var planZilnic = new PlanAlimentarZilnic
            {
                Data = DateTime.Now,
                Nume = $"Plan Zilnic {DateTime.Now.ToShortDateString()}",
                UserID = userID
            };
            _context.PlanAlimentarZilnics.InsertOnSubmit(planZilnic);
            _context.SubmitChanges();
            var retetePlan = retete.Select(reteta => new RetetePlanAlimentarZilnic
            {
                ReteteID = reteta.ID,
                PlanAlimentarZilnicID = planZilnic.ID
            });

            _context.RetetePlanAlimentarZilnics.InsertAllOnSubmit(retetePlan);
            _context.SubmitChanges();
        }

        public List<Retete> GetPlanAlimentarZilnic(int userID, DateTime data)
        {
            return _context.PlanAlimentarZilnics
                .Where(paz => paz.UserID == userID && paz.Data.Date == data.Date)
                .Join(_context.RetetePlanAlimentarZilnics,
                    paz => paz.ID,
                    rpaz => rpaz.PlanAlimentarZilnicID,
                    (paz, rpaz) => rpaz.ReteteID)
                .Join(_context.Retetes,
                    retId => retId,
                    ret => ret.ID,
                    (retId, ret) => ret)
                .ToList();
        }

    }
}