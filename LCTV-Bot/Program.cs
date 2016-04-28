using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jabber;
using jabber.client;
using jabber.protocol.client;
using jabber.connection;

namespace LCTV_Bot
{
    class Program
    {
        static string username = Config.Username;
        static string password = Config.Password;

        static bool DEBUG = true;

        static JabberClient client;
        static Room r;
        static List<Message> messages = new List<Message>();
        static string lastId = "";
        static string BOTNAME = "HighEntropyBot";

        static void Main(string[] args)
        {
            if (DEBUG)
            {
                Console.Title = "[DEBUG] " + BOTNAME + " by Entropy";
            }
            else
            {
                Console.Title = BOTNAME + " by Entropy";
            }
            bool firstrun = true;
            string hostname = "xmpp.livecoding.tv";
            JID jid = new JID(username + "@livecoding.tv");

            client = new JabberClient();
            client.User = jid.User;
            client.Password = password;
            client.Server = jid.Server;
            client.NetworkHost = hostname;
            client.Resource = BOTNAME;
            client.AutoPresence = true;
            client.Connect();
            client.Login();
            client.OnConnect += Client_OnConnect;
            client.OnAuthenticate += Client_OnAuthenticate;
            while (true)
            {
                if (firstrun)
                {
                    Thread.Sleep(5000);
                    firstrun = false;
                }
                else
                {
                    Thread.Sleep(500);
                }
                if (messages.Count > 0)
                {
                    Message latestMsg = messages[messages.Count - 1];
                    if (latestMsg.ID != lastId)
                    {
                        processMessage(latestMsg);
                        if (DEBUG)
                        {
                            Console.WriteLine("{message: \"" + latestMsg.Body + "\", ID: " + latestMsg.ID + "}");
                        }
                    }
                    lastId = latestMsg.ID;
                }
            }
        }

        private static void processMessage(Message message)
        {
            if (message.Body.StartsWith("!"))
            {
                string msg = message.Body.Replace("!", "").Split(' ')[0];
                string response = "";
                switch (msg)
                {
                    case "help":
                        response = "Help: \n!hello - Says hello!\n!help - Shows help message!\n!song - Shows track name I am listening to!\n!project - Tells you current project I am working on!\n!setproject - Sets current project!" + Messages.HostOnly.Value + "\n!roll - Rolls random winner on stream!" + Messages.HostOnly.Value + "";
                        break;
                    case "song":
                        response = "Current song I am listening to is: " + Extra.Scraper.GetSpotifyTrackInfo();
                        break;
                    case "project":
                        response = "Current project I am working on is: " + Extra.IO.ReadProject();
                        break;
                    case "setproject":
                        if (message.From == username + "@chat.livecoding.tv/" + username)
                            response = Extra.IO.SetProject(message.Body.Replace("!setproject ", ""));
                        else
                            response = Messages.NoRights.Value;
                        break;
                    case "hello":
                        response = "Hello, " + message.From.User + "!";
                        break;
                    case "roll":
                        if (message.From == username + "@chat.livecoding.tv/" + username)
                            response = Extra.LCTV.Roll(r);
                        else
                            response = Messages.NoRights.Value;
                        break;
                    case "joke":
                        response = Extra.Misc.Joke(message.Body);
                        break;
                    default:
                        response = "Unknown command '!" + msg + "' !";
                        break;
                }
                response = response.Insert(0, "\n----BOT REPLY: @" + UppercaseFirst(message.From.User) + "----\n" + BOTNAME + " Says: ");
                response = response + "\n----BOT REPLY: @" + UppercaseFirst(message.From.User) + "----";
                r.PublicMessage(response);
                if (DEBUG)
                {
                    Console.WriteLine("\n[RESPONSE START]" + response + "\n[RESPONSE END]\n");
                }
            }
        }

        private static void Client_OnConnect(object sender, StanzaStream stream)
        {
            Console.WriteLine("Connected!\nJID: " + client.JID);
        }

        private static void Client_OnAuthenticate(object sender)
        {
            Console.WriteLine("Authed!\nJID: " + client.JID + "\nServer: " + client.Server);
            JID room = new JID(username, "chat.livecoding.tv", UppercaseFirst(username));
            ConferenceManager manager = new ConferenceManager { Stream = client };
            r = manager.GetRoom(room.User + "@" + room.Server + "/" + room.Resource);
            r.OnJoin += R_OnJoin;
            r.OnRoomMessage += R_OnRoomMessage;
            r.Join();
        }

        private static void R_OnRoomMessage(object sender, Message msg)
        {
            messages.Add(msg);
        }

        private static void R_OnJoin(Room room)
        {
            Console.WriteLine("Joined room!\nRoom: " + room.JID.User);
        }

        private static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
