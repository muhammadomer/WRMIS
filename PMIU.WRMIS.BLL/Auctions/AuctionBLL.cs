using PMIU.WRMIS.DAL.DataAccess.Auctions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Auctions
{
    public class AuctionBLL : BaseBLL
    {
        AuctionDAL AuctionDAL = new AuctionDAL();
        #region "Auction Notice"

        public List<dynamic> GetApprovalAuthorities()
        {
            return AuctionDAL.GetApprovalAuthorities();
        }

        public bool DeletePublishingSourceforUpdation(long _AuctionNoticeID)
        {
            return AuctionDAL.DeletePublishingSourceforUpdation(_AuctionNoticeID);
        }
        public long SaveAuctionNotice(AC_AuctionNotice _AuctionNoticeModel)
        {
            return AuctionDAL.SaveAuctionNotice(_AuctionNoticeModel);
        }

        public bool SavePublishingSource(AC_AdvertisementSource __AuctionPublishingSource)
        {
            return AuctionDAL.SavePublishingSource(__AuctionPublishingSource);
        }

        public AC_AuctionNotice GetAuctionNoticeByID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAuctionNoticeData(_AuctionNoticeID);
        }
        public List<dynamic> GetPublishingSourceByAuctionNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetPublishingSourceByAuctionNoticeID(_AuctionNoticeID);
        }
        public List<AC_AuctionNotice> GetAllAuctionNotice(string _AuctionNoticeName, List<long> _DivisionID, DateTime? _OpeningDateFrom, DateTime? _OpeningDateTo)
        {
            return AuctionDAL.GetAllAuctionNotice(_AuctionNoticeName, _DivisionID, _OpeningDateFrom, _OpeningDateTo);
        }

        public AC_AuctionNotice GetAuctionDetailsByID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAuctionDetailsByID(_AuctionNoticeID);
        }
        public List<dynamic> GetBiddersByAuctionNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetBiddersByAuctionNoticeID(_AuctionNoticeID);
        }
        public bool SaveAuctionBidder(AC_AuctionBidder _mdlAuctionBidder)
        {
            return AuctionDAL.SaveAuctionBidder(_mdlAuctionBidder);
        }
        public List<dynamic> GetAssetCategories()
        {
            return AuctionDAL.GetAssetCategories();
        }
        public List<dynamic> GetAssetSubCategoriesByCategoryID(long _CategoryID)
        {
            return AuctionDAL.GetAssetSubCategoriesByCategoryID(_CategoryID);
        }
        public List<dynamic> GetAssetsBySubCategoryID(long _SubCategoryID,long _IrrigationBoundryID,List<long> IDs)
        {
            return AuctionDAL.GetAssetsBySubCategoryID(_SubCategoryID, _IrrigationBoundryID,IDs);
        }
        public List<dynamic> GetAttributesBySubCategoryID(long _AssetItemID)
        {
            return AuctionDAL.GetAttributesBySubCategoryID(_AssetItemID);
        }
        public string GetAttributeValueByID(long _AttributeID)
        {
            return AuctionDAL.GetAttributeValueByID(_AttributeID);
        }
        public long SaveAssetParent(AC_AuctionAssets _mdlAssetItem)
        {
            return AuctionDAL.SaveAssetParent(_mdlAssetItem);
        }
        public bool SaveAssetItem(AC_AuctionAssetItems _mdlAssetItem)
        {
            return AuctionDAL.SaveAssetItem(_mdlAssetItem);
        }
        public string GetAssetTypeByID(long _AssetItemID)
        {
            return AuctionDAL.GetAssetTypeByID(_AssetItemID);
        }
        public List<dynamic> GetAuctionAssetsByNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAuctionAssetsByNoticeID(_AuctionNoticeID);
        }
        public List<dynamic> GetAuctionAssetItemsByID(long _AuctionAssetID)
        {
            return AuctionDAL.GetAuctionAssetItemsByID(_AuctionAssetID);
        }
        public bool CompareLotQuantity(int _Quantity, long _AssetID)
        {
            return AuctionDAL.CompareLotQuantity(_Quantity, _AssetID);
        }
        public bool SaveGroupedAssetitem(AC_AuctionAssetItems _mdlAuctionAssetItem)
        {
            return AuctionDAL.SaveGroupedAssetitem(_mdlAuctionAssetItem);
        }
        public bool DeleteAuctionAssetItemByID(long _AuctionAssetItemID)
        {
            return AuctionDAL.DeleteAuctionAssetItemByID(_AuctionAssetItemID);
        }
        public bool DeleteAuctionAssetItemByAuctionAssetID(long _AuctionAssetID)
        {
            return AuctionDAL.DeleteAuctionAssetItemByAuctionAssetID(_AuctionAssetID);
        }
        public bool DeleteAuctionAssetByID(long _AuctionAssetID)
        {
            return AuctionDAL.DeleteAuctionAssetByID(_AuctionAssetID);
        }
        public bool IsHeadQuarterDivision(long? _IrrigationBoundryID)
        {
            return AuctionDAL.IsHeadQuarterDivision(_IrrigationBoundryID);
        }
        public long? GetCircleIDByDivisionID(long _DivisionID)
        {
            return AuctionDAL.GetCircleIDByDivisionID(_DivisionID);
        }
        public long GetZoneIDByCircleID(long _CircleID)
        {
            return AuctionDAL.GetZoneIDByCircleID(_CircleID);
        }
        public List<dynamic> GetAssetsBySubCategoryIDForCircleLevel(long _SubCategoryID, long _DivisionID, long _CircleID, List<long> IDs)
        {
            return AuctionDAL.GetAssetsBySubCategoryIDForCircleLevel(_SubCategoryID, _DivisionID, _CircleID,IDs);
        }
        public List<dynamic> GetAssetsBySubCategoryIDForZoneLevel(long _SubCategoryID, long _DivisionID, long _CircleID, long _ZoneID, List<long> IDs)
        {
            return AuctionDAL.GetAssetsBySubCategoryIDForZoneLevel(_SubCategoryID, _DivisionID, _CircleID, _ZoneID,IDs);
        }
        public dynamic GetAuctionAssetDetails(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return AuctionDAL.GetAuctionAssetDetails(_AuctionNoticeID, _AuctionAssetID);
        }
        public bool SaveAuctionAssetDetails(AC_AuctionAssetDetail _mdlAuctionAssetDetail)
        {
            return AuctionDAL.SaveAuctionAssetDetails(_mdlAuctionAssetDetail);
        }
        public bool IsAuctionAssetDetailExists(long _AuctionAssetID)
        {
            return AuctionDAL.IsAuctionAssetDetailExists(_AuctionAssetID);
        }
        public AC_AuctionAssetDetail getAuctionAssetDetailByAssetID(long _AuctionAssetID)
        {
            return AuctionDAL.getAuctionAssetDetailByAssetID(_AuctionAssetID);
        }
        public List<dynamic> GetAssetsforBidderEarnestMoney(long _AuctionNoticeID, long _BidderID)
        {
            return AuctionDAL.GetAssetsforBidderEarnestMoney(_AuctionNoticeID,_BidderID);
        }
        public bool SaveBidderEarnestMoney(AC_BidderEarnestMoney _mdlBidderEarnestMoney, bool IsAttachmentChanged)
        {
            return AuctionDAL.SaveBidderEarnestMoney(_mdlBidderEarnestMoney, IsAttachmentChanged);
        }
        public bool DeleteBidderEarnestMoney(long _ID)
        {
            return AuctionDAL.DeleteBidderEarnestMoney(_ID);
        }
        public List<dynamic> GetCommiteeMembersByNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetCommiteeMembersByNoticeID(_AuctionNoticeID);
        }
        public bool SaveCommittemembers(AC_AuctionCommiteeMembers _mdlCommitteMembers)
        {
            return AuctionDAL.SaveCommittemembers(_mdlCommitteMembers);
        }
        public bool DeleteCommitteMembersforUpdation(long _AuctionNoticeID)
        {
            return AuctionDAL.DeleteCommitteMembersforUpdation(_AuctionNoticeID);
        }
        public long SaveAuctionOpening(AC_AuctionOpening _mdlAuctionOpening)
        {
            return AuctionDAL.SaveAuctionOpening(_mdlAuctionOpening);
        }
        public List<dynamic> GetCommitteMembersByNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetCommitteMembersByNoticeID(_AuctionNoticeID);
        }
        public AC_AuctionOpening getAuctionOpeningDataByNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.getAuctionOpeningDataByNoticeID(_AuctionNoticeID);
        }
        public List<dynamic> GetAssetsByNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAssetsByNoticeID(_AuctionNoticeID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetID(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return AuctionDAL.GetBiddersByNoticeAndAssetID(_AuctionNoticeID, _AuctionAssetID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetIDForViewMode(long _AuctionNoticeID)
        {
            return AuctionDAL.GetBiddersByNoticeAndAssetIDForViewMode(_AuctionNoticeID);
        }
        public bool UpdateBidderAttendance(AC_BidderEarnestMoney _mdlBidderEarnestMoney)
        {
            return AuctionDAL.UpdateBidderAttendance(_mdlBidderEarnestMoney);
        }
        public bool UpdateBidderAttendanceAttachement(AC_AuctionOpening _mdlAuctionOpening, bool IsAttupdated)
        {
            return AuctionDAL.UpdateBidderAttendanceAttachement(_mdlAuctionOpening, IsAttupdated);
        }
        public string GetBidderAttendanceAttachment(long _AuctionNoticeID)
        {
            return AuctionDAL.GetBidderAttendanceAttachment(_AuctionNoticeID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetIDForBiddingProcess(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return AuctionDAL.GetBiddersByNoticeAndAssetIDForBiddingProcess(_AuctionNoticeID, _AuctionAssetID);
        }
        public bool SaveAuctionPrice(AC_AuctionPrice _mdlAuctionPrice)
        {
            return AuctionDAL.SaveAuctionPrice(_mdlAuctionPrice);
        }
        public bool SaveAuctionPriceForStatus(AC_AuctionPrice _mdlAuctionPrice)
        {
            return AuctionDAL.SaveAuctionPriceForStatus(_mdlAuctionPrice);
        }
        public bool DeleteAuctionPriceByID(long _AuctionPriceID)
        {
            return AuctionDAL.DeleteAuctionPriceByID(_AuctionPriceID);
        }
        public List<dynamic> GetBiddersByNoticeAndAssetIDForBidderSelection(long _AuctionNoticeID, long _AuctionAssetID)
        {
            return AuctionDAL.GetBiddersByNoticeAndAssetIDForBidderSelection(_AuctionNoticeID, _AuctionAssetID);
        }
        public bool SaveAwardedAuctionAsset(AC_AuctionPrice _mdlAuctionPrice)
        {
            return AuctionDAL.SaveAwardedAuctionAsset(_mdlAuctionPrice);
        }
        public List<dynamic> GetAuctionItemsForPayments(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAuctionItemsForPayments(_AuctionNoticeID);
        }
        public double? TotalTokenMoneySubmitted(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return AuctionDAL.TotalTokenMoneySubmitted(_AuctionNoticeID, _AuctionPriceID);
        }
        public double? TotalRemainingAmountSubmitted(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return AuctionDAL.TotalRemainingAmountSubmitted(_AuctionNoticeID, _AuctionPriceID);
        }
        public AC_AuctionPrice GetAuctionPriceEntity(long _AuctionPriceID)
        {
            return AuctionDAL.GetAuctionPriceEntity(_AuctionPriceID);
        }
        public dynamic GetAuctionDetails(AC_AuctionPrice _mdlAuctionPrice)
        {
            //return AuctionDAL.GetAuctionDetails(_AuctionNoticeID, _AuctionBidderID);
            return AuctionDAL.GetAuctionDetails(_mdlAuctionPrice);
        }
        public List<dynamic> GetRemainingpaymentsbyNoticeID(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return AuctionDAL.GetRemainingpaymentsbyNoticeID(_AuctionNoticeID, _AuctionPriceID);
        }
        public bool SaveRemainingPayment(AC_AuctionPayment _mdlAuctionPayment)
        {
            return AuctionDAL.SaveRemainingPayment(_mdlAuctionPayment);
        }
        public bool DeletePaymentEntryByID(long _PaymentID)
        {
            return AuctionDAL.DeletePaymentEntryByID(_PaymentID);
        }
        public bool UpdateDeliveryStatus(AC_AuctionStatusDetails _mdlAuctionAsset)
        {
            return AuctionDAL.UpdateDeliveryStatus(_mdlAuctionAsset);
        }
        public List<long> GetAssetItemIDs(long _AuctionAssetID)
        {
            return AuctionDAL.GetAssetItemIDs(_AuctionAssetID);
        }
        public bool UpdateAuctionStatus(long _AssetItemID)
        {
            return AuctionDAL.UpdateAuctionStatus(_AssetItemID);
        }
        public List<dynamic> GetAssetsforBidderEarnestMoneyView(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAssetsforBidderEarnestMoneyView(_AuctionNoticeID);
        }
        public bool IsBidderDuplicate(long _AuctionNoticeID, string _BidderName)
        {
            return AuctionDAL.IsBidderDuplicate(_AuctionNoticeID, _BidderName);
        }
        public dynamic GetDeliveryStatusData(long _AuctionAssetID)
        {
            return AuctionDAL.GetDeliveryStatusData(_AuctionAssetID);
        }
        public List<dynamic> GetAllAssetsAddedAgainstTenderNotice(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAllAssetsAddedAgainstTenderNotice(_AuctionNoticeID);
        }
        public string GetAssteTypeByAssetItemID(long _AssetItemID)
        {
            return AuctionDAL.GetAssteTypeByAssetItemID(_AssetItemID);
        }
        public int? GetLotQuantityByAuctionAssetID(long _AuctionAssetID, long _AssetItemID)
        {
            return AuctionDAL.GetLotQuantityByAuctionAssetID(_AuctionAssetID,_AssetItemID);
        }
        public int GetAvailableLotQuantity(long _AssetItemID)
        {
            return AuctionDAL.GetAvailableLotQuantity(_AssetItemID);
        }
        public bool UpdateLotQuantityInAsset(long _AssetItemID, int _Quantity)
        {
            return AuctionDAL.UpdateLotQuantityInAsset(_AssetItemID, _Quantity);
        }
        public long getAssetDetailIDByAssetID(long _AuctionAssetID)
        {
            return AuctionDAL.getAssetDetailIDByAssetID(_AuctionAssetID);
        }
        public AC_AuctionPrice GetAuctionPriceEntityByDetailID(long _AuctionAssetDetailID)
        {
            return AuctionDAL.GetAuctionPriceEntityByDetailID(_AuctionAssetDetailID);
        }
        public bool AddLotInspectionInAsset(AM_AssetInspectionLot _mdlLot)
        {
            return AuctionDAL.AddLotInspectionInAsset(_mdlLot);
        }
        public bool AddIndividualInspectionInAsset(AM_AssetInspectionInd _mdlInd)
        {
            return AuctionDAL.AddIndividualInspectionInAsset(_mdlInd);
        }
        public long GetApprovalAuthorityByAuctionNoticeID(long _AuctionNoticeID)
        {
            return AuctionDAL.GetApprovalAuthorityByAuctionNoticeID(_AuctionNoticeID);
        }
        public bool GetAwardedStatusByBidderID(long _AuctionDetailID)
        {
            return AuctionDAL.GetAwardedStatusByBidderID(_AuctionDetailID);
        }
        public long GetAssetItemIDByAuctionAssetID(long _AuctionAssetID)
        {
            return AuctionDAL.GetAssetItemIDByAuctionAssetID(_AuctionAssetID);
        }
        public bool IfItemsExistsAgainstGroup(long _AuctionAssetID)
        {
            return AuctionDAL.IfItemsExistsAgainstGroup(_AuctionAssetID);
        }
        public bool UpdateAwardStatus(long _AuctionAssetDetailID)
        {
            return AuctionDAL.UpdateAwardStatus(_AuctionAssetDetailID);
        }
        public bool DeleteBiddersforUpdation(long _AuctionBidderID)
        {
            return AuctionDAL.DeleteBiddersforUpdation(_AuctionBidderID);
        }
        public bool DeleteAuctionNoticeAttachmentsforUpdation(long _AuctionNoticeID)
        {
            return AuctionDAL.DeleteAuctionNoticeAttachmentsforUpdation(_AuctionNoticeID);
        }
        public string GetBidderNameByBidderID(long _BidderID)
        {
            return AuctionDAL.GetBidderNameByBidderID(_BidderID);
        }
        public bool SaveAuctionNoticeAttachment(AC_AuctionNoticeAttachment _mdlAttachment)
        {
            return AuctionDAL.SaveAuctionNoticeAttachment(_mdlAttachment);
        }
        public List<string> GetAuctionNoticeAttachment(long _AuctionNoticeID)
        {
            return AuctionDAL.GetAuctionNoticeAttachment(_AuctionNoticeID);
        }
        public string GetAssetTypeByAssetWorkID(long _WorkID)
        {
            return AuctionDAL.GetAssetTypeByAssetWorkID(_WorkID);
        }
        #endregion  


        #region Notification

        public AC_GetPendingApprovalNotifyData_Result GetPendingApprovalNotifyData(long _AuctionNoticeID, long _AuctionAssetsID, long AuctionPriceID)
        {
            return AuctionDAL.GetPendingApprovalNotifyData(_AuctionNoticeID, _AuctionAssetsID, AuctionPriceID);
        }

        public AC_GetApprovedCanceledNotifyData_Result GetApprovedCanceledNotifyData(long _AuctionNoticeID, long _AuctionAssetsID, long AuctionPriceID, long _UserID)
        {
            return AuctionDAL.GetApprovedCanceledNotifyData(_AuctionNoticeID, _AuctionAssetsID,  AuctionPriceID, _UserID);
        }
        public string GetStatusByNoticeAndPriceID(long _AuctionNoticeID, long _AuctionPriceID)
        {
            return AuctionDAL.GetStatusByNoticeAndPriceID(_AuctionNoticeID, _AuctionPriceID);
        }
        public AC_AuctionStatusDetails GetAuctionStatusForSECE(long _AuctionNoticeID, long _AuctionPriceID, int _DesignationID)
        {
            return AuctionDAL.GetAuctionStatusForSECE(_AuctionNoticeID, _AuctionPriceID, _DesignationID);
        }
        public AC_AuctionStatusDetails GetAuctionStatusForXEN(long _AuctionNoticeID, long _AuctionPriceID, int _DesignationID)
        {
            return AuctionDAL.GetAuctionStatusForXEN(_AuctionNoticeID, _AuctionPriceID, _DesignationID);
        }
        #endregion  Notification
    }
}
