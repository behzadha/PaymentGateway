using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PaymentGateway.DataModel.DomainModel;

namespace PaymentGateway.DataModel.Migrations
{
    [DbContext(typeof(PaymentGatewayContext))]
    [Migration("20170409151942_PaymentGateway")]
    partial class PaymentGateway
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PaymentGateway.DataModel.DomainModel.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("EntryDate");

                    b.Property<string>("GatewayName");

                    b.Property<long>("InvoiceNumber");

                    b.Property<string>("PaymentToken")
                        .IsRequired();

                    b.Property<string>("ReferenceNumber");

                    b.Property<long>("RequestId");

                    b.HasKey("Id");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("PaymentGateway.DataModel.DomainModel.PaymentVerification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EntryDate");

                    b.Property<bool>("IsVerified");

                    b.Property<int>("PaymentId");

                    b.Property<int>("StatusId");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("PaymentVerification");
                });

            modelBuilder.Entity("PaymentGateway.DataModel.DomainModel.PaymentVerification", b =>
                {
                    b.HasOne("PaymentGateway.DataModel.DomainModel.Payment", "Payment")
                        .WithOne("TransactionVerifications")
                        .HasForeignKey("PaymentGateway.DataModel.DomainModel.PaymentVerification", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
