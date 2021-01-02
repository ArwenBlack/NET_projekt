using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class DefaultInitializer : DropCreateDatabaseAlways<DefaultContext>
    {
        User TestUser1, TestUser2;
        EcgDataset Dataset1, Dataset2, Dataset3;
        EmgDataset Dataset4, Dataset5, Dataset6;
        protected override void Seed(DefaultContext context)
        {
            TestUser1 = new User
            {
                Nickname = "Janusz12345",
                EmailAddress = "januszex@wp.pl",
                Password = "VuZ8xBCb6/nnGG6+T2shTq/0Pnk+0PSV7VQQP5ij6so=", //hej  sprawdzone - dziala
                Salt = "otC9cez7YuqXPynsngnhfJcynN2PPu2YWPxnXX/VBeA=",
                PremiumStatus = true
            };
            TestUser2 = new User
            {
                Nickname = "Magik90",
                EmailAddress = "maggik012@wp.pl",
                Password = "541c57960bb997942655d14e3b9607f9", //hej
                Salt = "dgheyriwe",
                PremiumStatus = true
            };
            Dataset1 = new EcgDataset
            {
                ConcreteUser = TestUser1,
                DatasetName = "t1",
                GoogleReference = "refDELETEME"
            };
            Dataset2 = new EcgDataset
            {
                ConcreteUser = TestUser1,
                DatasetName = "t2",
                GoogleReference = "refDELETEME"
            };
            Dataset3 = new EcgDataset
            {
                ConcreteUser = TestUser2,
                DatasetName = "t3",
                GoogleReference = "refDELETEME"
            };
            Dataset4 = new EmgDataset
            {
                ConcreteUser = TestUser1,
                DatasetName = "EMGT1-U1",
                GoogleReference = "refDELETEME"
            };
            Dataset5 = new EmgDataset
            {
                ConcreteUser = TestUser2,
                DatasetName = "EMGT2-U2",
                GoogleReference = "refDELETEME"
            };
            Dataset6 = new EmgDataset
            {
                ConcreteUser = TestUser2,
                DatasetName = "EMGT3-U2",
                GoogleReference = "refDELETEME"
            };
            context.Users.Add(TestUser1);
            context.Users.Add(TestUser2);
            context.EcgDatasets.Add(Dataset1);
            context.EcgDatasets.Add(Dataset2);
            context.EcgDatasets.Add(Dataset3);
            context.EmgDatasets.Add(Dataset4);
            context.EmgDatasets.Add(Dataset5);
            context.EmgDatasets.Add(Dataset6);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}