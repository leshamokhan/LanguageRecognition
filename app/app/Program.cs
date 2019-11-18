using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace app
{
    public class Json
    {
        public int code { get; set; }
        public string lang { get; set; }
    }


    public class Word
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double En { get; set; }
        public double Bg { get; set; }
        public double Es { get; set; }
        public double Pt { get; set; }
        public double Ru { get; set; }
    }



    public class Program
    {
        public static List<Word> words = new List<Word>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Введите предложение :");
                //string text = "Bсеки човек има право на образование. My friends have come too late. Когда мама проснется, тогда и будем завтракать. Yo leo un libro todas las semanas. Y a veces, leo dos libros. A educação deve ser gratuita, pelo menos a correspondente ao ensino elementar fundamental.";
                string text = Console.ReadLine();
                string[] word = text.Split(new char[] { ' ', ',', '.', '-', '!', '?', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in word)
                {
                    Console.WriteLine(s.ToString());
                    DetectFive(s);
                }

                foreach (Word w in words)
                {
                    Console.WriteLine($"Слово - {w.Name}\t\t\t pt:{w.Pt}\t\t\t ru:{w.Ru}\t\t\t es:{w.Es}\t\t\t bg:{w.Bg}\t\t\t en:{w.En}\t");
                }
            }
            //Console.ReadKey();
        }


        static void DetectFive(string txt)
        {
            string[] lang = new string[6] { "", "pt", "es", "bg", "en", "ru" };

            int countPT = 0;
            int countES = 0;
            int countBG = 0;
            int countEN = 0;
            int countRU = 0;

            for (int i = 0; i < lang.Length; i++)
            {
                string lan = Detect(txt, lang[i]);

                if ("pt" == lan)
                {
                    countPT++;
                }
                if ("es" == lan)
                {
                    countES++;
                }
                if ("bg" == lan)
                {
                    countBG++;
                }
                if ("en" == lan)
                {
                    countEN++;
                }
                if ("ru" == lan)
                {
                    countRU++;
                }
            }
            Console.WriteLine($"pt = {100.0 / 6 * countPT} % \nes = {100.0 / 6 * countES} % \nbg = {100.0 / 6 * countBG} % \nen = {100.0 / 6 * countEN} % \nru = {100.0 / 6 * countRU} % ");

            words.Add(new Word() { Name = txt, Pt = Math.Round(100.0 / 6 * countPT,2), Es = Math.Round(100.0 / 6 * countES,2), Bg = Math.Round(100.0 / 6 * countBG,2), En = Math.Round(100.0 / 6 * countEN,2), Ru = Math.Round(100.0 / 6 * countRU,2) });
        }



        static string Detect(string txt, string lang)
        {
            string host = "https://translate.yandex.net/api/v1.5/tr.json/detect";
            string key = "&key=trnsl.1.1.20190531T164158Z.eaca9df6f9550378.f009cfe3c9e6efc28492b76e6a3fd5ba074344e6";
            string text = "&text=";
            string hint = "?hint=";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Set the method to POST
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(host + hint + lang + text + txt + key);
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                Json j = JsonConvert.DeserializeObject<Json>(jsonResponse);
                lang = j.lang;
                return lang;
            }
        }


        static string PrettyPrint(string s)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s), Formatting.Indented);
        }
    }
}

