using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.DataModel.DomainModel;

namespace PaymentGateway.DataModel
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<GatewayToken> GatewayTokenRepository { get; }
        IRepository<Payment> PaymentRepository { get; }
        IRepository<BankGateway> BankGatewayRepository { get; }
        IPaymentVerificationRepository PaymentVerificationRepository { get; }
        Task Save();
        Task RollBack();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IRepository<Payment> _paymentRepository;
        private IPaymentVerificationRepository _paymentVerificationRepository;
        private IRepository<BankGateway> _bankGatewayRepository;
        private IRepository<GatewayToken> _gatewayTokenRepository;
        public UnitOfWork(PaymentGatewayContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }

        public IRepository<Payment> PaymentRepository => _paymentRepository ?? (_paymentRepository = new Repository<Payment>(_context));

        public IPaymentVerificationRepository PaymentVerificationRepository => _paymentVerificationRepository ?? (_paymentVerificationRepository = new PaymentVerificationRepository(_context));

        public IRepository<BankGateway> BankGatewayRepository => _bankGatewayRepository ?? (_bankGatewayRepository = new Repository<BankGateway>(_context));

        public IRepository<GatewayToken> GatewayTokenRepository => _gatewayTokenRepository ?? (_gatewayTokenRepository = new Repository<GatewayToken>(_context));

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RollBack()
        {
            foreach (var entityEntry in _context
                .ChangeTracker
                .Entries())
            {
                await entityEntry.ReloadAsync();
            }
        }
    }
}
