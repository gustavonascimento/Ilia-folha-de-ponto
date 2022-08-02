using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IReportService
    {
        IEnumerable<Report> GetAll();
        Report GetById(int id);
        Report Create(Report allocation);
        void Delete(int id);
    }

    public class ReportService
    {
        private DataContext _context;

        public ReportService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> GetAll()
        {
            return _context.Reports;
        }

        public Report GetById(int id)
        {
            return _context.Reports.Find(id);
        }

        public Report Create(Report report)
        {
            // validation
            //if (_context.Reports.Any(x => x.Day == register.Day))
            //    throw new AppException("Day \"" + register.Day + "\" is already registered");

            _context.Reports.Add(report);
            _context.SaveChanges();

            return report;
        }

        public void Delete(int id)
        {
            var report = _context.Reports.Find(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                _context.SaveChanges();
            }
        }
    }
}

