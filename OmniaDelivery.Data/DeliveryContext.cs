using OmniaDelivery.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Data
{
    public class DeliveryContext: DbContext, IDeliveryContext
    {

        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
        {

        }

        public DbSet<DeliveryHeader> DeliveryHeaders { get; set; }
        public DbSet<FileExtension> FileExtensions { get; set; }

        public DbSet<FlowConfiguration> FlowConfigurations { get; set; }

        public DbSet<FlowValidation> FlowValidations { get; set; }
        public DbSet<FileExtensionValue> FileExtensionValues { get; set; }


        //public DbSet<StagingHeader> StagingHeaders { get; set; }
        //public DbSet<StagingExtension> StagingExtensios { get; set; }


        #region procdeures

        public virtual async Task<int> sp_AddFileInSubfolderToStaging(byte[] file, string filename, string subfolder, int headerid, Guid stream_id, string extension)
        {


            var fileParameter = file != null ?
               new SqlParameter("file", SqlDbType.VarBinary) { Value = file } :
               new SqlParameter("file", SqlDbType.VarBinary) { Value = null } ;

            var filenameParameter = filename != null ?
                new SqlParameter("filename", SqlDbType.NVarChar, 255) { Value = Path.GetFileName(filename) } :
                new SqlParameter("filename", SqlDbType.NVarChar, 255) { Value = string.Empty };


            var subfolderParameter = subfolder != null ?
                new SqlParameter("subfolder", SqlDbType.NVarChar, 255) { Value = subfolder } :
                new SqlParameter("subfolder", SqlDbType.NVarChar, 255) { Value = string.Empty };

            var streamidParameter = new SqlParameter("streamid", SqlDbType.UniqueIdentifier) { Value = stream_id };

            var isdeletedParameter = new SqlParameter("isdeleted", SqlDbType.Bit) { Value = false };
            var extensieParameter = new SqlParameter("extensie", SqlDbType.NVarChar, 5) { Value = ".zip" };
            var headerIdParameter = new SqlParameter("headerid", SqlDbType.Int) { Value = headerid };



            try
            {
                return await this.Database.ExecuteSqlCommandAsync("EXEC [delivery].[AddFileInSubfolderToStaging] @file, @filename, @subfolder, @headerid, @streamid, @isdeleted, @extensie", new SqlParameter[] { fileParameter, filenameParameter, subfolderParameter, headerIdParameter, streamidParameter, isdeletedParameter, extensieParameter });
            }
            catch(Exception ex)
            {
                return await Task.FromResult<int>(0);
            }

        }



        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryHeader>()
                .HasKey(contract => new { contract.DeliveryId });

            modelBuilder.Entity<FileExtension>()
                .HasKey(contract => new { contract.FileExtensionId });

            modelBuilder.Entity<FileExtension>().HasOne(d => d.DeliveryHeader).WithMany(p => p.FileExtensions).HasForeignKey(d => d.DeliveryId);


            modelBuilder.Entity<DeliveryHeader>().HasOne(d => d.FlowConfiguration).WithMany(p => p.DeliveryHeaders).HasForeignKey(d => d.ConfigurationID);

            modelBuilder.Entity<FlowValidation>().HasOne(d => d.FlowConfiguration).WithMany(p => p.FlowValidations).HasForeignKey(d => d.ConfigurationId);

            modelBuilder.Entity<FileExtensionValue>().HasOne(d => d.FileExtension).WithMany(p => p.FileExtensionValues).HasForeignKey(d => d.FileExtensionId);


        }


    }
}
