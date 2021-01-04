using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
	[DataContract]
	public class DataPoint
	{
		public DataPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		
		[DataMember(Name = "x")]
		public Nullable<double> X = null;

	
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;
	}
}