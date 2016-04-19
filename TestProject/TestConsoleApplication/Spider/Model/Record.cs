using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinCode.DateBase;
using System.Data.SqlClient;

namespace TestConsoleApplication.Spider
{
    public static class Recond
    {
        public static void AddRecond(this SingleRecord recond)
        {
            try
            {
                string sql = string.Format(@"INSERT INTO [TCOAMessage].[dbo].[Rec_Recond]
                                                    ([RecordId]
                                                    ,[IsSuccess]
                                                    ,[TopicName]
                                                    ,[TopicUrl]
                                                    ,[PictureUrl]
                                                    ,[DownloadTime])
                                                VALUES
                                                    (@RecordId
                                                    ,@IsSuccess
                                                    ,@TopicName
                                                    ,@TopicUrl
                                                    ,@PictureUrl
                                                    ,@DownloadTime)");
                SqlHelper helper = new SqlHelper();
                List<SqlParameter> paramsList = new List<SqlParameter>();
                paramsList.Add(new SqlParameter("@RecordId", recond.RecondId));
                paramsList.Add(new SqlParameter("@IsSuccess", recond.IsSuccess));
                paramsList.Add(new SqlParameter("@TopicName", recond.TopicName));
                paramsList.Add(new SqlParameter("@TopicUrl", recond.TopicUrl));
                paramsList.Add(new SqlParameter("@PictureUrl", recond.PictureUrl));
                paramsList.Add(new SqlParameter("@DownloadTime", recond.DownloadTime));
                helper.Execute(sql, paramsList);
            }
            catch
            { }
        }
    }


    public class SingleRecord
    {
        public Guid RecondId { get; } = Guid.NewGuid();
        public bool IsSuccess { get; set; } = false;
        public string TopicName { get; set; } = string.Empty;
        public string TopicUrl { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public DateTime DownloadTime { get; set; } = DateTime.Parse("1900-1-1");
    }
}
