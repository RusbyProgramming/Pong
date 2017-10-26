using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PongClient
{
    class Game
    {
        //State of game
        public State CurrentState { get; private set; }

        private TcpClient client;
        private byte[] clientBuffer;
        private Graphics frmGraphics;

        public Game(ref Bitmap g)
        {
            //Set state to first state
            this.CurrentState = State.Connecting;

            //Set frmGraphics to the forms bitmap object which allows for writing to the screen
            this.frmGraphics = Graphics.FromImage(g);
        }

        public void Connect(int x, int y)
        {
            //Draw connecting to form
            this.frmGraphics.DrawString("Connecting...", new Font(FontFamily.GenericSansSerif, 40, FontStyle.Regular), Brushes.White, (x / 4), (y / 3));

            //Begin connecting to server
            //this.client = new TcpClient();
            //this.client.ConnectAsync("127.0.0.1", 8122).Wait();

            //Start connection thread
            //new Thread(this.beginRead).Start();
        }

        private void beginRead()
        {
            try
            {
                //Set the size of client buffer to 6 bytes and start read. 2 = Packet ID, 4 = Size of packet
                this.clientBuffer = new byte[6];
                this.client.GetStream().BeginRead(this.clientBuffer, 0, this.clientBuffer.Length, this.readPacket, null);
            }
            catch(Exception ex)
            {
                
            }
        }

        private void readPacket(IAsyncResult ar)
        {
            //Check if length is correct
            int read = this.client.GetStream().EndRead(ar);
            if (read < this.clientBuffer.Length)
                throw new Exception();

            short packetID = 0;
            int packetSize = 0;

            //Read memory from buffer stream
            using (BinaryReader br = new BinaryReader(new MemoryStream(this.clientBuffer)))
            {
                packetID = br.ReadInt16();
                packetSize = br.ReadInt32();
            }

            //Get prepared for packet and read
            this.clientBuffer = new byte[packetSize];
            this.client.GetStream().ReadAsync(this.clientBuffer, 0, packetSize).Wait();


        }

        //Game states
        public enum State
        {
            Connecting,
            WaitingOtherPlayer,
            Started,
            Ended,
        }
    }
}
