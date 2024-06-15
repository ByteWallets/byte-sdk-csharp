using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Byte.Core.Constant;
using Byte.Core.Http.Client;
using static com.sun.jndi.cosnaming.IiopUrl;

namespace Byte.Core.Http
{
    public class ByteService
    {
        private ByteClient byteClient;
        private string host;
        private List<string> supportedCoins;

        public bool IsSupportedCoin(string coinName)
        {
            return supportedCoins != null && supportedCoins.Contains(coinName);
        }

        /**
         * 创建币种地址
         * @param coinType
         * @return
         */
        public Address CreateCoinAddress(CoinType coinType)
        {
            string callbackUrl = host + "/bipay/notify";
            try
            {
                ResponseMessage<Address> resp = byteClient.CreateCoinAddress(coinType.code, callbackUrl);
                return resp.data;
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
            return null;
        }

        public ResponseMessage<string> transfer(string orderId, BigDecimal amount, CoinType coinType, string subCoinType, string address, string memo)
        {
            string callbackUrl = host + "/bipay/notify";
            try
            {
                ResponseMessage<string> resp = biPayClient.transfer(orderId, amount, coinType.getCode(), subCoinType, address, callbackUrl, memo);
                return resp;
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
            return ResponseMessage.error("提交转币失败");
        }

        public ResponseMessage<string> autoTransfer(string orderId, BigDecimal amount, CoinType coinType, string subCoinType, string address, string memo)
        {
            string callbackUrl = host + "/bipay/notify";
            try
            {
                ResponseMessage<string> resp = biPayClient.autoTransfer(orderId, amount, coinType.getCode(), subCoinType, address, callbackUrl, memo);
                return resp;
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
            return ResponseMessage.error("提交转币失败");
        }

        public List<Transaction> queryTransaction() throws Exception
        {
        return biPayClient.queryTransaction("","","",null,"","","");
    }
}
}
