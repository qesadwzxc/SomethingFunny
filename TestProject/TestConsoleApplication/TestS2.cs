using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Common.Geometry;

namespace TestConsoleApplication
{
    public class TestS2
    {
        public ulong GetCell(double lat, double lng)
        {
            /*第一步：把经纬度转换成弧度。由于经纬度是角度，弧度转角度乘以 π / 180° */
            S2LatLng ll = S2LatLng.FromDegrees(lat, lng);
            //第二步：经纬弧度转换成坐标系上的一个点 S(lat,lng) -> f(x,y,z)  */
            S2Point point = ll.ToPoint();
            //第六步：坐标轴点与希尔伯特曲线 Cell ID 相互转换
            S2CellId cellid = S2CellId.FromPoint(point);
            Console.WriteLine(lat + "," + lng + ":" + cellid.Level);
            return cellid.Id;
        }

        public void TestPoint(double lat, double lng)
        {
            Console.WriteLine(string.Format($"-------------坐标： {lat}，{lng}---------------- "));

            /*第一步：把经纬度转换成弧度。由于经纬度是角度，弧度转角度乘以 π / 180° */
            S2LatLng ll = S2LatLng.FromDegrees(lat, lng);
            Console.WriteLine(string.Format("-------------第一步：把经纬度转换成弧度--------------"));
            Console.WriteLine(string.Format("latr:{0},lngr{1}", ll.LatRadians, ll.LngRadians));

            //第二步：经纬弧度转换成坐标系上的一个点 S(lat,lng) -> f(x,y,z)  */
            S2Point point = ll.ToPoint();
            Console.WriteLine(string.Format("-------------第二步：经纬弧度转换成坐标系上的一个点 S(lat,lng) -> f(x,y,z) --------------"));
            Console.WriteLine(string.Format("f(x,y,z): x:{0},y:{1},z:{2}", point.X, point.Y, point.Z));

            //第三步：进行投影  f(x,y,z) -> g(face,u,v)
            int face = S2Projections.XyzToFace(point);
            R2Vector vector = S2Projections.ValidFaceXyzToUv(face, point);
            Console.WriteLine(string.Format("-------------进行投影  f(x,y,z) -> g(face,u,v) --------------"));
            Console.WriteLine(string.Format("g(face,u,v): {0},{1},{2}", face, vector.X, vector.Y));

            //第四步： 投影面积修正 g(face,u,v) -> h(face,s,t)
            double s = S2Projections.UvToSt(vector.X);
            double t = S2Projections.UvToSt(vector.Y);
            Console.WriteLine(string.Format("-------------第四步： 投影面积修正 g(face,u,v) -> h(face,s,t) --------------"));
            Console.WriteLine(string.Format("h(face,s,t): {0},{1},{2}", face, s, t));

            //第五步：点与坐标轴点相互转换 h(face,s,t) -> H(face,i,j) 
            //int i = S2CellId.StToIj(s);
            //int j = S2CellId.StToIj(t);
            //Console.WriteLine(string.Format("-------------第五步：点与坐标轴点相互转换 h(face,s,t) -> H(face,i,j)  --------------"));
            //Console.WriteLine(string.Format("H(face, i, j): {0},{1},{2}", face, i, j));

            //第六步：坐标轴点与希尔伯特曲线 Cell ID 相互转换
            S2CellId cellid = S2CellId.FromPoint(point);
            Console.WriteLine(string.Format("-------------第六步：坐标轴点与希尔伯特曲线 Cell ID 相互转换  --------------"));
            Console.WriteLine(string.Format("CellID.id: {0},level:{1}", cellid.Id, cellid.Level));

            //验证转坐标
            S2LatLng lan = cellid.ToLatLng();
            Console.WriteLine(string.Format("lat: {0},lng:{1}", lan.LatDegrees, lan.LngDegrees));
        }

        public void GetParentAreaOfPoint(double lat, double lng)
        {
            S2LatLng ll = S2LatLng.FromDegrees(lat, lng);
            S2Point point = ll.ToPoint();
            var cell = new S2CellId(3869613537281197363);
            var parent = cell.ParentForLevel(13);
            Console.WriteLine(cell.Id);
            Console.WriteLine(parent.Level);            
            Console.WriteLine(parent.ChildBegin.Id);
            Console.WriteLine(parent.ChildEnd.Id);
        }

        public double S2LevelToAvgArea(int level)
        {
            //单位:平方公里（km^2）
            double area = 0;
            switch (level)
            {
                case 30: area = 0.0000000000737; break;
                case 29: area = 0.0000000002949; break;
                case 28: area = 0.0000000011798; break;
                case 27: area = 0.0000000047191; break;
                case 26: area = 0.0000000188762; break;
                case 25: area = 0.0000000755049; break;
                case 24: area = 0.0000003020198; break;
                case 23: area = 0.0000012080791; break;
                case 22: area = 0.0000048323166; break;
                case 21: area = 0.0000193292663; break;
                case 20: area = 0.0000773170650; break;
                case 19: area = 0.0003092682600; break;
                case 18: area = 0.0012370730401; break;
                case 17: area = 0.0049482921604; break;
                case 16: area = 0.0197931686416; break;
                case 15: area = 0.0791726745665; break;
                case 14: area = 0.3166906982660; break;
                case 13: area = 1.2667627930641; break;
                case 12: area = 5.0670511722565; break;
                case 11: area = 20.2682046890259; break;
                case 10: area = 81.0728187561035; break;
                case 9: area = 324.2912750244140; break;
                case 8: area = 1297.1651000976600; break;
                case 7: area = 5188.6604003906200; break;
                case 6: area = 20754.6416015625000; break;
                case 5: area = 83018.5664062500000; break;
                case 4: area = 332074.2656250000000; break;
                case 3: area = 1328279.0625000000000; break;
                case 2: area = 5313188.2500000000000; break;
                case 1: area = 21252753.0000000000000; break;
                case 0: area = 85011012.0000000000000; break;
            }
            return area;
        }
    }
}
