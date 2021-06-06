using Infrastructure.Aggregets.MessageAgg;
using Infrastructure.Data.Context;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private GenericRepository<Message> messageRepo;
        private GenericRepository<ApplicationUser> userRepo;

        public GenericRepository<Message> MessageRepo
        {
            get
            {

                if (this.messageRepo == null)
                {
                    this.messageRepo = new GenericRepository<Message>(context);
                }
                return messageRepo;
            }
        }

        public GenericRepository<ApplicationUser> UserRepo
        {
            get
            {

                if (this.userRepo == null)
                {
                    this.userRepo = new GenericRepository<ApplicationUser>(context);
                }
                return userRepo;
            }

        }
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
