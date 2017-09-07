using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace 图片转ASCII字符
{
    class Program
    {
        private static readonly string charset = "MNHQ&OC?7>!:-;.";
        static Bitmap GetBitMap()
        {
            string path= @"C:\Users\PD\Desktop\timg (1).jpg";
            Bitmap map = new Bitmap(path);
            return map;
        }

        static int[,] RGB2Gray(Bitmap map)
        {
            int[,] gray = new int[map.Width , map.Height];
            for(int i=0;i<map.Width;i++)
            {
                for(int j=0;j<map.Height;j++)
                {
                    Color color = map.GetPixel(i , j);
                    gray[i , j] = (int)(color.R * 0.299 + color.G * 0.578 + color.B * 0.114);
                }
            }
            return gray;
        }
        //采样
        static int[,] Sample(int[,] gray)
        {
            int widthcell = 8;//方块宽
            int heightcell = 12;//方块边长

            int row = gray.GetLength(0);
            int line = gray.GetLength(1);

            int[,] resGray = new int[(int)Math.Ceiling(row / (double)widthcell) , (int)Math.Ceiling(line / (double)heightcell)];
            int resRow = resGray.GetLength(0);
            int resLine = resGray.GetLength(1);
            for(int i=0;i<resRow;i++)
            {
                for(int j=0;j<resLine;j++)
                {//遍历所有的块
                    int sum = 0;
                    int pluscount = 0;

                    for(int indexi=0;indexi< widthcell; indexi++)
                    {
                        for(int indexj=0;indexj< heightcell; indexj++)
                        {
                            int rowindex = i * widthcell + indexi;
                            int lineindex = j * heightcell + indexj;
                            if( rowindex < row && lineindex < line)
                            {
                                sum += gray[rowindex , lineindex];
                                pluscount++;
                            }
                        }
                    }
                    int avage = (int)Math.Round(sum / (double)pluscount);
                    resGray[i , j] = avage;
                }
            }
            return resGray;
        }
        static string GetStringByGray(int[,] gray)
        {
            int row = gray.GetLength(0);
            int line = gray.GetLength(1);

            StringBuilder sb = new StringBuilder(row * line + row - 1);
            StringBuilder testsb = new StringBuilder(row * line);

            for(int i = 0; i < line; i++)
            {
                for(int j = 0; j < row; j++)
                {
                    float x = 255f / charset.Length;
                    float tmpavgGray = gray[j , i] / x;
                    int avgGray = (int)Math.Round(tmpavgGray);

                    if(--avgGray < 0)
                        avgGray = 0;
                    sb.Append(charset[avgGray]);
                }
                if(i != row - 1)
                {
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
        static void Main(string[] args)
        {
            Bitmap map = GetBitMap();//取得位图
            Console.WriteLine("Width:" + map.Width);
            Console.WriteLine("Height:" + map.Height);
            int[,] gray = RGB2Gray(map);//取得灰度图
            gray = Sample(gray);//采样
            string context = GetStringByGray(gray);//取得对应的字符
            WriteInFile(context);//写入磁盘

            //Console.ReadKey();
        }

        static void WriteInFile(string context)
        {
            File.WriteAllText(@"C:\Users\PD\Desktop\testPic.txt",context);
            Console.WriteLine("Success!!");
        }
    }
}
