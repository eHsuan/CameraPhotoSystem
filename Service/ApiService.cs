using System;
using System.Net.Http;
using System.Threading.Tasks;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Service
{
    public class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<bool> CheckPassStatusAsync(string dataMatrix)
        {
            if (string.IsNullOrWhiteSpace(dataMatrix)) return false;

            try
            {
                // TODO: 這裡替換為實際的 API URL
                // string url = string.Format("http://your-factory-api/check?dm={0}", dataMatrix);
                // var response = await _httpClient.GetAsync(url);
                // if (response.IsSuccessStatusCode)
                // {
                //     var result = await response.Content.ReadAsStringAsync();
                //     return result.ToUpper().Contains("PASS");
                // }

                // 模擬 API 呼叫：如果長度 > 5 且不包含 "FAIL" 則回傳 PASS
                await Task.Delay(500); // 模擬網路延遲
                bool isPass = dataMatrix.Length > 5 && !dataMatrix.ToUpper().Contains("FAIL");
                
                Logger.Info(string.Format("API 檢查 DM: {0}, 結果: {1}", dataMatrix, (isPass ? "PASS" : "FAIL")));
                return isPass;
            }
            catch (Exception ex)
            {
                Logger.Error("API 呼叫失敗", ex);
                return false;
            }
        }
    }
}
