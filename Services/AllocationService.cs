using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IAllocationService
    {
        IEnumerable<Allocation> GetAll();
        Allocation GetById(int id);
        Allocation Create(Allocation allocation);
        void Update(Allocation allocation);
        void Delete(int id);
    }

    public class AllocationService : IAllocationService
    {
        private DataContext _context;

        public AllocationService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Allocation> GetAll()
        {
            return _context.Allocations;
        }

        public Allocation GetById(int id)
        {
            return _context.Allocations.Find(id);
        }

        public Allocation Create(Allocation allocation)
        {
            // validation
            if (_context.Allocations.Any(x => x.ProjectName == allocation.ProjectName))
                throw new AppException("Project \"" + allocation.ProjectName + "\" is already taken");

            DateTime dt;

            bool dateIsValid = DateTime.TryParseExact(
                allocation.Date,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dt);

            if (!dateIsValid)
            {
                throw new AppException("Date in wrong format. Correct format: yyyy-MM-dd");
            }

            bool timeIsValid = TimeSpan.TryParseExact(allocation.Time, "g", CultureInfo.CurrentCulture, out TimeSpan ts1);

            if(!timeIsValid)
            {
                throw new AppException("Time in wrong format. Correct format: hh:mm:ss");
            }

            //TimeSpan horarioSaida

            _context.Allocations.Add(allocation);
            _context.SaveChanges();

            return allocation;
        }

        public void Update(Allocation allocationParam)
        {
            var allocation = _context.Allocations.Find(allocationParam.Id);

            if (allocation == null)
                throw new AppException("Allocation not found");

            // update date if it has changed
            if (allocationParam.Date != allocation.Date)
            {
                // throw error if the new username is already taken
                if (_context.Allocations.Any(x => x.Date == allocationParam.Date))
                    throw new AppException("Allocation " + allocationParam.Date + " already updated");

                allocation.Date = allocationParam.Date;
            }

            // update time if it has changed
            if (allocationParam.Time != allocation.Time)
            {
                // throw error if the new username is already taken
                if (_context.Allocations.Any(x => x.Time == allocationParam.Time))
                    throw new AppException("Allocation " + allocationParam.Time + " already updated");

                allocation.Time = allocationParam.Time;
            }

            // update project name if it has changed
            if (allocationParam.ProjectName != allocation.ProjectName)
            {
                // throw error if the new username is already taken
                if (_context.Allocations.Any(x => x.ProjectName == allocationParam.ProjectName))
                    throw new AppException("Allocation " + allocationParam.ProjectName + " already updated");

                allocation.ProjectName = allocationParam.ProjectName;
            }

            _context.Allocations.Update(allocation);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var allocation = _context.Allocations.Find(id);
            if (allocation != null)
            {
                _context.Allocations.Remove(allocation);
                _context.SaveChanges();
            }
        }
    }
    
}
