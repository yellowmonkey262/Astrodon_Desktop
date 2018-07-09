using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Astrodon.Classes {

    public class CommClient {
        private Thread tcpThread;      // Receiver
        private bool _conn = false;    // Is connected/connecting?
        private bool _logged = false;  // Is logged in?
        private string _user;          // Username
        private string _pass;          // Password
        private bool reg;              // Register mode
        private System.Timers.Timer timer;

        // public string Server { get { return "localhost"; } }  // Address of server. In this case - local IP address. - 10.0.1.10
        public string Server {
            get {
                String ip = "10.0.1.10";//);//(Environment.MachineName == "STEPHEN-PC" ? "localhost" :
                return ip;
            }
        }  // Address of server. In this case - local IP address. - 10.0.1.10

        public CommClient() {
        }

        public int Port { get { return 2000; } }

        public bool IsLoggedIn { get { return _logged; } }

        public string UserName { get { return _user; } }

        public string Password { get { return _pass; } }

        private TcpClient client;
        private NetworkStream netStream;
        private BinaryReader br;
        private BinaryWriter bw;

        // Start connection thread and login or register.
        private void connect(string user, string password, bool register) {
            if (!_conn) {
                //_conn = true;
                _user = user;
                _pass = password;
                reg = register;
                tcpThread = new Thread(new ThreadStart(SetupConn));
                tcpThread.Start();
            }
        }

        public void Login(string user, string password) {
            connect(user, password, false);
        }

        public void Disconnect() {
            if (_conn)
                CloseConn();
        }

        public void SendMessage(string msg) {
            if (_conn) {
                bw.Write(IM_Send);
                bw.Write(msg);
                bw.Flush();
            }
        }

        // Events
        public event EventHandler LoginOK;

        public event IMErrorEventHandler LoginFailed;

        public event EventHandler Disconnected;

        public event IMReceivedEventHandler MessageReceived;

        virtual protected void OnLoginOK() {
            if (LoginOK != null) { LoginOK(this, EventArgs.Empty); }
        }

        virtual protected void OnLoginFailed(IMErrorEventArgs e) {
            if (LoginFailed != null) { LoginFailed(this, e); }
        }

        virtual protected void OnDisconnected() {
            if (Disconnected != null) { Disconnected(this, EventArgs.Empty); }
        }

        virtual protected void OnMessageReceived(IMReceivedEventArgs e) {
            if (MessageReceived != null) { MessageReceived(this, e); }
        }

        private void SetupConn() {
            try {
                client = new TcpClient(Server, Port);  // Connect to the server.
                netStream = client.GetStream();
                br = new BinaryReader(netStream, Encoding.UTF8);
                bw = new BinaryWriter(netStream, Encoding.UTF8);

                // Receive "hello"
                int hello = br.ReadInt32();
                if (hello == IM_Hello) {
                    // Hello OK, so answer.
                    bw.Write(IM_Hello);

                    bw.Write(IM_Login);  // Login or register
                    bw.Write(UserName);
                    bw.Write(Password);
                    bw.Flush();

                    byte ans = br.ReadByte();  // Read answer.
                    if (ans == IM_OK) {
                        _conn = true;
                        OnLoginOK();  // Login is OK (when registered, automatically logged in)
                        Heartbeat();
                        Receiver(); // Time for listening for incoming messages.
                    } else {
                        IMErrorEventArgs err = new IMErrorEventArgs((IMError)ans);
                        OnLoginFailed(err);
                    }
                }
                if (_conn)
                    CloseConn();
            } catch (Exception ex) { Controller.HandleError(ex); }
        }

        private void Heartbeat() {
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (client.Connected) {
                bw.Write(IM_Heartbeat);
            } else {
                client = new TcpClient(Server, Port);  // Connect to the server.
                bw.Write(IM_Heartbeat);
            }
        }

        private void CloseConn() {
            timer.Enabled = false;
            br.Close();
            bw.Close();
            netStream.Close();
            client.Close();
            OnDisconnected();
            _conn = false;
        }

        private void Receiver() {
            _logged = true;

            try {
                while (client.Connected) {
                    byte type = br.ReadByte();  // Get incoming packet type.
                    if (type == IM_Received) {
                        string msg = br.ReadString();
                        OnMessageReceived(new IMReceivedEventArgs(msg));
                    }
                }
            } catch (IOException) {  }

            _logged = false;
        }

        // Packet types
        public const int IM_Hello = 2012;      // Hello

        public const byte IM_OK = 0;           // OK
        public const byte IM_Login = 1;        // Login
        public const byte IM_Register = 2;     // Register
        public const byte IM_TooUsername = 3;  // Too long username
        public const byte IM_TooPassword = 4;  // Too long password
        public const byte IM_Exists = 5;       // Already exists
        public const byte IM_NoExists = 6;     // Doesn't exist
        public const byte IM_WrongPass = 7;    // Wrong password
        public const byte IM_IsAvailable = 8;  // Is user available?
        public const byte IM_Send = 9;         // Send message
        public const byte IM_Received = 10;    // Message received
        public const byte IM_Heartbeat = 11;    // Heartbeat
    }
}