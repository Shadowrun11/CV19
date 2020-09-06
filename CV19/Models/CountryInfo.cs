using System.Collections.Generic;
using System.Text;

namespace CV19.Models
{

    internal class CounryInfo : PlaceInfo
    {
        public IEnumerable<PlaceInfo> ProvinceCounts { get; set; }
    }

}
