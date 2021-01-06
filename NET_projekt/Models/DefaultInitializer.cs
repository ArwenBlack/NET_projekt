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
        Dataset Dataset1, Dataset2, Dataset3;
        protected override void Seed(DefaultContext context)
        {
            TestUser1 = new User
            {
                Nickname = "Janusz12345",
                EmailAddress = "januszex@wp.pl",
                Password = "VuZ8xBCb6/nnGG6+T2shTq/0Pnk+0PSV7VQQP5ij6so=",
                Salt = "otC9cez7YuqXPynsngnhfJcynN2PPu2YWPxnXX/VBeA=",
                PremiumStatus = true
            };
            TestUser2 = new User
            {
                Nickname = "Magik90",
                EmailAddress = "maggik012@wp.pl",
                Password = "VuZ8xBCb6/nnGG6+T2shTq/0Pnk+0PSV7VQQP5ij6so=",
                Salt = "otC9cez7YuqXPynsngnhfJcynN2PPu2YWPxnXX/VBeA=",
                PremiumStatus = true
            };
            Dataset1 = new Dataset
            {
                DatasetName = "Samo ECG",
                DatasetColumnsInfo = "ECG:1,2,3,4",
                DatasetHzFrequency = 10,
                Reference = @"D:\ecg_emg_10Hz_30min.csv",
                ConcreteUser = TestUser1
            };
            Dataset2 = new Dataset
            {
                DatasetName = "Samo EMG",
                DatasetColumnsInfo = "EMG:5,6,7,8",
                DatasetHzFrequency = 10,
                Reference = @"D:\ecg_emg_10Hz_2h.csv",
                ConcreteUser = TestUser1
            };
            Dataset3 = new Dataset
            {
                DatasetName = "ECG EMG wybrane kolumny",
                DatasetColumnsInfo = "ECG:1,3 EMG:5,8",
                DatasetHzFrequency = 100,
                Reference = @"D:\ecg_emg_100Hz_30min.csv",
                ConcreteUser = TestUser2
            };
            context.Users.Add(TestUser1);
            context.Users.Add(TestUser2);
            context.Datasets.Add(Dataset1);
            context.Datasets.Add(Dataset2);
            context.Datasets.Add(Dataset3);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}