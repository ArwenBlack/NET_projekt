using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class DefaultInitializer : DropCreateDatabaseIfModelChanges<DefaultContext>
    {
        User TestUser1, TestUser2;
        EcgDataset Dataset1, Dataset2, Dataset3;
        EmgDataset Dataset4, Dataset5, Dataset6;
        EcgDataPoint EcgPoint1, EcgPoint2;
        protected override void Seed(DefaultContext context)
        {
            var tableNames = context.Database.SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME NOT LIKE '%Migration%'").ToList();
            foreach (var tableName in tableNames)
            {
                context.Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }
            context.SaveChanges();
            TestUser1 = new User
            {
                Nickname = "Janusz123",
                EmailAddress = "januszex@wp.pl",
                PremiumStatus = true
            };
            TestUser2 = new User
            {
                Nickname = "Magik90",
                EmailAddress = "maggik012@wp.pl",
                PremiumStatus = true
            };
            Dataset1 = new EcgDataset
            {
                ConcreteUser = TestUser1,
                DatasetName = "t1"
            };
            Dataset2 = new EcgDataset
            {
                ConcreteUser = TestUser1,
                DatasetName = "t2"
            };
            Dataset3 = new EcgDataset
            {
                ConcreteUser = TestUser2,
                DatasetName = "t3"
            };
            Dataset4 = new EmgDataset
            {
                ConcreteUser = TestUser1,
                DatasetName = "EMGT1-U1"
            };
            Dataset5 = new EmgDataset
            {
                ConcreteUser = TestUser2,
                DatasetName = "EMGT2-U2"
            };
            Dataset6 = new EmgDataset
            {
                ConcreteUser = TestUser2,
                DatasetName = "EMGT3-U2"
            };
            EcgPoint1 = new EcgDataPoint
            {
                ConcreteDataset = Dataset1,
                Point = 0.0123
            };
            EcgPoint2 = new EcgDataPoint
            {
                ConcreteDataset = Dataset1,
                Point = 0.0129
            };
            context.Users.Add(TestUser1);
            context.Users.Add(TestUser2);
            context.EcgDatasets.Add(Dataset1);
            context.EcgDatasets.Add(Dataset2);
            context.EcgDatasets.Add(Dataset3);
            context.EmgDatasets.Add(Dataset4);
            context.EmgDatasets.Add(Dataset5);
            context.EmgDatasets.Add(Dataset6);
            context.EcgDataPoints.Add(EcgPoint1);
            context.EcgDataPoints.Add(EcgPoint2);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}