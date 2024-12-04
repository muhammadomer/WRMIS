using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.Auctions
{
    public class AuctionRepository : Repository<AC_AuctionNotice>
    {
         WRMIS_Entities _context;

         public AuctionRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<AC_AuctionNotice>();
            _context = context;
        }

        #region "Auction Notice"
         public bool DeletePublishingSourceforUpdation(long _AuctionNoticeID)
         {
             bool IsDeleted = false;
             try
             {
                 context.AC_AdvertisementSource.RemoveRange(context.AC_AdvertisementSource.Where(q => q.AcutionNoticeID == _AuctionNoticeID));
                 context.SaveChanges();
                 IsDeleted = true;
             }
             catch (Exception)
             {

                 throw;
             }
             return IsDeleted;
         }

         public List<dynamic> GetPublishingSourceByAuctionNoticeID(long _AuctionNoticeID)
         {
             List<dynamic> Sources = (from D in context.AC_AdvertisementSource
                                      where D.AcutionNoticeID == _AuctionNoticeID
                                      select new
                                      {
                                          ID = D.ID,
                                          AdvertisementSource = D.AdvertisementSource,
                                          AdvertisementDate = D.AdvertisementDate

                                      }).ToList<dynamic>();

             return Sources;
         }

        public List<dynamic> GetBiddersByAuctionNoticeID(long _AuctionNoticeID)
         {
             List<dynamic> Sources = (from D in context.AC_AuctionBidder
                                      where D.AuctionNoticeID == _AuctionNoticeID
                                      select new
                                      {
                                          ID = D.ID,
                                          Name = D.BidderName
                                          
                                      }).ToList<dynamic>();

             return Sources;
         }

        public List<dynamic> GetAssetAttributesBySubCategoryID(long _AssetItemID)
        {
            List<dynamic> Sources = (from D in context.AM_AssetAttributes
                                     join C in context.AM_Attributes on D.AttributeID equals C.ID
                                     where D.AssetItemID == _AssetItemID
                                     select new
                                     {
                                         ID = D.ID,
                                         Name = C.AttributeName

                                     }).ToList<dynamic>();

            return Sources;
        }

        public List<dynamic> GetAuctionAssetsByNoticeID(long _AuctionNoticeID)
        {
            List<dynamic> Sources = (from Assets in context.AC_AuctionAssets
                                     join AI in context.AC_AuctionAssetItems on new { ID = Assets.ID, Value = Assets.GroupIndividual } equals new { ID = AI.AuctionAssetID, Value = "Individual" } into AI1
                                     from AI2 in AI1.DefaultIfEmpty()
                                     join Att in context.AM_AssetAttributes on AI2.AssetAttributeID equals Att.ID into Att1
                                     from Att2 in Att1.DefaultIfEmpty()
                                     join AMAtt in context.AM_Attributes on Att2.AttributeID equals AMAtt.ID into AMAtt1
                                     from AMAtt2 in AMAtt1.DefaultIfEmpty()
                                     join IR in context.UA_IrrigationLevel on Assets.IrrigationLevelID equals IR.ID into IR1
                                     from IR2 in IR1.DefaultIfEmpty()
                                     join Cat in context.AM_Category on Assets.AssetCategoryID equals Cat.ID
                                     join SCat in context.AM_SubCategory on Assets.AssetSubCategoryID equals SCat.ID
                                     join AM in context.AM_AssetItems on AI2.AssetItemID equals AM.ID into AM1
                                     from AM2 in AM1.DefaultIfEmpty()
                                     where Assets.AuctionNoticeID == _AuctionNoticeID
                                     select new
                                     {
                                         AuctionAssetID = Assets.ID,
                                         Level = IR2.Name == null ? "Division" : IR2.Name,
                                         LevelID = Assets.IrrigationLevelID,
                                         GroupIndividual = Assets.GroupIndividual,
                                         Category = Cat.Name,
                                         CategoryID = Assets.AssetCategoryID,
                                         SubCategory = SCat.Name,
                                         SubCategoryID = Assets.AssetSubCategoryID,
                                         Name = AM2 == null ? Assets.GroupName : AM2.AssetName,
                                         NameID = (long?)AI2.AssetItemID,
                                         AttributeType = (AMAtt2.AttributeName == null && AM2.AssetName != null && AI2.AssetQuantity != null) ? "Quantity" : AMAtt2.AttributeName,
                                         AttributeTypeID = (long?)AI2.AssetAttributeID,
                                         AttributeValue = Att2 == null ? SqlFunctions.StringConvert((double)AI2.AssetQuantity).Trim() : Att2.AttributeValue,
                                         CreatedBy = Assets.CreatedBy,
                                         CreatedDate = Assets.CreatedDate,
                                         AssetQuantity =  AI2.AssetQuantity
                                        }).ToList<dynamic>();

            return Sources;
        }

        public List<dynamic> GetAuctionAssetItemsByID(long _AuctionAssetID)
        {
            List<dynamic> Assets = (from AUC in context.AC_AuctionAssetItems
                                     join AI in context.AM_AssetItems on AUC.AssetItemID equals AI.ID
                                     join Att in context.AM_AssetAttributes on AUC.AssetAttributeID equals Att.ID into Att1
                                     from Att2 in Att1.DefaultIfEmpty()
                                     join AsseAtt in context.AM_Attributes on Att2.AttributeID equals AsseAtt.ID into AsseAtt1
                                     from AsseAtt2 in AsseAtt1.DefaultIfEmpty()
                                     where AUC.AuctionAssetID == _AuctionAssetID
                                     select new
                                     {
                                         AuctionAssetItemID = AUC.ID,
                                         AuctionAssetID = AUC.AuctionAssetID,
                                         AssetItemID = AUC.AssetItemID,
                                         Name = AI.AssetName,
                                         AttributeType = AsseAtt2 == null && AUC.AssetQuantity != null ? "Quantity" : AsseAtt2.AttributeName,
                                         AttributeTypeID = AUC.AssetAttributeID,
                                         AttributeValue = Att2 == null ? SqlFunctions.StringConvert((double)AUC.AssetQuantity).Trim() : Att2.AttributeValue,
                                         LotQuantity = AUC.AssetQuantity,
                                         CreatedBy = AUC.CreatedBy,
                                         CreatedDate = AUC.CreatedDate

                                     }).ToList<dynamic>();

            return Assets;
        }
        public bool DeleteAuctionAssetItemByAuctionAssetID(long _AuctionAssetID)
        {
            bool IsDeleted = false;
            try
            {
                context.AC_AuctionAssetItems.RemoveRange(context.AC_AuctionAssetItems.Where(q => q.AuctionAssetID == _AuctionAssetID));
                context.SaveChanges();
                IsDeleted = true;
            }
            catch (Exception)
            {

                throw;
            }
            return IsDeleted;
        }

        public dynamic GetAuctionAssetDetails(long _AuctionNoticeID, long _AuctionAssetID)
        {
            dynamic Details = (from AN in context.AC_AuctionNotice
                               join AI in context.AC_AuctionAssets on AN.ID equals AI.AuctionNoticeID
                               join AAI in context.AC_AuctionAssetItems on AI.ID equals AAI.AuctionAssetID into AAI1
                               from AAI2 in AAI1.DefaultIfEmpty()
                               join AMAI in context.AM_AssetItems on AAI2.AssetItemID equals AMAI.ID into AMAI1
                               from AMAI2 in AMAI1.DefaultIfEmpty()
                               join AIC in context.AM_Category on AI.AssetCategoryID equals AIC.ID
                               join AIS in context.AM_SubCategory on AI.AssetSubCategoryID equals AIS.ID
                               where AN.ID == _AuctionNoticeID && AI.ID == _AuctionAssetID

                               select new
                               {
                                   NoticeName = AN.AuctionTitle,
                                   OpeningDate = AN.OpeningDate,
                                   SubmissionDate = AN.SubmissionDate,
                                   Category = AIC.Name,
                                   SubCategory = AIS.Name,
                                   AssetName = AI.GroupIndividual == "Group" ? AI.GroupName : AMAI2.AssetName

                               }).FirstOrDefault();

            return Details;
        }

        //public List<dynamic> GetAssetsforBidderEarnestMoney(long _AuctionNoticeID, long _BidderID)
        //{
        //    List<dynamic> Assets = (from AAD in context.AC_AuctionAssetDetail
        //                            join AC in context.AC_AuctionAssets on AAD.AuctionAssetID equals AC.ID
        //                            join AI in context.AC_AuctionAssetItems on AC.ID equals AI.AuctionAssetID into AI1
        //                            from AI2 in AI1.DefaultIfEmpty()
        //                            join AAI in context.AM_AssetItems on AI2.AssetItemID equals AAI.ID into AAI1
        //                            from AAI2 in AAI1.DefaultIfEmpty()
        //                            join Bid in context.AC_AuctionBidder on AC.AuctionNoticeID equals Bid.AuctionNoticeID into Bid1
        //                            from Bid2 in Bid1.DefaultIfEmpty()
        //                            join BEM in context.AC_BidderEarnestMoney on Bid2.ID equals BEM.AuctionAssetDetailID into BEM1
        //                            from BEM2 in BEM1.DefaultIfEmpty()

        //                            where AC.AuctionNoticeID == _AuctionNoticeID
        //                            select new
        //                            {
        //                                ID = (long?)BEM2.ID,
        //                                AuctionDetailID = AAD.ID,
        //                                AssetName = AI2 == null ? AC.GroupName : AAI2.AssetName,
        //                                EarnestMoney = AAD.EarnestMoney,
        //                                isChecked = BEM2 == null ? false : true,
        //                                EarnestMoneySubmitted = BEM2 == null ? "" : SqlFunctions.StringConvert(BEM2.EarnestMoneySubmitted).Trim()
                                     
        //                            }).Distinct().ToList<dynamic>();

        //    return Assets;
        //}

        public List<dynamic> GetAssetsforBidderEarnestMoney(long _AuctionNoticeID, long _BidderID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    from AI2 in AI1.DefaultIfEmpty()
                                    join AAI in context.AM_AssetItems on AI2.AssetItemID equals AAI.ID into AAI1
                                    from AAI2 in AAI1.DefaultIfEmpty()
                                    join ADet in context.AC_AuctionAssetDetail on AucAssets.ID equals ADet.AuctionAssetID
                                    join Bid in context.AC_AuctionBidder on AucAssets.AuctionNoticeID equals Bid.AuctionNoticeID into Bid1
                                    from Bid2 in Bid1.DefaultIfEmpty()
                                    join BEM in context.AC_BidderEarnestMoney on new { DetIID = ADet.ID, BidID = Bid2.ID } equals new { DetIID = BEM.AuctionAssetDetailID, BidID = BEM.AuctionBidderID } into BEM1
                                    from BEM2 in BEM1.DefaultIfEmpty()

                                    where AucAssets.AuctionNoticeID == _AuctionNoticeID && Bid2.ID == _BidderID
                                    select new
                                    {
                                        ID = (long?)BEM2.ID,
                                        AuctionDetailID = ADet.ID,
                                        AssetName = AucAssets.GroupIndividual == "Group" ? AucAssets.GroupName : AAI2.AssetName,
                                        EarnestMoney = ADet.EarnestMoneyType == "LumpSum" ? ADet.EarnestMoney : (ADet.ReservePrice * (ADet.EarnestMoney / 100)),//ADet.EarnestMoney,
                                        isChecked = BEM2 == null ? false : true,
                                        EarnestMoneySubmitted = BEM2 == null ? "" : SqlFunctions.StringConvert(BEM2.EarnestMoneySubmitted).Trim(),
                                        Attachement = BEM2.EMAttachment

                                    }).Distinct().ToList<dynamic>();

            return Assets;
        }
        public bool DeleteCommitteMembersforUpdation(long _AuctionNoticeID)
        {
            bool IsDeleted = false;
            try
            {
                context.AC_AuctionCommiteeMembers.RemoveRange(context.AC_AuctionCommiteeMembers.Where(q => q.AuctionNoticeID == _AuctionNoticeID));
                context.SaveChanges();
                IsDeleted = true;
            }
            catch (Exception)
            {

                throw;
            }
            return IsDeleted;
        }
        public List<dynamic> GetAssetsByNoticeID(long _AuctionNoticeID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    from AI2 in AI1.DefaultIfEmpty()
                                    join AAI in context.AM_AssetItems on AI2.AssetItemID equals AAI.ID into AAI1
                                    from AAI2 in AAI1.DefaultIfEmpty()
                           
                                    where AucAssets.AuctionNoticeID == _AuctionNoticeID
                                    select new
                                    {
                                        ID = AucAssets.ID,
                                        Name = AucAssets.GroupIndividual == "Group" ? AucAssets.GroupName : AAI2.AssetName,
                                   
                                    }).Distinct().ToList<dynamic>();

            return Assets;
        }
        public List<dynamic> GetBiddersByNoticeAndAssetID(long _AuctionNoticeID, long _AuctionAssetID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AucDet in context.AC_AuctionAssetDetail on AucAssets.ID equals AucDet.AuctionAssetID
                                    //join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    //from AI2 in AI1.DefaultIfEmpty()
                                    join BEM in context.AC_BidderEarnestMoney on AucDet.ID equals BEM.AuctionAssetDetailID into BEM1
                                    from BEM2 in BEM1.DefaultIfEmpty()
                                    join Bid in context.AC_AuctionBidder on BEM2.AuctionBidderID equals Bid.ID into Bid1
                                    from Bid2 in Bid1.DefaultIfEmpty()

                                    where Bid2.AuctionNoticeID == _AuctionNoticeID && AucAssets.ID == _AuctionAssetID
                                    select new
                                    {
                                        ID = BEM2.ID,
                                        Bidder = Bid2.BidderName,
                                        isAttended = BEM2.Attended == null ? false : true,
                                        AlternateName = BEM2.BidderRepresentative,
                                        AlternateRemarks = BEM2.Remarks,

                                    }).Distinct().ToList<dynamic>();

            return Assets;
        }
        public List<dynamic> GetBiddersByNoticeAndAssetIDForViewMode(long _AuctionNoticeID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AucDet in context.AC_AuctionAssetDetail on AucAssets.ID equals AucDet.AuctionAssetID
                                    //join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    //from AI2 in AI1.DefaultIfEmpty()
                                    join BEM in context.AC_BidderEarnestMoney on AucDet.ID equals BEM.AuctionAssetDetailID into BEM1
                                    from BEM2 in BEM1.DefaultIfEmpty()
                                    join Bid in context.AC_AuctionBidder on BEM2.AuctionBidderID equals Bid.ID into Bid1
                                    from Bid2 in Bid1.DefaultIfEmpty()

                                    where AucAssets.AuctionNoticeID == _AuctionNoticeID
                                    select new
                                    {
                                        ID = BEM2.ID,
                                        Bidder = Bid2.BidderName,
                                        isAttended = BEM2.Attended == null ? false : true,
                                        AlternateName = BEM2.BidderRepresentative,
                                        AlternateRemarks = BEM2.Remarks,

                                    }).Distinct().ToList<dynamic>();

            return Assets;
        }

        public List<dynamic> GetBiddersByNoticeAndAssetIDForBiddingProcess(long _AuctionNoticeID, long _AuctionAssetID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AucDet in context.AC_AuctionAssetDetail on AucAssets.ID equals AucDet.AuctionAssetID
                                    join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    from AI2 in AI1.DefaultIfEmpty()
                                    join BEM in context.AC_BidderEarnestMoney on AucDet.ID equals BEM.AuctionAssetDetailID into BEM1
                                    from BEM2 in BEM1.DefaultIfEmpty()
                                    join Bid in context.AC_AuctionBidder on BEM2.AuctionBidderID equals Bid.ID into Bid1
                                    from Bid2 in Bid1.DefaultIfEmpty()
                                    join Price in context.AC_AuctionPrice on new { DetIID = AucDet.ID, BidID = Bid2.ID } equals new { DetIID = Price.AuctionAssetDetailID, BidID = Price.AuctionBidderID.Value } into Price1
                                    from Price2 in Price1.DefaultIfEmpty()

                                    where AucAssets.AuctionNoticeID == _AuctionNoticeID && AucAssets.ID == _AuctionAssetID && BEM2.Attended == true
                                    select new
                                    {
                                        ID = (long?)Price2.ID,
                                        isChecked = Price2.BidderRate == null ? false : true,
                                        BidderDetail = Bid2.BidderName,
                                        BidderRep = BEM2.BidderRepresentative,
                                        BidderRate = (double?)Price2.BidderRate,
                                        BidderID= Bid2.ID,
                                        AuctionAssetDetailID = AucDet.ID
                                    }).Distinct().ToList<dynamic>();

            //Assets = Assets.Where(x => x.ID != null).ToList<dynamic>();
            return Assets;
        }

        public List<dynamic> GetBiddersByNoticeAndAssetIDForBidderSelection(long _AuctionNoticeID, long _AuctionAssetID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AucDet in context.AC_AuctionAssetDetail on AucAssets.ID equals AucDet.AuctionAssetID
                                    join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    from AI2 in AI1.DefaultIfEmpty()
                                    join BEM in context.AC_BidderEarnestMoney on AucDet.ID equals BEM.AuctionAssetDetailID into BEM1
                                    from BEM2 in BEM1.DefaultIfEmpty()
                                    join Bid in context.AC_AuctionBidder on BEM2.AuctionBidderID equals Bid.ID into Bid1
                                    from Bid2 in Bid1.DefaultIfEmpty()
                                    join Price in context.AC_AuctionPrice on new { DetIID = AucDet.ID, BidID = Bid2.ID } equals new { DetIID = Price.AuctionAssetDetailID, BidID = Price.AuctionBidderID.Value } into Price1
                                    from Price2 in Price1.DefaultIfEmpty()

                                    where AucAssets.AuctionNoticeID == _AuctionNoticeID && AucAssets.ID == _AuctionAssetID && BEM2.Attended == true && Price2 != null
                                    select new
                                    {
                                        ID = (long?)Price2.ID,
                                        isChecked = Price2.Awarded == null ? false : Price2.Awarded,
                                        Contractor = Bid2.BidderName,
                                        BidderRep = BEM2.BidderRepresentative,
                                        BidderRate = (double?)Price2.BidderRate,
                                        BidderID = Bid2.ID,
                                        AuctionAssetDetailID = AucDet.ID,
                                        IsAwarded = Price2.Awarded,//context.AC_AuctionPrice.Where(x=>x.AuctionAssetDetailID == AucDet.ID).Any(x=>x.Awarded == true),
                                        AwardedReason = Price2.AwardedRemarks
                                    }).Distinct().OrderByDescending(x=>x.BidderRate).ToList<dynamic>();
            
            return Assets;
        }

        public List<dynamic> GetAuctionItemsForPayments(long _AuctionNoticeID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AucDet in context.AC_AuctionAssetDetail on AucAssets.ID equals AucDet.AuctionAssetID
                                    join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    from AI2 in AI1.DefaultIfEmpty()
                                    join AAI in context.AM_AssetItems on AI2.AssetItemID equals AAI.ID into AAI1
                                    from AAI2 in AAI1.DefaultIfEmpty()
                                    join BEM in context.AC_BidderEarnestMoney on AucDet.ID equals BEM.AuctionAssetDetailID into BEM1
                                    from BEM2 in BEM1.DefaultIfEmpty()
                                    join Bid in context.AC_AuctionBidder on BEM2.AuctionBidderID equals Bid.ID into Bid1
                                    from Bid2 in Bid1.DefaultIfEmpty()
                                    join Price in context.AC_AuctionPrice on new { DetIID = AucDet.ID, BidID = Bid2.ID } equals new { DetIID = Price.AuctionAssetDetailID, BidID = Price.AuctionBidderID.Value } into Price1
                                    from Price2 in Price1.DefaultIfEmpty()
                                    //join Payment in context.AC_AuctionPayment on new { NotID = AucAssets.AuctionNoticeID, PriceID = Price2.ID , PaymentType = "TokenMoney" } equals new { NotID = Payment.AuctionNoticeID, PriceID = Payment.AuctionPriceID, PaymentType = Payment.PaymentType } into Payment1
                                    //from Payment2 in Payment1.DefaultIfEmpty()

                                    where AucAssets.AuctionNoticeID == _AuctionNoticeID && BEM2.Attended == true && Price2.Awarded == true
                                    select new
                                    {
                                        ID = (long?)Price2.ID,
                                        AssetName = AucAssets.GroupIndividual == "Group" ? AucAssets.GroupName : AAI2.AssetName,
                                        SelectedBidder = Bid2.BidderName,
                                        BidderRep = BEM2.BidderRepresentative,
                                        BidderRate = (double?)Price2.BidderRate,
                                        BidderID = Bid2.ID,
                                        AuctionAssetDetailID = AucDet.ID,
                                        EarnestMoney = AucDet.EarnestMoneyType == "LumpSum" ? AucDet.EarnestMoney : (AucDet.ReservePrice * (AucDet.EarnestMoney/100)),
                                        EarnestMoneyType = AucDet.EarnestMoneyType,
                                        TokenMoney = AucDet.TokenMoneyType == "LumpSum" ? AucDet.TokenMoney : (Price2.BidderRate * (AucDet.TokenMoney / 100)),
                                        TokenMoneyType = AucDet.TokenMoneyType,
                                        EarnestMoneySubmitted = BEM2.EarnestMoneySubmitted,
                                        TokenMoneySubmitted = (context.AC_AuctionPayment.Where(x => x.AuctionNoticeID == _AuctionNoticeID && x.AuctionPriceID == Price2.ID && x.PaymentType == "TokenMoney").Sum(x => x.PaidAmount)),//Payment2.ID == null ? 0 : (Payment2.PaymentType == "TokenMoney" ? Payment2.PaidAmount : 0),
                                        BalanceAmount = Price2.BidderRate,
                                        TotalMoneyPaid = Price2.BidderRate,
                                        Status = AucAssets.DeliveryStatus == null ? "Pending for Approval" : AucAssets.DeliveryStatus
                                    }).Distinct().ToList<dynamic>();

            return Assets;
        }
        public dynamic GetAuctionDetails(AC_AuctionPrice _mdlAuctionPrice)
        {
            //dynamic Asset = (from AucAssets in context.AC_AuctionAssets
            //                        join AcNotice in context.AC_AuctionNotice on AucAssets.AuctionNoticeID equals AcNotice.ID
            //                        join Category in context.AM_Category on AucAssets.AssetCategoryID equals Category.ID
            //                        join SCategory in context.AM_SubCategory on AucAssets.AssetSubCategoryID equals SCategory.ID
            //                        join AucDet in context.AC_AuctionAssetDetail on AucAssets.ID equals AucDet.AuctionAssetID
            //                        join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
            //                        from AI2 in AI1.DefaultIfEmpty()
            //                        join AAI in context.AM_AssetItems on AI2.AssetItemID equals AAI.ID into AAI1
            //                        from AAI2 in AAI1.DefaultIfEmpty()
            //                        join BEM in context.AC_BidderEarnestMoney on AucDet.ID equals BEM.AuctionAssetDetailID into BEM1
            //                        from BEM2 in BEM1.DefaultIfEmpty()
            //                        join Bid in context.AC_AuctionBidder on BEM2.AuctionBidderID equals Bid.ID into Bid1
            //                        from Bid2 in Bid1.DefaultIfEmpty()
            //                        join Price in context.AC_AuctionPrice on new { DetIID = AucDet.ID, BidID = Bid2.ID } equals new { DetIID = Price.AuctionAssetDetailID, BidID = Price.AuctionBidderID.Value } into Price1
            //                        from Price2 in Price1.DefaultIfEmpty()
            //                        join Payment in context.AC_AuctionPayment on new { NotID = AucAssets.AuctionNoticeID, PriceID = Price2.ID } equals new { NotID = Payment.AuctionNoticeID, PriceID = Payment.AuctionPriceID } into Payment1
            //                        from Payment2 in Payment1.DefaultIfEmpty()

            //                        where AucAssets.AuctionNoticeID == _AuctionNoticeID && BEM2.Attended == true && Price2.Awarded == true && Bid2.ID == _AuctionBidderID
            //                        select new
            //                        {
            //                            ID = (long?)Price2.ID,
            //                            NoticeName = AcNotice.AuctionTitle,
            //                            OpeningDate = AcNotice.OpeningDate,
            //                            SubmissionDate = AcNotice.SubmissionDate,
            //                            AssetCategory = Category.Name,
            //                            AssetSubCategory = SCategory.Name,
            //                            AssetName = AucAssets.GroupIndividual == "Group" ? AucAssets.GroupName : AAI2.AssetName,
            //                            SelectedBidder = Bid2.BidderName,
            //                            BidderRep = BEM2.BidderRepresentative,
            //                            BidderRate = (double?)Price2.BidderRate,
            //                            BidderID = Bid2.ID,
            //                            AuctionAssetDetailID = AucDet.ID,
            //                            EarnestMoney = AucDet.EarnestMoneyType == "LumpSum" ? AucDet.EarnestMoney : (AucDet.ReservePrice * (AucDet.EarnestMoney / 100)),
            //                            EarnestMoneyType = AucDet.EarnestMoneyType,
            //                            TokenMoney = AucDet.TokenMoneyType == "LumpSum" ? AucDet.TokenMoney : (Price2.BidderRate * (AucDet.TokenMoney / 100)),
            //                            TokenMoneyType = AucDet.TokenMoneyType,
            //                            EarnestMoneySubmitted = BEM2.EarnestMoneySubmitted,
            //                            TokenMoneySubmitted = Payment2.ID == null ? 0 : (Payment2.PaymentType == "TokenMoney" ? Payment2.PaidAmount : 0),
            //                            BalanceAmount = Price2.BidderRate,
            //                            SubmissionFeeDate = AucDet.SubDateOfBalanceAmount
            //                        }).FirstOrDefault();

            //return Asset;

            dynamic Asset = (from Price in context.AC_AuctionPrice
                             join AucDet in context.AC_AuctionAssetDetail on Price.AuctionAssetDetailID equals AucDet.ID
                             join AuctionAssets in context.AC_AuctionAssets on AucDet.AuctionAssetID equals AuctionAssets.ID
                             join AcNotice in context.AC_AuctionNotice on AuctionAssets.AuctionNoticeID equals AcNotice.ID
                             join Category in context.AM_Category on AuctionAssets.AssetCategoryID equals Category.ID
                             join SCategory in context.AM_SubCategory on AuctionAssets.AssetSubCategoryID equals SCategory.ID

                             join AI in context.AC_AuctionAssetItems on AuctionAssets.ID equals AI.AuctionAssetID into AI1
                             from AI2 in AI1.DefaultIfEmpty()
                             join AAI in context.AM_AssetItems on AI2.AssetItemID equals AAI.ID into AAI1
                             from AAI2 in AAI1.DefaultIfEmpty()
                             join BEM in context.AC_BidderEarnestMoney on AucDet.ID equals BEM.AuctionAssetDetailID into BEM1
                             from BEM2 in BEM1.DefaultIfEmpty()
                             join Bid in context.AC_AuctionBidder on BEM2.AuctionBidderID equals Bid.ID into Bid1
                             from Bid2 in Bid1.DefaultIfEmpty()
                             join Payment in context.AC_AuctionPayment on new { NotID = AuctionAssets.AuctionNoticeID, PriceID = Price.ID } equals new { NotID = Payment.AuctionNoticeID, PriceID = Payment.AuctionPriceID } into Payment1
                             from Payment2 in Payment1.DefaultIfEmpty()

                             where Price.ID == _mdlAuctionPrice.ID
                             select new
                             {
                                 ID = Price.ID,
                                 NoticeName = AcNotice.AuctionTitle,
                                 OpeningDate = AcNotice.OpeningDate,
                                 SubmissionDate = AcNotice.SubmissionDate,
                                 AssetCategory = Category.Name,
                                 AssetSubCategory = SCategory.Name,
                                 AssetName = AuctionAssets.GroupIndividual == "Group" ? AuctionAssets.GroupName : AAI2.AssetName,
                                 SelectedBidder = Bid2.BidderName,
                                 BidderRep = BEM2.BidderRepresentative,
                                 BidderRate = Price.BidderRate,
                                 BidderID = Bid2.ID,
                                 AuctionAssetDetailID = AucDet.ID,
                                 EarnestMoney = AucDet.EarnestMoneyType == "LumpSum" ? AucDet.EarnestMoney : (AucDet.ReservePrice * (AucDet.EarnestMoney / 100)),
                                 EarnestMoneyType = AucDet.EarnestMoneyType,
                                 TokenMoney = AucDet.TokenMoneyType == "LumpSum" ? AucDet.TokenMoney : (Price.BidderRate * (AucDet.TokenMoney / 100)),
                                 TokenMoneyType = AucDet.TokenMoneyType,
                                 EarnestMoneySubmitted = BEM2.EarnestMoneySubmitted,
                                 TokenMoneySubmitted = Payment2.ID == null ? 0 : (Payment2.PaymentType == "TokenMoney" ? Payment2.PaidAmount : 0),
                                 BalanceAmount = Price.BidderRate,
                                 SubmissionFeeDate = AucDet.SubDateOfBalanceAmount,
                                 Status = AuctionAssets.DeliveryStatus,
                                 Remarks = AuctionAssets.DeliveryStatusRemarks,
                                 StatusAttachment = AuctionAssets.DeliveryAttachment,
                                 AuctionAssetID = AuctionAssets.ID
                             }).FirstOrDefault();

            return Asset;

        }

        public List<dynamic> GetAssetsforBidderEarnestMoneyView(long _AuctionNoticeID)
        {
            List<dynamic> Assets = (from AucAssets in context.AC_AuctionAssets
                                    join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID into AI1
                                    from AI2 in AI1.DefaultIfEmpty()
                                    join AAI in context.AM_AssetItems on AI2.AssetItemID equals AAI.ID into AAI1
                                    from AAI2 in AAI1.DefaultIfEmpty()
                                    join ADet in context.AC_AuctionAssetDetail on AucAssets.ID equals ADet.AuctionAssetID
                                    join Bid in context.AC_AuctionBidder on AucAssets.AuctionNoticeID equals Bid.AuctionNoticeID into Bid1
                                    from Bid2 in Bid1.DefaultIfEmpty()
                                    join BEM in context.AC_BidderEarnestMoney on new { DetIID = ADet.ID, BidID = Bid2.ID } equals new { DetIID = BEM.AuctionAssetDetailID, BidID = BEM.AuctionBidderID } into BEM1
                                    from BEM2 in BEM1.DefaultIfEmpty()

                                    where AucAssets.AuctionNoticeID == _AuctionNoticeID
                                    select new
                                    {
                                        ID = (long?)BEM2.ID,
                                        AuctionDetailID = ADet.ID,
                                        AssetName = AI2 == null ? AucAssets.GroupName : AAI2.AssetName,
                                        EarnestMoney = ADet.EarnestMoneyType == "LumpSum" ? ADet.EarnestMoney : (ADet.ReservePrice * (ADet.EarnestMoney / 100)),//ADet.EarnestMoney,
                                        EarnestMoneySubmitted = BEM2 == null ? "" : SqlFunctions.StringConvert(BEM2.EarnestMoneySubmitted).Trim(),
                                        Attachment = BEM2.EMAttachment,
                                        BidderName = Bid2.BidderName

                                    }).Distinct().ToList<dynamic>();

            return Assets;
        }

        public List<dynamic> GetAllAssetsAddedAgainstTenderNotice(long _AuctionNoticeID)
        {
            List<dynamic> Assetitems = (from AucAssets in context.AC_AuctionAssets
                                 join AI in context.AC_AuctionAssetItems on AucAssets.ID equals AI.AuctionAssetID
                                 where AucAssets.AuctionNoticeID == _AuctionNoticeID
                                 select new
                                 {
                                     ID = AI.AssetItemID,

                                 }).Distinct().ToList<dynamic>();

            return Assetitems;
        }
        public bool DeleteBiddersforUpdation(long _AuctionBidderID)
        {
            bool IsDeleted = false;
            try
            {
                context.AC_BidderEarnestMoney.RemoveRange(context.AC_BidderEarnestMoney.Where(q => q.AuctionBidderID == _AuctionBidderID));
                context.SaveChanges();
                IsDeleted = true;
            }
            catch (Exception)
            {

                throw;
            }
            return IsDeleted;
        }
        public bool DeleteAuctionNoticeAttachmentsforUpdation(long _AuctionNoticeID)
        {
            bool IsDeleted = false;
            try
            {
                context.AC_AuctionNoticeAttachment.RemoveRange(context.AC_AuctionNoticeAttachment.Where(q => q.AcutionNoticeID == _AuctionNoticeID));
                context.SaveChanges();
                IsDeleted = true;
            }
            catch (Exception)
            {

                throw;
            }
            return IsDeleted;
        }
        #endregion

        #region Notification

        public AC_GetPendingApprovalNotifyData_Result GetPendingApprovalNotifyData(long _AuctionNoticeID, long _AuctionAssetsID, long AuctionPriceID)
        {
            AC_GetPendingApprovalNotifyData_Result lstPendingApprovalNotifyData = context.AC_GetPendingApprovalNotifyData(_AuctionNoticeID, _AuctionAssetsID,AuctionPriceID).FirstOrDefault<AC_GetPendingApprovalNotifyData_Result>();

            return lstPendingApprovalNotifyData;
        }

        public AC_GetApprovedCanceledNotifyData_Result GetApprovedCanceledNotifyData(long _AuctionNoticeID, long _AuctionAssetsID, long AuctionPriceID, long _UserID)
        {
            AC_GetApprovedCanceledNotifyData_Result lstApprovedCanceledNotifyData = context.AC_GetApprovedCanceledNotifyData(_AuctionNoticeID, _AuctionAssetsID, AuctionPriceID, _UserID).FirstOrDefault<AC_GetApprovedCanceledNotifyData_Result>();

            return lstApprovedCanceledNotifyData;
        }
        #endregion  Notification
    }
}
