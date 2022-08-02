using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface INewAccessUserService
    {
        // User Authenticate(string username, string password);
        NewAccessUser NewAccessRequest(NewAccessUser user);
        IEnumerable<NewAccessUser> GetAll();
        string RemoveSpecialCharacters(string str);
        NewAccessUser GetById(int id);
        void Delete(int id);
    }

    public class NewAccessUserService : INewAccessUserService
    {
        private DataContext _context;

        public NewAccessUserService(DataContext context)
        {
            _context = context;
        }

        public NewAccessUser NewAccessRequest(NewAccessUser user)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.Cpf))
                throw new AppException("CNPJ mandatory.");

            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException("User " + user.Email + " already registered.");

            if (_context.NewAccessUsers.Any(x => x.Email == user.Email))
                throw new AppException("User " + user.Email + " already registered.");
        
            _context.NewAccessUsers.Add(user);
            _context.SaveChanges();
            
            return user;
        }

        public IEnumerable<NewAccessUser> GetAll()
        {
            return _context.NewAccessUsers;
        }

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public NewAccessUser GetById(int id)
        {
            return _context.NewAccessUsers.Find(id);
        }

        public void Delete(int id)
        {
            var user = _context.NewAccessUsers.Find(id);
            if (user != null)
            {
                _context.NewAccessUsers.Remove(user);
                _context.SaveChanges();
            }
        }


    }
}