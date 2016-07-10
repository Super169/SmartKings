using KingsLib;
using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        private void btnTeter_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            oGA.checkStatus();
            if (!oGA.IsOnline()) return;

            goTester_01(oGA);
            // Thread myThread = new Thread(() => goTester_01(oGA));
            // myThread.Start();
        }

        private void goTester_01(GameAccount oGA)
        {
            string taskId = Scheduler.TaskId.Patrol;
            string taskName = Scheduler.getTaskName(taskId);
            ConnectionInfo ci = oGA.connectionInfo;
            string sid = oGA.sid;
            RequestReturnObject rro;

            string js;
            rro = KingsLib.request.Hero.getPlayerHeroList(ci, sid);
            js = rro.responseText;

            /*
            List<hero> heros = new List<hero>();
            heros.Add(new hero() { idx = 1 });
            heros.Add(new hero() { idx = 2 });
            heros.Add(new hero() { idx = 3 });

            js = JsonConvert.SerializeObject(heros);
            */

            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(HeroList));
            JObject json = JObject.Parse(js);
            IList<string> err;

            bool valid = json.IsValid(schema, out err);

            MessageBox.Show(valid.ToString());

            object data = JsonConvert.DeserializeObject(js );
            HeroList heroList =  JsonConvert.DeserializeObject<HeroList>(js);

            js = JsonConvert.SerializeObject(heroList);
            System.IO.File.WriteAllText(@"js.out", js);

            saveObject01(heroList);

        }

        private void saveObject01(object data)
        {
            string js = JsonConvert.SerializeObject(data);
            HeroList hl = JsonConvert.DeserializeObject<HeroList>(js);
            System.IO.File.WriteAllText(@"js.out", js);
        }

        private class HeroList
        {
            public List<Hero> heros { get; set; }
        }

        private class Hero
        {
            public class Hero_trs
            {
                public int no { get; set; }
            }

            public class Hero_talent
            {
                public string type { get; set; }
                public int level { get; set; }
            }

            public class Hero_talentInfo
            {
                public int point { get; set; }
                public int level { get; set; }
                public List<Hero_talent> talents { get; set; }
            }

            public class Hero_afi
            {
                public string sta { get; set; }
                public string col { get; set; }
                public int lvl { get; set; }
            }

            [JsonProperty("idx", Required = Required.Always)]
            public int idx { get; set; }

            public string nm { get; set; }
            public string dsp { get; set; }
            public int lv { get; set; }
            public int exp { get; set; }
            public int intl { get; set; }
            public int strg { get; set; }
            public int chrm { get; set; }
            public int attk { get; set; }
            public int dfnc { get; set; }
            public int spd { get; set; }
            public int hp { get; set; }
            public int mxHp { get; set; }
            public int mor { get; set; }
            public string army { get; set; }
            public List<int> amftLvs { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public string skill { get; set; }
            public string clr { get; set; }
            public int star { get; set; }
            public int phs { get; set; }
            public List<int> armm { get; set; }

            [JsonProperty("trss", NullValueHandling = NullValueHandling.Ignore)]
            public List<Hero_trs> trss { get; set; }

            public int power { get; set; }
            public int cfd { get; set; }
            public int flags { get; set; }
            public Hero_talentInfo talentInfo { get; set; }
            public Hero_afi afi {get; set;}

        }


    }
}
