using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using PaymentGateway.DataModel;
using PaymentGateway.DataModel.DomainModel;
using PaymentGateway.Service.ServiceModel;

namespace PaymentGateway.Service.ServiceInterface
{
    public interface IPaymentService
    {
        Task<IEnumerable<ServiceModel.BankGateway>> BankGateways(bool? isActive);
        Task<ServiceModel.PaymentSessionResponse> CreatePaymentSession(ServiceModel.PaymentSessionRequest createPaymentSessionRequest);
        Task<ServiceModel.PaymentSessionResponse> GetPaymentSession(string token);
        Task PaymentVerification(ServiceModel.PaymentVerification paymentVerification);
        Task<bool> CheckPaymentValidityForVerification(int paymentId);
        Task AddGatewayToken(int paymentId,string status, string token);
        Task UpdatePaymentStatus(string token, string status);
    }
    public class PaymentService:IPaymentService
    {
        private readonly IUnitOfWork _uow;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<ServiceModel.PaymentSessionResponse> CreatePaymentSession(ServiceModel.PaymentSessionRequest createPaymentSessionRequest)
        {
            try
            {
                _uow.PaymentRepository.Add(new Payment()
                {
                    EntryDate = DateTime.Now,
                    InvoiceNumber = createPaymentSessionRequest.InvoiceNumber,
                    RequestId = createPaymentSessionRequest.InvoiceNumber + 800000,
                    Amount = createPaymentSessionRequest.TotalPrice,
                    PaymentToken = createPaymentSessionRequest.PaymentToken,
                    Description = createPaymentSessionRequest.Description,
                    IsActive = true,
                    BankGatewayId = createPaymentSessionRequest.GatewayId
                });
                await _uow.Save();
                return new PaymentSessionResponse
                {
                    EntryDate = DateTime.Now,
                    PaymentToken = createPaymentSessionRequest.PaymentToken,
                    InvoiceNumber = createPaymentSessionRequest.InvoiceNumber,
                    TotalPrice = createPaymentSessionRequest.TotalPrice
                };
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<ServiceModel.PaymentSessionResponse> GetPaymentSession(string token)
        {
            var payment = await _uow.PaymentRepository.SingleOrDefault(k => k.PaymentToken == token);
            if (payment == null) return null;
            return new ServiceModel.PaymentSessionResponse
            {
                PaymentId = payment.Id,
                EntryDate = payment.EntryDate,
                GatewayId = payment.BankGatewayId,
                InvoiceNumber = payment.InvoiceNumber,
                PaymentDate = payment.EntryDate,
                TotalPrice = payment.Amount,
                PaymentToken = token,
                Status = "",
                Description = payment.Description,
                Verification = payment.TransactionVerification == null ? null : new ServiceModel.Verification
                {
                    ReferenceNumber = payment.TransactionVerification.ReferenceNumber,
                    Status = payment.TransactionVerification.OriginalStatusId,
                    VerificationDate = payment.TransactionVerification.EntryDate
                },
                GatewayToken = payment.GatewayToken == null ? null : new ServiceModel.GatewayToken
                {
                    EntryDate = payment.GatewayToken.EntryDate,
                    Status = payment.GatewayToken.OriginalStatusId,
                    Token = payment.GatewayToken.Token
                }
            };
        }

        public async Task PaymentVerification(ServiceModel.PaymentVerification paymentVerification)
        {
           
             _uow.PaymentVerificationRepository.Add(new DataModel.DomainModel.PaymentVerification()
            {
                EntryDate = DateTime.Now,
                OriginalStatusId = paymentVerification.StatusId,
                PaymentId = paymentVerification.PaymentId, 
            });
            await _uow.Save();
        }
        public async Task UpdatePaymentStatus(string token, string status)
        {
            var payment = await _uow.PaymentRepository.SingleOrDefault(k => k.PaymentToken == token);
            payment.OriginalStatusId = status;
            await _uow.Save();
        }

        public async Task<bool> CheckPaymentValidityForVerification(int paymentId)
        {
            var payment = await _uow.PaymentRepository.SingleOrDefault(k => k.Id == paymentId);
            if(DateTime.Now - payment.EntryDate == new TimeSpan(0,20,0)&& payment.TransactionVerification == null)
            {
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<ServiceModel.BankGateway>> BankGateways(bool? isActive)
        {
            var res =  await _uow.BankGatewayRepository.Find(k => k.IsActive==isActive);
            return res.Select(k => new ServiceModel.BankGateway { BankGatewayId = k.Id, Name = k.GatewayName, IsActive=k.IsActive });
        }

        public async Task AddGatewayToken(int paymentId,string status, string token)
        {
            _uow.GatewayTokenRepository.Add(new DataModel.DomainModel.GatewayToken{ OriginalStatusId = status, Token = token, PaymentId = paymentId,EntryDate=DateTime.Now });
            await _uow.Save();
        }


    }
}

