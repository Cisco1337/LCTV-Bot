using jabber.client;
using jabber.connection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LCTV_Bot
{
    class Extra
    {
        public class Scraper
        {
            public static string GetSpotifyTrackInfo() 
            {
                var proc = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));

                if (proc == null)
                {
                    return "Spotify is not running!";
                }

                if (string.Equals(proc.MainWindowTitle, "Spotify", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "No track is playing";
                }

                return proc.MainWindowTitle;
            }
        }

        public class IO
        {
            public static string ReadProject()
            {
                if (!File.Exists("project.dat"))
                {
                    File.Create("project.dat").Close();
                    File.WriteAllText("project.dat", "Random thing");
                }
                byte[] contents = File.ReadAllBytes("project.dat");
                return Encoding.UTF8.GetString(contents);
            }

            public static string SetProject(string project)
            {
                if (!File.Exists("project.dat"))
                {
                    File.Create("project.dat").Close();
                }
                File.WriteAllText("project.dat", project);
                byte[] contents = File.ReadAllBytes("project.dat");
                return "Project successfully set to " + Encoding.UTF8.GetString(contents);
            }
        }

        public class LCTV
        {
            public static string Roll(Room room)
            {
                List<RoomParticipant> users = new List<RoomParticipant>();
                foreach (RoomParticipant user in room.Participants)
                {
                    if (user.Nick.ToLower() != Config.Username)
                    {
                        users.Add(user);
                    }
                }
                string winner;
                try
                {
                    winner = "Winner is: @" + users[new Random().Next(0, users.Count - 1)].Nick + "";
                }
                catch (Exception x)
                {
                    winner = "Cannot decide winner! Error: " + Helpers.HandleError(x);
                }
                Console.WriteLine(winner);
                return winner;
            }
        }

        public class Misc
        {
            public static string Joke(string message)
            {
                string firstname = "Chuck";
                string lastname = "Norris";
                message = message.Replace("!joke ", "");
                if (message.Contains(" "))
                {
                    string[] msgs = message.Split(' ');
                    firstname = msgs[0];
                    lastname = msgs[1];
                }
                dynamic response;
                using (WebClient wc = new WebClient())
                {
                    string url = "http://api.icndb.com/jokes/random?firstName=" + firstname + "&lastName=" + lastname + "";
                    response = JsonConvert.DeserializeObject(wc.DownloadString(url));
                }
                string joke = response.value.joke;
                return WebUtility.HtmlDecode(joke);
            }
        }
    }
}
