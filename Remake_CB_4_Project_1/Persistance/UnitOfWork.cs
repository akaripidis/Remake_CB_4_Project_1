using Remake_CB_4_Project_1.Core;
using Remake_CB_4_Project_1.Core.Repositories;
using Remake_CB_4_Project_1.Persistance.Repositories;
using System;

namespace Remake_CB_4_Project_1.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MessageAppContext _context;
        private static int instanceCount;
        public static int Count => instanceCount;

        private UnitOfWork(MessageAppContext context)
        {
            instanceCount++;
            _context = context;
            Users = new UserRepository(_context);
            Messages = new MessageRepository(_context);
        }

        public IUserRepository Users { get; private set; }
        public IMessageRepository Messages { get; private set; }

        private static Lazy<UnitOfWork> instance = new Lazy<UnitOfWork>(()=>new UnitOfWork(new MessageAppContext()));
        public static UnitOfWork Instance => instance.Value;

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
