using green_craze_be_v1.Application.Model.Paging;
using System.Text.Json.Serialization;

namespace green_craze_be_v1.Application.Model.Address
{
	public class GetAddressPagingRequest : PagingRequest
	{
		[JsonIgnore]
		public string UserId { get; set; }
	}
}