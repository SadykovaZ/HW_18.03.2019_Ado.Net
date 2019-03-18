using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HW_18._03._2019
{
    class URLDto
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public int NCount { get; set; }
    }
    class Repo
    {
        string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=URLDB;Data Source=DESKTOP-O9KC10I";

        public IEnumerable<URLDto> GetOnlyURL()
        {
            ICollection<URLDto> url = new List<URLDto>();
            string executeSql = "SELECT * FROM [dbo].[URLAddress]";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(executeSql, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                object Url = reader.GetValue(1);
                url.Add(new URLDto()
                {
                    URL = Url.ToString()
                });
            }
            connection.Close();
            return url;
        }
        public IEnumerable<URLDto> GetCount()
        {
            ICollection<URLDto> url = new List<URLDto>();
            string executeSql = "SELECT * FROM [dbo].[URLAddress]";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(executeSql, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                object NCount = reader.GetValue(2);

                url.Add(new URLDto()
                {
                    NCount = Int32.Parse(NCount.ToString())
                });
            }
            connection.Close();
            return url;

        }
        public void UpdateCount(int count)
        {
            string executeSql = "UPDATE [dbo].[URLAddress]" + $" set NCount = {count}";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(executeSql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    class Program
    {
        public static void charRnd()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            Console.WriteLine(finalString);
        }       

        public static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Что вы хотите сделать?");
            Console.WriteLine("1. Получить короткую URL ссылку");
            Console.WriteLine("2. Получить полную URL ссылку");

            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    {
                        Repo r = new Repo();
                        var NC = r.GetCount();
                        foreach (URLDto item in NC)
                        {
                            if (item.NCount == 0)
                            {
                                Console.WriteLine("Ссылкой можно воспользоваться только 5 раз");
                            }
                            Console.WriteLine("Количество попыток = {0}", item.NCount);
                            int count1 = item.NCount;
                            while (count1 > 0)
                            {
                                count1--;
                                r.UpdateCount(count1);
                                //Хэширование MD5

                                //Repo r1 = new Repo();
                                //var url = r1.GetOnlyURL();
                                //foreach (URLDto i in url)
                                //{
                                //    Console.WriteLine(GetHash(i.URL.ToString()));
                                //}

                                //С помощью char
                                charRnd();
                                Console.WriteLine("Еще раз? д/н");
                                string c = Console.ReadLine();
                                if (c == "н")
                                {
                                    break;
                                }

                            }
                        }
                    }
                    break;
                case 2:
                    {
                        Repo r = new Repo();
                        var url = r.GetOnlyURL();

                        foreach (URLDto item in url)
                        {
                            Console.WriteLine(item.URL);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
