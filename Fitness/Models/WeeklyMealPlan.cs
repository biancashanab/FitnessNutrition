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
    public class WeeklyMealPlan : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime DataInceput { get; set; }
        public DateTime DataSfarsit { get; set; }
        private readonly FitnessDBDataContext _context;
        public event PropertyChangedEventHandler PropertyChanged;

        public WeeklyMealPlan()
        {
            _context = new FitnessDBDataContext();
        }

        public void AddPlanAlimentarSaptamanal
            (List<PlanAlimentarZilnic> planuriZilnice, int userID)
        {
            var planSaptamanal = new PlanAlimentarSaptamanal
            {
                Nume = $"Plan Săptămânal {DateTime.Now.ToShortDateString()}",
                UserID = userID,
                DataInceput = DateTime.Now,
                DataSfarsit = DateTime.Now.AddDays(7)
            };
            _context.PlanAlimentarSaptamanals.InsertOnSubmit(planSaptamanal);
            _context.SubmitChanges();
            var planuriLegatura = planuriZilnice.Select(plan => new PlanAlimentarSaptamanal_Zilnic
            {
                PlanAlimentarSaptamanalID = planSaptamanal.ID,
                PlanAlimentarZilnicID = plan.ID
            });
            _context.PlanAlimentarSaptamanal_Zilnics.InsertAllOnSubmit(planuriLegatura);
            _context.SubmitChanges();
        }

        public List<PlanAlimentarZilnic> GetPlanAlimentarSaptamanal(int userID, DateTime data)
        {
                DateTime endDate = data.AddDays(7);
                return (from pas in _context.PlanAlimentarSaptamanals
                        join pasz in _context.PlanAlimentarSaptamanal_Zilnics
                            on pas.ID equals pasz.PlanAlimentarSaptamanalID
                        join paz in _context.PlanAlimentarZilnics
                            on pasz.PlanAlimentarZilnicID equals paz.ID
                        where pas.UserID == userID
                            && paz.Data >= data
                            && paz.Data <= endDate
                        select paz).ToList();
        }

    }
}
