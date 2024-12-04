using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Parser
{
    class Program
    {

        //static string Tags = System.Configuration.ConfigurationManager.AppSettings["Tags"].ToString();
        //static string TagSplitter = System.Configuration.ConfigurationManager.AppSettings["TagSplitter"].ToString();
        //static string ValueSplitter = System.Configuration.ConfigurationManager.AppSettings["ValueSplitter"].ToString();

        static void Main(string[] args)
        {
            // List of SMS to be recieved from Service of Service Provider
            //List<string> lstSMS = new List<string> { 
            //    "TG1=50 TG2=40 TG3=30 TG4=20 TG5=10", 
            //    "TG1=51 TG2=41 TG3=31 TG4=21 TG5=11", 
            //    "TG1=15 TG2=14 TG3=13 TG4=12 TG5=11" 
            //};


            //GetAllSMSTagValues(lstSMS);

            //RSWS.CatalogItem c = new RSWS.CatalogItem();
            
            
        }

        //static public List<TagValue> GetAllSMSTagValues(List<string> lstSMS)
        //{
    //        List<string> lstTags = Tags.Split(",".ToCharArray()).ToList<string>();

    //        List<TagValue> lstTagValues = new List<TagValue>();

    //        foreach (string sSMS in lstSMS)
    //        {
    //            //List<string> lstTagValue = sSMS.Split(TagSplitter.ToCharArray()).ToList<string>();

    //            //foreach (string sTagValue in lstTagValue)
    //            //{
    //            //    string[] sTG = sTagValue.Split(ValueSplitter.ToCharArray()).ToArray();

    //            //    TagValue tg = new TagValue();
    //            //    tg.Tag = sTG[0];
    //            //    tg.Value = sTG[1];

    //            //    lstTagValues.Add(tg);
    //            //}

    //            lstTagValues = GetSMSTagValues(sSMS);

    //        }

    //        return lstTagValues;
    //    }

    //    static public List<TagValue> GetSMSTagValues(string sSMS)
    //    {
    //        List<TagValue> lstTagValues = new List<TagValue>();
    //        List<string> lstTagValue = sSMS.Split(TagSplitter.ToCharArray()).ToList<string>();

    //        foreach (string sTagValue in lstTagValue)
    //        {
    //            string[] sTG = sTagValue.Split(ValueSplitter.ToCharArray()).ToArray();

    //            TagValue tg = new TagValue();
    //            tg.Tag = sTG[0];
    //            tg.Value = sTG[1];

    //            lstTagValues.Add(tg);
    //        }

    //        return lstTagValues;
    //    }
    }

    //public class TagValue
    //{
    //    public string Tag;
    //    public string Value;
    //}
}
