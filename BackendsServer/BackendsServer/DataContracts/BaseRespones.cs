using System.Runtime.Serialization;

namespace BackendsServer.Controllers
{
	[DataContract(Namespace = "")]
	public class BaseRespones
	{
		[DataMember(IsRequired = true, EmitDefaultValue = true)]
		public int ErrorId { get; set; }

		[DataMember(IsRequired = true, EmitDefaultValue = true)]
		public string ErrorDesc { get; set; }
	}
}