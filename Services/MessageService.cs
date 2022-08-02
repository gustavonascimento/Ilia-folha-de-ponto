using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IMessageService
    {
        IEnumerable<Message> GetAll();
        Message GetById(int id);
        Message Create(Message allocation);
        void Update(Message allocation);
        void Delete(int id);
        void CreateMessages();
    }

    public class MessageService : IMessageService
    {
        private DataContext _context;

        public MessageService(DataContext context)
        {
            _context = context;
        }

        public void CreateMessages()
        {
            if (!_context.Messages.Any())
            {
                Message message = new Message();
                message.MessageDescription = "Data e hora em formato inválido";
                _context.Messages.Add(message);

                Message message2 = new Message();
                message2.MessageDescription = "Campo obrigatório não informado";
                _context.Messages.Add(message2);

                Message message3 = new Message();
                message3.MessageDescription = "Apenas 4 horários podem ser registrados por dia";
                _context.Messages.Add(message3);

                Message message4 = new Message();
                message4.MessageDescription = "Deve haver no mínimo 1 hora de almoço";
                _context.Messages.Add(message4);

                Message message5 = new Message();
                message5.MessageDescription = "Horários já registrado";
                _context.Messages.Add(message5);

                Message message6 = new Message();
                message6.MessageDescription = "Não pode alocar tempo maior que o tempo trabalhado no dia";
                _context.Messages.Add(message6);

                _context.SaveChanges();
            }
        }

        public IEnumerable<Message> GetAll()
        {
            return _context.Messages;
        }

        public Message GetById(int id)
        {
            return _context.Messages.Find(id);
        }


        public Message Create(Message message)
        {
            // validation
            if (_context.Messages.Any(x => x.MessageDescription == message.MessageDescription))
                throw new AppException("Message \"" + message.MessageDescription + "\" is already registered");

            _context.Messages.Add(message);
            _context.SaveChanges();

            return message;
        }

        public void Update(Message messageParam)
        {
            var message = _context.Messages.Find(messageParam.Id);

            if (message == null)
                throw new AppException("Message not found");

            // update message if it has changed
            if (messageParam.MessageDescription != message.MessageDescription)
            {
                // throw error if the new username is already taken
                if (_context.Allocations.Any(x => x.ProjectName == messageParam.MessageDescription))
                    throw new AppException("Message " + messageParam.MessageDescription + " already updated");

                message.MessageDescription = messageParam.MessageDescription;
            }

            _context.Messages.Update(message);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var message = _context.Messages.Find(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
        }
    }
}

