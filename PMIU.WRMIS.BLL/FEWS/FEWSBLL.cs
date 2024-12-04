using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.Repositories;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.BLL.FEWS
{
    public class FEWSBLL : BaseBLL
    {
        public FW_Ref_Classifications GetFlowStatus(string _River, string _Station)
        {
            return this.db.Repository<FW_Ref_Classifications>().Query().Get().Where(x => x.River.ToUpper() == _River.ToUpper() && x.Station.ToUpper() == _Station.ToUpper()).ToList().FirstOrDefault();
        }
    }
}
