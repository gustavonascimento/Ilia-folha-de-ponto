using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IRegisterService
    {
        IEnumerable<Register> GetAll();
        Register GetById(int id);
        Register Create(Register register);
        void Update(Register register);
        void Delete(int id);
    }

    public class RegisterService : IRegisterService
    {
        private DataContext _context;

        public RegisterService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Register> GetAll()
        {
            return _context.Registers;
        }

        public Register GetById(int id)
        {
            return _context.Registers.Find(id);
        }

        public Register Create(Register register)
        {
            // validation
            if (_context.Registers.Any(x => x.Day == register.Day))
                throw new AppException("Day \"" + register.Day + "\" is already registered");

            _context.Registers.Add(register);
            _context.SaveChanges();

            return register;
        }

        public void Update(Register registerParam)
        {
            var register = _context.Registers.Find(registerParam.Id);

            if (register == null)
                throw new AppException("Register not found");

            // update message if it has changed
            if (registerParam.Day != register.Day)
            {
                // throw error if the new username is already taken
                if (_context.Registers.Any(x => x.Day == registerParam.Day))
                    throw new AppException("Day " + registerParam.Day + " already updated");

                register.Day = registerParam.Day;
            }

            _context.Registers.Update(register);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var register = _context.Registers.Find(id);
            if (register != null)
            {
                _context.Registers.Remove(register);
                _context.SaveChanges();
            }
        }
    }
}

