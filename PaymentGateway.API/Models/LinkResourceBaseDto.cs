using System.Collections.Generic;

namespace PaymentGateway.API.Models
{
    public class LinkResourceBaseDto
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
