﻿using java.math;
using java.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Byte.Core.Constant;
using Byte.Core.Entity;
using Byte.Core.Http;
using Byte.Core.Http.Client;
using Byte.Core.Utils;

namespace Byte.Api.Controller
{
    /// <summary>
    /// RequestController 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://byteapi.io/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(true)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class RequestController : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        ByteClient _byteClient;
        ByteClient byteClient
        {
            get
            {
                if (_byteClient == null)
                {
                    string gateway = System.Web.Configuration.WebConfigurationManager.AppSettings["Gateway"].ToString();
                    string merchantId = System.Web.Configuration.WebConfigurationManager.AppSettings["MerchantId"].ToString();
                    string merchantKey = System.Web.Configuration.WebConfigurationManager.AppSettings["MerchantKey"].ToString();
                    _byteClient = new ByteClient(gateway, merchantId, merchantKey);
                }
                return _byteClient;
            }
        }

        log4net.ILog _logger;
        log4net.ILog logger
        {
            get
            {
                if (_logger == null)
                    _logger = log4net.LogManager.GetLogger(this.GetType());
                return _logger;
            }
        }

        string _callBackUrl;
        string callBackUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_callBackUrl))
                    _callBackUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["CallBackUrl"].ToString();
                //_callBackUrl = System.Configuration.ConfigurationManager.AppSettings["CallBackUrl"];
                return _callBackUrl;
            }
        }

        private string GetCallBackUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return this.callBackUrl;
            else
                return url;
        }

        /**
        * 创建新地址
        * @param coinType
        * @return
        */
        [WebMethod]
        public Address CreateCoinAddress(int coinType, string callBackUrl, string alias, string walletId)
        {
            //不使用codeof
            return byteClient.CreateCoinAddress(coinType.ToString(), GetCallBackUrl(callBackUrl), alias, walletId).data;
            //return byteClient.CreateCoinAddress(CoinOperateBase.CodeOf(coinType).code.ToString(), GetCallBackUrl(callBackUrl), alias, walletId).data;
        }

        /**
         * 发起转账请求
         * @param coinType
         * @param amount
         * @param address
         * @return
         */
        [WebMethod]
        public ResponseMessage<string> Transfer(string mainCoinType, string subCoinType, string amount, string address, string callBackUrl, string memo, string remark, string walletId)
        {
            string orderId = Calendar.getInstance().getTimeInMillis().ToString();
            ResponseMessage<string> resp = byteClient.TransferAmt(orderId, amount, mainCoinType, subCoinType, address, GetCallBackUrl(callBackUrl), memo, remark, walletId);
            if (resp.code == 200)
                resp.data = orderId.ToString();
            return resp;
        }

        /**
         * 代付
         * @param coinType
         * @param amount
         * @param address
         * @return
         */
        [WebMethod]
        public ResponseMessage<string> AutoTransfer(string mainCoinType, string subCoinType, string amount, string address, string callBackUrl, string memo, string remark, string walletId)
        {
            string orderId = Calendar.getInstance().getTimeInMillis().ToString();
            ResponseMessage<string> resp = byteClient.AutoTransfer(orderId, amount, mainCoinType, subCoinType, address, GetCallBackUrl(callBackUrl), memo, remark, walletId);
            if (resp.code == 200)
                resp.data = orderId.ToString();
            return resp;
        }

        /// <summary>
        /// 获取支持的币种及资金情况
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<SupportCoin> GetSupportCoin()
        {
            return byteClient.GetSupportCoin(true);
        }

        /// <summary>
        /// 验证是否合法
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [WebMethod]
        public bool CheckAddress(string mainCoinType, string address)
        {
            return byteClient.CheckAddress(mainCoinType, address);
        }

        ///// <summary>
        ///// 查询
        ///// </summary>
        ///// <param name="mainCoinType"></param>
        ///// <param name="coinType"></param>
        ///// <param name="tradeId"></param>
        ///// <param name="tradeType"></param>
        ///// <param name="address"></param>
        ///// <param name="startTimestamp"></param>
        ///// <param name="endTimestamp"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public List<Transaction> QueryTransaction(string mainCoinType, string coinType, string tradeId, int tradeType, string address, string startTimestamp, string endTimestamp)
        //{
        //    return byteClient.QueryTransaction(mainCoinType, coinType, tradeId, tradeType, address, startTimestamp, endTimestamp);
        //}
    }
}
