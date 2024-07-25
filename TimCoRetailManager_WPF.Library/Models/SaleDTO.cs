using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library.Models
{
    public class SaleDTO
    {
        public List<SaleDetailDTO> SaleDetails { get; set; } = new List<SaleDetailDTO>();
    }
}
