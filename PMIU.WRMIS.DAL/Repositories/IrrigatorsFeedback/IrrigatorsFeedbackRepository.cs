using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.IrrigatorsFeedback
{
    public class IrrigatorsFeedbackRepository : Repository<IF_Irrigator>
    {
        WRMIS_Entities _context;

        public IrrigatorsFeedbackRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<IF_Irrigator>();
            _context = context;
        }

        /// <summary>
        /// this function return all channel of specific Division
        /// Created On: 9/5/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>List<dynamic></returns>
        public List<GetTailofChannelinDivision_Result> GetChannelByDivisionID(long _DivisionID)
        {
            //List<GetTailofChannelinDivision_Result> lstChannels = context.GetTailofChannelinDivision(_DivisionID).Where(x => x.TailOfftake == null).OrderBy(z => z.ChannelName).ToList<GetTailofChannelinDivision_Result>();
            List<GetTailofChannelinDivision_Result> lstChannels = context.GetTailofChannelinDivision(_DivisionID).OrderBy(z => z.ChannelName).ToList<GetTailofChannelinDivision_Result>();
            return lstChannels;
        }
    }
}
