using System;
using System.Threading.Tasks;
using PaymentGateway.DataModel;
using PaymentGateway.DataModel.DomainModel;
using PaymentGateway.Service.Model;
using System.Linq;
using System.Collections.Generic;

namespace PaymentGateway.Service
{
    public interface IPaymentService
    {
        Task<IEnumerable<Model.BankGateway>> AvailableBankGateways();
        Task<string> CreatePaymentSession(PaymentSessionRequest createPaymentSessionRequest);
        Task<PaymentGateway.Service.Model.PaymentSessionResponse> GetPaymentSession(string token);
        Task PaymentVerification(PaymentGateway.Service.Model.PaymentVerification paymentVerification);
        Task<PaymentInfo> GetPaymentInfo(string paymentToken);
        Task<bool> CheckPaymentValidityForVerification(int paymentId);
        Task AddGatewayToken(int paymentId,string status, string token);
    }
    public class PaymentService:IPaymentService
    {
        private readonly IUnitOfWork _uow;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<string> CreatePaymentSession(PaymentSessionRequest createPaymentSessionRequest)
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
                return createPaymentSessionRequest.PaymentToken;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<PaymentSessionResponse> GetPaymentSession(string token)
        {
            var payment = await _uow.PaymentRepository.SingleOrDefault(k => k.PaymentToken == token);
            if (payment == null) return null;
            return new PaymentSessionResponse
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
                Verification = payment.TransactionVerification == null ? null : new Verification
                {
                    ReferenceNumber = payment.TransactionVerification.ReferenceNumber,
                    Status = payment.TransactionVerification.OriginalStatusId,
                    VerificationDate = payment.TransactionVerification.EntryDate
                },
                GatewayToken = payment.GatewayToken == null ? null : new Model.GatewayToken
                {
                    EntryDate = payment.GatewayToken.EntryDate,
                    Status = payment.GatewayToken.OriginalStatusId,
                    Token = payment.GatewayToken.Token
                }
            };
        }

        //public async Task UpdatePaymentStatus(string token,string status)
        //{
        //    var payment = await _uow.PaymentRepository.SingleOrDefault(k => k.PaymentToken == token);
        //    payment.OriginalStatusId = status;
        //    _uow.PaymentRepository.Update(payment);
        //}
        public async Task<PaymentInfo> GetPaymentInfo(string paymentToken)
        {
            var payment = await  _uow.PaymentRepository.SingleOrDefault(k => k.PaymentToken == paymentToken);
            if (payment == null) return null;
            return new PaymentInfo()
            {
                InvoiceNumber = payment.InvoiceNumber,
                EntryDate = payment.EntryDate,
                Amount = payment.Amount,
                Token = paymentToken,
                RequestId = payment.RequestId
            };
        }

        public async Task PaymentVerification(PaymentGateway.Service.Model.PaymentVerification paymentVerification)
        {
           
             _uow.PaymentVerificationRepository.Add(new DataModel.DomainModel.PaymentVerification()
            {
                EntryDate = DateTime.Now,
                OriginalStatusId = paymentVerification.StatusId,
                PaymentId = paymentVerification.PaymentId, 
            });
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
        public async Task<IEnumerable<Ser.BankGateway>> AvailableBankGateways()
        {
            var res =  await _uow.BankGatewayRepository.Find(k => k.IsActive);
            return res.Select(k => new Model.BankGateway { Id = k.Id, GatewayName = k.GatewayName });
        }

        public async Task AddGatewayToken(int paymentId,string status, string token)
        {
            _uow.GatewayTokenRepository.Add(new DataModel.DomainModel.GatewayToken{ OriginalStatusId = status, Token = token, PaymentId = paymentId,EntryDate=DateTime.Now });
            await _uow.Save();
        }
    }
}

