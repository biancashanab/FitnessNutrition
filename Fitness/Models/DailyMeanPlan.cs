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
            var reteteList = string.Join(",", retete.Select(r => r.ID));
            var numePlanZilnic = $"Plan Zilnic {DateTime.Now.ToShortDateString()}";
            _context.ExecuteCommand(
                "EXEC addPlanAlimentarZilnic @UserID = {0}, @Data = {1}, @Nume = {2}, @ReteteList = {3}",
                userID,
                DateTime.Now,
                numePlanZilnic,
                reteteList
            );
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