﻿
using java.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Byte.Core.Constant;
using Byte.Core.Entity;
using Byte.Core.Http;
using Byte.Core.Http.Client;

namespace Byte.FormDemo.Api
{
    public class RequestControllerBase
    {

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
                    string gateway = System.Configuration.ConfigurationManager.AppSettings["Gateway"].ToString();
                    string merchantId = System.Configuration.ConfigurationManager.AppSettings["MerchantId"].ToString();
                    string merchantKey = System.Configuration.ConfigurationManager.AppSettings["MerchantKey"].ToString();
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
                    _callBackUrl = System.Configuration.ConfigurationManager.AppSettings["CallBackUrl"].ToString();
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
        public Address CreateCoinAddress(int coinType, string callBackUrl, string alias, string walletId)
        {
            //不使用codeof
            //return byteClient.CreateCoinAddress(CoinOperateBase.CodeOf(coinType).code.ToString(), GetCallBackUrl(callBackUrl), alias, walletId).data;
            return byteClient.CreateCoinAddress(coinType.ToString(), GetCallBackUrl(callBackUrl), alias, walletId).data;
        }

        /**
         * 发起转账请求
         * @param coinType
         * @param amount
         * @param address
         * @return
         */
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
        public ResponseMessage<string> AutoTransfer(string mainCoinType, string subCoinType, string amount, string address, string callBackUrl, string memo, string remark, string walletId)
        {
            string orderId = Calendar.getInstance().getTimeInMillis().ToString();
            ResponseMessage<string> resp = byteClient.AutoTransfer(orderId, amount, mainCoinType, subCoinType, address, GetCallBackUrl(callBackUrl), memo, remark, walletId);
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
        public string CallBack(Trade trade, bool error)
        {
            string orderId = Calendar.getInstance().getTimeInMillis().ToString();
            string resp = byteClient.OperateCallBaceResult(trade, callBackUrl, error);
            return resp;
        }

        /// <summary>
        /// 获取支持的币种及资金情况
        /// </summary>
        /// <returns></returns>
        public List<SupportCoin> GetSupportCoin()
        {
            return byteClient.GetSupportCoin(true);
        }

        /// <summary>
        /// 验证是否是内部地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool CheckAddress(string mainCoinType, string address)
        {
            return byteClient.CheckAddress(mainCoinType, address);
        }

        ///// <summary>
        ///// 接口弃用了
        ///// </summary>
        ///// <param name="mainCoinType"></param>
        ///// <param name="coinType"></param>
        ///// <param name="tradeId"></param>
        ///// <param name="tradeType"></param>
        ///// <param name="address"></param>
        ///// <param name="startTimestamp"></param>
        ///// <param name="endTimestamp"></param>
        ///// <returns></returns>
        //public List<Transaction> QueryTransaction(string mainCoinType, string coinType, string tradeId, int tradeType, string address, string startTimestamp, string endTimestamp)
        //{
        //    return byteClient.QueryTransaction(mainCoinType, coinType, tradeId, tradeType, address, startTimestamp, endTimestamp);
        //}
    }
}
