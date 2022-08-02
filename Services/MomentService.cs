using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IMomentService
    {
        IEnumerable<Moment> GetAll();
        Moment GetById(int id);
        Moment Create(Moment moment, List<Register> registers);
        void Update(Moment moment);
        void Delete(int id);
    }

    public class MomentService : IMomentService
    {
        private DataContext _context;

        public MomentService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Moment> GetAll()
        {
            return _context.Moments;
        }

        public Moment GetById(int id)
        {
            return _context.Moments.Find(id);
        }

        public Moment Create(Moment moment, List<Register> registers)
        {
            // validation
            if (_context.Moments.Any(x => x.DateTime == moment.DateTime))
                throw new AppException("Data e hora já registrada");

            DateTime dt;

            bool isValidDateTime = DateTime.TryParseExact(moment.DateTime,
                       "yyyy-MM-dd HH:mm:ss",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dt);

            if (!isValidDateTime)
            {
                throw new AppException("Data e hora em formato inválido");
            }

            _context.Moments.Add(moment);

            var date = DateTime.ParseExact(moment.DateTime, "yyyy-MM-dd HH:mm:ss",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None);

            if(!registers.Where(x => DateTime.ParseExact(x.Day, "yyyy-MM-dd HH:mm:ss",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None).Day == date.Day).Any())
            {
                foreach (var r in registers)
                {
                    if (r.Day == date.ToString())
                    {
                        r.DayOut = date.Hour.ToString();
                        _context.Registers.Update(r);
                        _context.SaveChanges();
                    }
                    else if (r.Day == date.ToString() && (!String.IsNullOrEmpty(r.DayOut)
                                                           && String.IsNullOrEmpty(r.AfternoonIn)
                                                           && String.IsNullOrEmpty(r.AfternoonOut)))
                    {
                        r.AfternoonIn = date.Hour.ToString();
                        _context.Registers.Update(r);
                        _context.SaveChanges();
                    }
                    else if (r.Day == date.ToString() && (!String.IsNullOrEmpty(r.DayOut)
                                                           && !String.IsNullOrEmpty(r.AfternoonIn)
                                                           && String.IsNullOrEmpty(r.AfternoonOut)))
                    {
                        r.AfternoonOut = date.Hour.ToString();
                        _context.Registers.Update(r);
                        _context.SaveChanges();
                    }
                    else
                    {
                        //Nothing to do
                    }
                }
            } else
            {
                Register register = new Register();
                register.Day = date.ToString();
                register.DayIn = date.Hour.ToString();
                register.DayOut = "";
                register.AfternoonIn = "";
                register.AfternoonOut = "";

                _context.Registers.Add(register);
                _context.SaveChanges();
            }

            return moment;
        }

        public void Update(Moment momentParam)
        {
            var moment = _context.Moments.Find(momentParam.Id);

            if (moment == null)
                throw new AppException("Moment not found");

            // update message if it has changed
            if (momentParam.DateTime != moment.DateTime)
            {
                // throw error if the new username is already taken
                if (_context.Moments.Any(x => x.DateTime == momentParam.DateTime))
                    throw new AppException("Moment " + momentParam.DateTime + " already updated");

                moment.DateTime = momentParam.DateTime;
            }

            _context.Moments.Update(moment);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var moment = _context.Moments.Find(id);
            if (moment != null)
            {
                _context.Moments.Remove(moment);
                _context.SaveChanges();
            }
        }
    }
}

