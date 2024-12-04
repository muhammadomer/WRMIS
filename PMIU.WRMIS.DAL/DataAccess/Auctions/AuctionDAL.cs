using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.Repositories.Auctions;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace PMIU.WRMIS.DAL.DataAccess.Auctions
{
    public class AuctionDAL
    {
        ContextDB db = new ContextDB();

        #region "Auction Notice"
        public List<dynamic> GetApprovalAuthorities()
        {
            return db.Repository<AC_ApprovelAuthority>().GetAll().Select(x => new { x.ID, x.Name }).ToList<dynamic>();
        }

        public bool DeletePublishingSourceforUpdation(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().DeletePublishingSourceforUpdation(_AuctionNoticeID);
        }

        public long SaveAuctionNotice(AC_AuctionNotice _AuctionNoticeModel)
        {
            long AuctionNoticeID = 0;
            try
            {
                if (_AuctionNoticeModel.ID > 0)
                {
                    db.Repository<AC_AuctionNotice>().Update(_AuctionNoticeModel);
                    db.Save();
                    AuctionNoticeID = _AuctionNoticeModel.ID;
                }
                else
                {
                    db.Repository<AC_AuctionNotice>().Insert(_AuctionNoticeModel);
                    db.Save();
                    AuctionNoticeID = _AuctionNoticeModel.ID;
                }


            }
            catch (Exception)
            {

                throw;
            }

            return AuctionNoticeID;

        }

        public bool SavePublishingSource(AC_AdvertisementSource _AuctionPublishingSource)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<AC_AdvertisementSource>().Insert(_AuctionPublishingSource);
                db.Save();

            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }

            return IsSaved;

        }

        public AC_AuctionNotice GetAuctionNoticeData(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionNotice>().GetAll().Where(x => x.ID == _AuctionNoticeID).FirstOrDefault();
        }

        public List<dynamic> GetPublishingSourceByAuctionNoticeID(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetPublishingSourceByAuctionNoticeID(_AuctionNoticeID);
        }

        public List<AC_AuctionNotice> GetAllAuctionNotice(string _AuctionNoticeName, List<long> _DivisionID, DateTime? _OpeningDateFrom, DateTime? _OpeningDateTo)
        {
            List<AC_AuctionNotice> lstTenderNotice = db.Repository<AC_AuctionNotice>().GetAll().Where(x =>
                (x.AuctionTitle.Contains(_AuctionNoticeName) || _AuctionNoticeName == string.Empty) &&
                (_DivisionID.Contains(x.DivisionID) || _DivisionID.Count == 0) &&
                (_OpeningDateFrom == null || (DbFunctions.TruncateTime(x.OpeningDate) >=  DbFunctions.TruncateTime(_OpeningDateFrom)) &&
                (_OpeningDateTo == null || (DbFunctions.TruncateTime(x.OpeningDate) <= DbFunctions.TruncateTime(_OpeningDateTo))))).OrderByDescending(x => x.ID).ToList<AC_AuctionNotice>();


            return lstTenderNotice;
        }

        public AC_AuctionNotice GetAuctionDetailsByID(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionNotice>().GetAll().Where(x => x.ID == _AuctionNoticeID).FirstOrDefault();
        }
        
        public List<dynamic> GetBiddersByAuctionNoticeID(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetBiddersByAuctionNoticeID(_AuctionNoticeID);
        }
        public bool SaveAuctionBidder(AC_AuctionBidder _mdlAuctionBidder)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<AC_AuctionBidder>().Insert(_mdlAuctionBidder);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public List<dynamic> GetAssetCategories()
        {
            return db.Repository<AM_Category>().GetAll().Select(x => new { x.ID, x.Name }).ToList<dynamic>();
        }
        public List<dynamic> GetAssetSubCategoriesByCategoryID(long _CategoryID)
        {
            return db.Repository<AM_SubCategory>().GetAll().Where(x=>x.CategoryID == _CategoryID).Select(x => new { x.ID, x.Name }).ToList<dynamic>();
        }
        public List<dynamic> GetAssetsBySubCategoryID(long _SubCategoryID,long _IrrigationBoundryID,List<long> IDs)
        {
            return db.Repository<AM_AssetItems>().GetAll().Where(x => x.SubCategoryID == _SubCategoryID && x.IrrigationBoundryID == _IrrigationBoundryID && x.IsAuctioned != true && !IDs.Contains(x.ID)).Select(x => new { ID = x.ID, Name = x.AssetName }).ToList<dynamic>();
        }
        public List<dynamic> GetAssetsBySubCategoryIDForCircleLevel(long _SubCategoryID, long _DivisionID, long _CircleID, List<long> IDs)
        {
            return db.Repository<AM_AssetItems>().GetAll().Where(x => x.SubCategoryID == _SubCategoryID && (x.IrrigationBoundryID == _DivisionID || x.IrrigationBoundryID == _CircleID) && x.IsAuctioned != true && !IDs.Contains(x.ID)).Select(x => new { ID = x.ID, Name = x.AssetName }).ToList<dynamic>();
        }
        public List<dynamic> GetAssetsBySubCategoryIDForZoneLevel(long _SubCategoryID, long _DivisionID, long _CircleID, long _ZoneID, List<long> IDs)
        {
            return db.Repository<AM_AssetItems>().GetAll().Where(x => x.SubCategoryID == _SubCategoryID && (x.IrrigationBoundryID == _DivisionID || x.IrrigationBoundryID == _CircleID || x.IrrigationBoundryID == _ZoneID) && x.IsAuctioned != true && !IDs.Contains(x.ID)).Select(x => new { ID = x.ID, Name = x.AssetName }).ToList<dynamic>();
        }
        public List<dynamic> GetAttributesBySubCategoryID(long _AssetItemID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAssetAttributesBySubCategoryID(_AssetItemID);
        }
        public string GetAttributeValueByID(long _AttributeID)
        {
            return db.Repository<AM_AssetAttributes>().GetAll().Where(x => x.ID == _AttributeID).Select(x => x.AttributeValue).FirstOrDefault();
        }
        public long SaveAssetParent(AC_AuctionAssets _mdlAsset)
        {
            long ID = 0;
            try
            {
                if (_mdlAsset.ID > 0)
                {
                    AC_AuctionAssets mdlAsset = db.Repository<AC_AuctionAssets>().FindById(_mdlAsset.ID);
                    mdlAsset.AuctionNoticeID = _mdlAsset.AuctionNoticeID;
                    mdlAsset.GroupIndividual = _mdlAsset.GroupIndividual;
                    mdlAsset.GroupName = _mdlAsset.GroupName;
                    mdlAsset.AssetCategoryID = _mdlAsset.AssetCategoryID;
                    mdlAsset.AssetSubCategoryID = _mdlAsset.AssetSubCategoryID;
                    mdlAsset.IrrigationLevelID = _mdlAsset.IrrigationLevelID;
                    mdlAsset.ModifiedDate = _mdlAsset.ModifiedDate;
                    mdlAsset.ModifiedBy = _mdlAsset.ModifiedBy;
                    db.Repository<AC_AuctionAssets>().Update(mdlAsset);
                    db.Save();
                    ID = _mdlAsset.ID;
                }
                else
                {
                    db.Repository<AC_AuctionAssets>().Insert(_mdlAsset);
                    db.Save();
                    ID = _mdlAsset.ID;
                }
                
            }
            catch (Exception)
            {
                
                throw;
            }
            return ID;
        }

        public bool SaveAssetItem(AC_AuctionAssetItems _mdlAssetItem)
        {
            bool IsSaved = true;
            try
            {
                if (_mdlAssetItem.ID > 0)
                {
                    AC_AuctionAssetItems mdlItems = db.Repository<AC_AuctionAssetItems>().FindById(_mdlAssetItem.ID);
                    mdlItems.ModifiedBy = _mdlAssetItem.ModifiedBy;
                    mdlItems.ModifiedDate = _mdlAssetItem.ModifiedDate;
                    mdlItems.AssetAttributeID = _mdlAssetItem.AssetAttributeID;
                    mdlItems.AssetItemID = _mdlAssetItem.AssetItemID;
                    mdlItems.AssetQuantity = _mdlAssetItem.AssetQuantity;
                    mdlItems.AuctionAssetID = _mdlAssetItem.AuctionAssetID;
                    db.Repository<AC_AuctionAssetItems>().Update(mdlItems);
                    db.Save();
                }
                else
                {
                    db.Repository<AC_AuctionAssetItems>().Insert(_mdlAssetItem);
                    db.Save();
                }
             
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public string GetAssetTypeByID(long _AssetItemID)
        {
            return db.Repository<AM_AssetItems>().FindById(_AssetItemID).AssetType;
        }
        public List<dynamic> GetAuctionAssetsByNoticeID(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAuctionAssetsByNoticeID(_AuctionNoticeID);
        }
        public List<dynamic> GetAuctionAssetItemsByID(long _AuctionAssetID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAuctionAssetItemsByID(_AuctionAssetID);
        }
        public bool CompareLotQuantity(int _Quantity, long _AssetID)
        {
            bool Valid = false;
                int? Qty = db.Repository<AM_AssetItems>().FindById(_AssetID).AssetAvailableQuantity;
                if (_Quantity <= Qty)
                {
                    Valid = true;
                }
                else
                {
                    Valid = false;
                }
            return Valid;
        }

        public bool SaveGroupedAssetitem(AC_AuctionAssetItems _mdlAuctionAssetItem)
        {
            bool IsSaved = true;
            try
            {
            db.Repository<AC_AuctionAssetItems>().Insert(_mdlAuctionAssetItem);
            db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public bool DeleteAuctionAssetItemByID(long _AuctionAssetItemID)
        {
            bool IsDeleted = true;
            try
            {
                db.Repository<AC_AuctionAssetItems>().Delete(_AuctionAssetItemID);
                db.Save();
            }
            catch (Exception)
            {
                IsDeleted = false;
                throw;
            }
            return IsDeleted;
        }
        public bool DeleteAuctionAssetItemByAuctionAssetID(long _AuctionAssetID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().DeleteAuctionAssetItemByAuctionAssetID(_AuctionAssetID);
        }
        public bool DeleteAuctionAssetByID(long _AuctionAssetID)
        {
            bool IsDeleted = true;
            try
            {
                db.Repository<AC_AuctionAssets>().Delete(_AuctionAssetID);
                db.Save();
            }
            catch (Exception)
            {
                IsDeleted = false;
                throw;
            }
            return IsDeleted;
        }
        public bool IsHeadQuarterDivision(long? _IrrigationBoundryID)
        {
            return db.Repository<AM_HeadquarterDivision>().GetAll().Any(x => x.DivisionID == _IrrigationBoundryID);
        }

        public long? GetCircleIDByDivisionID(long _DivisionID)
        {
            return db.Repository<CO_Division>().GetAll().Where(x => x.ID == _DivisionID).Select(x => x.CircleID).FirstOrDefault();
        }
        public long GetZoneIDByCircleID(long _CircleID)
        {
            return db.Repository<CO_Circle>().GetAll().Where(x => x.ID == _CircleID).Select(x => x.ZoneID).FirstOrDefault();
        }
        public dynamic GetAuctionAssetDetails(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAuctionAssetDetails(_AuctionNoticeID, _AuctionAssetID);
        }
        public bool SaveAuctionAssetDetails(AC_AuctionAssetDetail _mdlAuctionAssetDetail)
        {
            bool IsSaved = true;
            try
            {
                if (_mdlAuctionAssetDetail.ID > 0)
                {
                    AC_AuctionAssetDetail mdlDetails = db.Repository<AC_AuctionAssetDetail>().FindById(_mdlAuctionAssetDetail.ID);
                    mdlDetails.ReservePrice = _mdlAuctionAssetDetail.ReservePrice;
                    mdlDetails.EarnestMoneyType = _mdlAuctionAssetDetail.EarnestMoneyType;
                    mdlDetails.EarnestMoney = _mdlAuctionAssetDetail.EarnestMoney;
                    mdlDetails.TokenMoneyType = _mdlAuctionAssetDetail.TokenMoneyType;
                    mdlDetails.TokenMoney = _mdlAuctionAssetDetail.TokenMoney;
                    mdlDetails.SubDateOfBalanceAmount = _mdlAuctionAssetDetail.SubDateOfBalanceAmount;
                    mdlDetails.ModifiedBy = _mdlAuctionAssetDetail.ModifiedBy;
                    mdlDetails.ModifiedDate = _mdlAuctionAssetDetail.ModifiedDate;

                    db.Repository<AC_AuctionAssetDetail>().Update(mdlDetails);
                }
                else
                {
                    db.Repository<AC_AuctionAssetDetail>().Insert(_mdlAuctionAssetDetail);
                }
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public bool IsAuctionAssetDetailExists(long _AuctionAssetID)
        {
            return db.Repository<AC_AuctionAssetDetail>().GetAll().Any(x => x.AuctionAssetID == _AuctionAssetID);
        }
        public AC_AuctionAssetDetail getAuctionAssetDetailByAssetID(long _AuctionAssetID)
        {
            return db.Repository<AC_AuctionAssetDetail>().GetAll().Where(x => x.AuctionAssetID == _AuctionAssetID).FirstOrDefault();
        }
        public List<dynamic> GetAssetsforBidderEarnestMoney(long _AuctionNoticeID, long _BidderID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAssetsforBidderEarnestMoney(_AuctionNoticeID,_BidderID);
        }
        public bool SaveBidderEarnestMoney(AC_BidderEarnestMoney _mdlBidderEarnestMoney, bool IsAttachemntChnaged)
        {
            bool IsSaved = true;
            try
            {
                //if (_mdlBidderEarnestMoney.ID > 0)
                //{
                //    AC_BidderEarnestMoney mdlDetails = db.Repository<AC_BidderEarnestMoney>().FindById(_mdlBidderEarnestMoney.ID);
                //    mdlDetails.EarnestMoneySubmitted = _mdlBidderEarnestMoney.EarnestMoneySubmitted;
                //    mdlDetails.AuctionBidderID = _mdlBidderEarnestMoney.AuctionBidderID;
                //    mdlDetails.AuctionAssetDetailID = _mdlBidderEarnestMoney.AuctionAssetDetailID;
                //    if (IsAttachemntChnaged)
                //    {
                //         mdlDetails.EMAttachment = _mdlBidderEarnestMoney.EMAttachment;
                //    }
                //    //mdlDetails.EMAttachment = _mdlBidderEarnestMoney.EMAttachment;
                //    mdlDetails.ModifiedBy = _mdlBidderEarnestMoney.ModifiedBy;
                //    mdlDetails.ModifiedDate = _mdlBidderEarnestMoney.ModifiedDate;

                //    db.Repository<AC_BidderEarnestMoney>().Update(mdlDetails);
                //}
                //else
                //{
                    db.Repository<AC_BidderEarnestMoney>().Insert(_mdlBidderEarnestMoney);
                //}
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public bool DeleteBidderEarnestMoney(long _ID)
        {
            bool IsDeleted = true;
            try
            {
                db.Repository<AC_BidderEarnestMoney>().Delete(_ID);
                db.Save();
            }
            catch (Exception)
            {
                IsDeleted = false;
                throw;
            }
            return IsDeleted;
        }
        public List<dynamic> GetCommiteeMembersByNoticeID(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionCommiteeMembers>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID).Select(x => new { ID = x.ID, AuctionNoticeID = x.AuctionNoticeID, AuctionOpeningID = x.AuctionOpeningID, MemberName = x.MemberName, Designation = x.Designation }).ToList<dynamic>();
        }
        public bool SaveCommittemembers(AC_AuctionCommiteeMembers _mdlCommitteMembers)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<AC_AuctionCommiteeMembers>().Insert(_mdlCommitteMembers);
                db.Save();

            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }

            return IsSaved;

        }
        public bool DeleteCommitteMembersforUpdation(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().DeleteCommitteMembersforUpdation(_AuctionNoticeID);
        }
        public List<dynamic> GetAssetsByNoticeID(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAssetsByNoticeID(_AuctionNoticeID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetID(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetBiddersByNoticeAndAssetID(_AuctionNoticeID, _AuctionAssetID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetIDForViewMode(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetBiddersByNoticeAndAssetIDForViewMode(_AuctionNoticeID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetIDForBiddingProcess(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetBiddersByNoticeAndAssetIDForBiddingProcess(_AuctionNoticeID, _AuctionAssetID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetIDForBidderSelection(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetBiddersByNoticeAndAssetIDForBidderSelection(_AuctionNoticeID, _AuctionAssetID);
        }
        public long SaveAuctionOpening(AC_AuctionOpening _mdlAuctionOpening)
        {
            long AuctionOpeningID = 0;
            try
            {
                if (_mdlAuctionOpening.ID > 0)
                {
                    db.Repository<AC_AuctionOpening>().Update(_mdlAuctionOpening);
                    db.Save();
                    AuctionOpeningID = _mdlAuctionOpening.ID;
                }
                else
                {
                    db.Repository<AC_AuctionOpening>().Insert(_mdlAuctionOpening);
                    db.Save();
                    AuctionOpeningID = _mdlAuctionOpening.ID;
                }


            }
            catch (Exception)
            {

                throw;
            }

            return AuctionOpeningID;

        }
        public List<dynamic> GetCommitteMembersByNoticeID(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionCommiteeMembers>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID).Select(x => new { MemberName = x.MemberName, Designation = x.Designation }).ToList<dynamic>();
        }
        public AC_AuctionOpening getAuctionOpeningDataByNoticeID(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionOpening>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID).FirstOrDefault();
        }
        public bool UpdateBidderAttendance(AC_BidderEarnestMoney _mdlBidderEarnestMoney)
        {
            bool IsSaved = true;
            try
            {
                AC_BidderEarnestMoney mdlBidder = db.Repository<AC_BidderEarnestMoney>().FindById(_mdlBidderEarnestMoney.ID);
                mdlBidder.Attended = _mdlBidderEarnestMoney.Attended;
                mdlBidder.Remarks = _mdlBidderEarnestMoney.Remarks;
                mdlBidder.BidderRepresentative = _mdlBidderEarnestMoney.BidderRepresentative;
                mdlBidder.ModifiedBy = _mdlBidderEarnestMoney.ModifiedBy;
                mdlBidder.ModifiedDate = _mdlBidderEarnestMoney.ModifiedDate;
                db.Repository<AC_BidderEarnestMoney>().Update(mdlBidder);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }

            return IsSaved;

        }
        public bool UpdateBidderAttendanceAttachement(AC_AuctionOpening _mdlAuctionOpening, bool IsAttUpdated)
        {
            bool IsSaved = true;
            try
            {
                AC_AuctionOpening mdlBidder = db.Repository<AC_AuctionOpening>().GetAll().Where(x => x.AuctionNoticeID == _mdlAuctionOpening.AuctionNoticeID).FirstOrDefault();
                if (IsAttUpdated)
                {
                    mdlBidder.BidderAttendanceFile = _mdlAuctionOpening.BidderAttendanceFile;
                }
                mdlBidder.ModifiedBy = _mdlAuctionOpening.ModifiedBy;
                mdlBidder.ModifiedDate = _mdlAuctionOpening.ModifiedDate;
                db.Repository<AC_AuctionOpening>().Update(mdlBidder);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }

            return IsSaved;
        }
        public string GetBidderAttendanceAttachment(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionOpening>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID).Select(x => x.BidderAttendanceFile).FirstOrDefault();
        }
        public bool SaveAuctionPrice(AC_AuctionPrice _mdlAuctionPrice)
        {
            bool IsSaved = true;
            try
            {
                if (_mdlAuctionPrice.ID > 0)
                {
                    AC_AuctionPrice mdlAuctionPrice = db.Repository<AC_AuctionPrice>().FindById(_mdlAuctionPrice.ID);
                    mdlAuctionPrice.ModifiedBy = _mdlAuctionPrice.ModifiedBy;
                    mdlAuctionPrice.ModifiedDate = _mdlAuctionPrice.ModifiedDate;
                    mdlAuctionPrice.BidderRate = _mdlAuctionPrice.BidderRate;
                    db.Repository<AC_AuctionPrice>().Update(mdlAuctionPrice);
                    db.Save();
                }
                else
                {
                    db.Repository<AC_AuctionPrice>().Insert(_mdlAuctionPrice);
                    db.Save();
                }
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public bool SaveAuctionPriceForStatus(AC_AuctionPrice _mdlAuctionPrice)
        {
            bool IsSaved = true;
            try
            {
                if (_mdlAuctionPrice.ID > 0)
                {
                    AC_AuctionPrice mdlAuctionPrice = db.Repository<AC_AuctionPrice>().FindById(_mdlAuctionPrice.ID);
                    mdlAuctionPrice.ModifiedBy = _mdlAuctionPrice.ModifiedBy;
                    mdlAuctionPrice.ModifiedDate = _mdlAuctionPrice.ModifiedDate;
                    mdlAuctionPrice.Status = _mdlAuctionPrice.Status;
                    mdlAuctionPrice.StatusAttachment = _mdlAuctionPrice.StatusAttachment;
                    mdlAuctionPrice.StatusRemarks = _mdlAuctionPrice.StatusRemarks;
                    db.Repository<AC_AuctionPrice>().Update(mdlAuctionPrice);
                    db.Save();
                }
                else
                {
                    db.Repository<AC_AuctionPrice>().Insert(_mdlAuctionPrice);
                    db.Save();
                }
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public bool DeleteAuctionPriceByID(long _AuctionPriceID)
        {
            bool IsDeleted = true;
            try
            {
                db.Repository<AC_AuctionPrice>().Delete(_AuctionPriceID);
                db.Save();
            }
            catch (Exception)
            {
                IsDeleted = false;
                throw;
            }
            return IsDeleted;
        }
        public bool SaveAwardedAuctionAsset(AC_AuctionPrice _mdlAuctionPrice)
        {
            bool IsSaved = true;
            try
            {
                if (_mdlAuctionPrice.ID > 0)
                {
                    AC_AuctionPrice mdlAuctionPrice = db.Repository<AC_AuctionPrice>().FindById(_mdlAuctionPrice.ID);
                    mdlAuctionPrice.Awarded = _mdlAuctionPrice.Awarded;
                    mdlAuctionPrice.AwardedRemarks = _mdlAuctionPrice.AwardedRemarks;
                    mdlAuctionPrice.ModifiedBy = _mdlAuctionPrice.ModifiedBy;
                    mdlAuctionPrice.ModifiedDate = _mdlAuctionPrice.ModifiedDate;

                    db.Repository<AC_AuctionPrice>().Update(mdlAuctionPrice);
                    db.Save();
                }
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public List<dynamic> GetAuctionItemsForPayments(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAuctionItemsForPayments(_AuctionNoticeID);
        }
       
        public double? TotalTokenMoneySubmitted(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return db.Repository<AC_AuctionPayment>().GetAll().Where(x => x.AuctionPriceID == _AuctionPriceID && x.AuctionNoticeID == _AuctionNoticeID && x.PaymentType == "TokenMoney").Sum(x => x.PaidAmount);
        }
        public double? TotalRemainingAmountSubmitted(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return db.Repository<AC_AuctionPayment>().GetAll().Where(x => x.AuctionPriceID == _AuctionPriceID && x.AuctionNoticeID == _AuctionNoticeID && x.PaymentType == "RemainingAmount").Sum(x => x.PaidAmount);
        }

        public AC_AuctionPrice GetAuctionPriceEntity(long _AuctionPriceID)
        {
            return db.Repository<AC_AuctionPrice>().FindById(_AuctionPriceID);
        }
        public dynamic GetAuctionDetails(AC_AuctionPrice _mdlAuctionPrice)
        {
            //return db.ExtRepositoryFor<AuctionRepository>().GetAuctionDetails(_AuctionNoticeID, _AuctionBidderID);

            return db.ExtRepositoryFor<AuctionRepository>().GetAuctionDetails(_mdlAuctionPrice);
        }

        public List<dynamic> GetRemainingpaymentsbyNoticeID(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return db.Repository<AC_AuctionPayment>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID && x.AuctionPriceID == _AuctionPriceID).Select(x => new { ID = x.ID, PaymentType = x.PaymentType, Amount = x.PaidAmount, Date = x.PaymentDate, Attachment = x.PaymentAttachment, PaymentTypeID = x.PaymentType == "TokenMoney" ? 1 : 2 }).ToList<dynamic>();
        }

        public bool SaveRemainingPayment(AC_AuctionPayment _mdlAuctionPayment)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<AC_AuctionPayment>().Insert(_mdlAuctionPayment);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public bool DeletePaymentEntryByID(long _PaymentID)
        {
            bool IsDeleted = true;
            try
            {
                db.Repository<AC_AuctionPayment>().Delete(_PaymentID);
                db.Save();
            }
            catch (Exception)
            {
                IsDeleted = false;
                throw;
            }
            return IsDeleted;
        }

        public bool UpdateDeliveryStatus(AC_AuctionStatusDetails _mdlAuctionAsset)
        {
            bool IsUpdated = true;
            try
            {
                db.Repository<AC_AuctionStatusDetails>().Insert(_mdlAuctionAsset);
                db.Save();
            }
            catch (Exception)
            {
                IsUpdated = false;   
                throw;
            }
            return IsUpdated;
        }

        public List<long> GetAssetItemIDs(long _AuctionAssetID)
        {
            return db.Repository<AC_AuctionAssetItems>().GetAll().Where(x => x.AuctionAssetID == _AuctionAssetID).Select(x => x.AssetItemID).ToList();
        }

        public bool UpdateAuctionStatus(long _AssetItemID)
        {
            bool IsUpdated = true;
            try
            {
                AM_AssetItems mdlAssetItem =  db.Repository<AM_AssetItems>().FindById(_AssetItemID);
                mdlAssetItem.IsAuctioned = true;
                db.Repository<AM_AssetItems>().Update(mdlAssetItem);
                db.Save();

            }
            catch (Exception)
            {
                IsUpdated = false;
                throw;
            }
            return IsUpdated;
        }
        public List<dynamic> GetAssetsforBidderEarnestMoneyView(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAssetsforBidderEarnestMoneyView(_AuctionNoticeID);
        }

        public bool IsBidderDuplicate(long _AuctionNoticeID, string _BidderName)
        {
           return db.Repository<AC_AuctionBidder>().GetAll().Where(x=>x.AuctionNoticeID == _AuctionNoticeID).Any(x=>x.BidderName.ToUpper() == _BidderName);
             
        }
        public dynamic GetDeliveryStatusData(long _AuctionAssetID)
        {
            return db.Repository<AC_AuctionAssets>().GetAll().Where(x => x.ID == _AuctionAssetID).Select(x => new { DeliveryStatus = x.DeliveryStatus, Remarks = x.DeliveryStatusRemarks, Attachment = x.DeliveryAttachment }).FirstOrDefault();
        }
        public List<dynamic> GetAllAssetsAddedAgainstTenderNotice(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetAllAssetsAddedAgainstTenderNotice(_AuctionNoticeID);
        }

        public string GetAssteTypeByAssetItemID(long _AssetItemID)
        {
            return db.Repository<AM_AssetItems>().GetAll().Where(x => x.ID == _AssetItemID).Select(x => x.AssetType).FirstOrDefault();
        }

        public int? GetLotQuantityByAuctionAssetID(long _AuctionAssetID, long _AssetItemID)
        {
            return db.Repository<AC_AuctionAssetItems>().GetAll().Where(x => x.AuctionAssetID == _AuctionAssetID && x.AssetItemID == _AssetItemID).Select(x => x.AssetQuantity).FirstOrDefault();
        }

        public int GetAvailableLotQuantity(long _AssetItemID)
        {
            return db.Repository<AM_AssetItems>().GetAll().Where(x => x.ID == _AssetItemID).Select(x => x.AssetAvailableQuantity).FirstOrDefault();
        }

        public bool UpdateLotQuantityInAsset(long _AssetItemID, int _Quantity)
        {
            bool IsUpdated = true;
            try
            {
                AM_AssetItems mdlAssetItem = db.Repository<AM_AssetItems>().FindById(_AssetItemID);
                mdlAssetItem.AssetAvailableQuantity = _Quantity;
                db.Repository<AM_AssetItems>().Update(mdlAssetItem);
                db.Save();
            }
            catch (Exception)
            {
                IsUpdated = false;
                throw;
            }
            return IsUpdated;
        }

        public long getAssetDetailIDByAssetID(long _AuctionAssetID)
        {
            return db.Repository<AC_AuctionAssetDetail>().GetAll().Where(x => x.AuctionAssetID == _AuctionAssetID).Select(x => x.ID).FirstOrDefault();
        }

        public AC_AuctionPrice GetAuctionPriceEntityByDetailID(long _AuctionAssetDetailID)
        {
            return db.Repository<AC_AuctionPrice>().GetAll().Where(x => x.AuctionAssetDetailID == _AuctionAssetDetailID).FirstOrDefault();
        }
        public bool AddLotInspectionInAsset(AM_AssetInspectionLot _mdlLot)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<AM_AssetInspectionLot>().Insert(_mdlLot);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public bool AddIndividualInspectionInAsset(AM_AssetInspectionInd _mdlInd)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<AM_AssetInspectionInd>().Insert(_mdlInd);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public long GetApprovalAuthorityByAuctionNoticeID(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionNotice>().GetAll().Where(x => x.ID == _AuctionNoticeID).Select(x=>x.ApprovalAuthorityID).FirstOrDefault();
        }
        public bool GetAwardedStatusByBidderID(long _AuctionDetailID)
        {
            return db.Repository<AC_AuctionPrice>().GetAll().Where(x => x.AuctionAssetDetailID == _AuctionDetailID).Any(x => x.Awarded == true);
        }
        public long GetAssetItemIDByAuctionAssetID(long _AuctionAssetID)
        {
            return db.Repository<AC_AuctionAssetItems>().GetAll().Where(x => x.AuctionAssetID == _AuctionAssetID).Select(x => x.ID).FirstOrDefault();
        }
        public bool IfItemsExistsAgainstGroup(long _AuctionAssetID)
        {
            return db.Repository<AC_AuctionAssetItems>().GetAll().Any(x => x.AuctionAssetID == _AuctionAssetID);
        }

        public bool UpdateAwardStatus(long _AuctionAssetDetailID)
        {
            bool IsUpdated = true;
            try
            {
                List<AC_AuctionPrice> lstPrice = db.Repository<AC_AuctionPrice>().GetAll().Where(x => x.AuctionAssetDetailID == _AuctionAssetDetailID).ToList();
                foreach (var item in lstPrice)
                {
                    item.Awarded = null;
                    item.AwardedRemarks = null;
                    db.Repository<AC_AuctionPrice>().Update(item);
                    db.Save();
                }
            }
            catch (Exception)
            {
                IsUpdated = false;
                throw;
            }
            return IsUpdated;
        }
        public bool DeleteBiddersforUpdation(long _AuctionBidderID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().DeleteBiddersforUpdation(_AuctionBidderID);
        }
        public bool DeleteAuctionNoticeAttachmentsforUpdation(long _AuctionNoticeID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().DeleteAuctionNoticeAttachmentsforUpdation(_AuctionNoticeID);
        }
        public string GetBidderNameByBidderID(long _BidderID)
        {
            return db.Repository<AC_AuctionBidder>().GetAll().Where(x => x.ID == _BidderID).Select(x => x.BidderName).FirstOrDefault();
        }
        public bool SaveAuctionNoticeAttachment(AC_AuctionNoticeAttachment _mdlAttachment)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<AC_AuctionNoticeAttachment>().Insert(_mdlAttachment);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public List<string> GetAuctionNoticeAttachment(long _AuctionNoticeID)
        {
            return db.Repository<AC_AuctionNoticeAttachment>().GetAll().Where(x => x.AcutionNoticeID == _AuctionNoticeID).Select(x => x.Attachment).ToList();
        }
        public string GetAssetTypeByAssetWorkID(long _WorkID)
        {
            return db.Repository<AM_AssetWork>().GetAll().Where(x => x.ID == _WorkID).Select(x => x.AssetType).FirstOrDefault();
        }
        #endregion


        #region Notification

        public AC_GetPendingApprovalNotifyData_Result GetPendingApprovalNotifyData(long _AuctionNoticeID, long _AuctionAssetsID, long AuctionPriceID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetPendingApprovalNotifyData(_AuctionNoticeID, _AuctionAssetsID, AuctionPriceID);
        }

        public AC_GetApprovedCanceledNotifyData_Result GetApprovedCanceledNotifyData(long _AuctionNoticeID, long _AuctionAssetsID, long AuctionPriceID, long _UserID)
        {
            return db.ExtRepositoryFor<AuctionRepository>().GetApprovedCanceledNotifyData(_AuctionNoticeID, _AuctionAssetsID, AuctionPriceID, _UserID);
        }
        public string GetStatusByNoticeAndPriceID(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return db.Repository<AC_AuctionStatusDetails>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID && x.AuctionPriceID == _AuctionPriceID).OrderByDescending(x => x.ID).Select(x => x.DeliveryStatus).FirstOrDefault();
        }
        public AC_AuctionStatusDetails GetAuctionStatusForSECE(long _AuctionNoticeID, long _AuctionPriceID, int _DesignationID)
        {
            return db.Repository<AC_AuctionStatusDetails>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID && x.AuctionPriceID == _AuctionPriceID && (x.DesignationID == 3 || x.DesignationID == 21)).OrderByDescending(x => x.ID).FirstOrDefault();
        }
        public AC_AuctionStatusDetails GetAuctionStatusForXEN(long _AuctionNoticeID, long _AuctionPriceID, int _DesignationID)
        {
            return db.Repository<AC_AuctionStatusDetails>().GetAll().Where(x => x.AuctionNoticeID == _AuctionNoticeID && x.AuctionPriceID == _AuctionPriceID && x.DesignationID == 4).OrderByDescending(x => x.ID).FirstOrDefault();
        }
        #endregion  Notification
       
    }
}
