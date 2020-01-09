using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinCode;

namespace TestConsoleApplication
{
    public class TestSalesman
    {
        private string ImgLocalPath
        {
            get
            {
                var baseAddress = AppDomain.CurrentDomain.BaseDirectory;
                return $"{baseAddress}";
            }
        }

        public DataTable GetData()
        {
            string path = ImgLocalPath + "\\head";
            //第二种方法
            DirectoryInfo folder = new DirectoryInfo(path);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Department");
            dataTable.Columns.Add("SerialNumber");
            dataTable.Columns.Add("FileName");
            int errorCount = 0;
            foreach (FileInfo file in folder.GetFiles())
            {
                Console.WriteLine(file.FullName);
                string fileName = file.Name.Replace(".jpg", "").Replace(".JPG", "").Replace(".jpeg", "").Replace(".png", "");
                if (fileName.Length >= 12)
                {
                    string mobilePhone = fileName.Substring(fileName.Length - 11, 11);
                    string name = fileName.Replace(mobilePhone, "");
                    VinCode.DateBase.MySqlHelper conn = new VinCode.DateBase.MySqlHelper();
                    DataTable salesmanInfo = conn.ExecuteReader($@"SELECT a.ID,b.SerialNumber,a.Department FROM caixin021.salesman a
                            inner join caixin021.merchant b on b.ID = a.MerchantId and(b.DisabledLevel is null or b.DisabledLevel = '' or b.DisabledLevel = '0')
                            where a.IsDeleted = 0 and a.MobilePhone = '{mobilePhone}' and department!=''");
                    if (salesmanInfo != null && salesmanInfo.Rows.Count == 1)
                    {
                        DataRow row = salesmanInfo.Rows[0];
                        if (row["SerialNumber"] != DBNull.Value && !string.IsNullOrEmpty(row["SerialNumber"].ToString()))
                        {
                            if (GenerateQRCodeImg(row["ID"].ToString(), fileName))
                            {
                                dataTable.Rows.Add(name, row["Department"], row["SerialNumber"], file.Name);
                            }
                            else
                            {
                                LogHelper.Write("qrcode", fileName + ":生成二维码图片失败！");
                                errorCount++;
                            }
                        }
                        else
                        {
                            LogHelper.Write("qrcode", fileName + ":未找到用户对应的商户信息！");
                            errorCount++;
                        }
                    }
                    else
                    {
                        LogHelper.Write("qrcode", fileName+ ":未找到用户信息！");
                        errorCount++;
                    }
                }
                else
                {
                    LogHelper.Write("qrcode", fileName + ":文件名格式错误！");
                    errorCount++;
                }
            }
            Console.WriteLine("完成");
            return dataTable;
        }

        private bool GenerateQRCodeImg(string shortCode, string fileName)
        {
            try
            {
                //图片路径
                var imgPath = $"{ImgLocalPath}\\qrcode\\{fileName}.jpg";

                if (File.Exists(imgPath))
                {
                    return true;
                }

                //获取支付连接写进去
                var urlCode = "https://www.caixin021.com/Salesman/SalesmanInfo?qrCode=" + shortCode;

                //否则生成一遍
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(urlCode, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, false);

                var dir = Path.GetDirectoryName(imgPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                //var img = this.GenerateImage(qrCodeImage, shortCode);

                //图片加工
                qrCodeImage.Save(imgPath);

                qrCodeImage.Dispose();//主动释放掉

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
