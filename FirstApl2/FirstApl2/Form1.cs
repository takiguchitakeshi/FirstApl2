﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace FirstApl2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            // サーバーのIPアドレス（または、ホスト名）とポート番号
            string ipOrHost = "127.0.0.1";
            //            long ipOrHost = "127.0.0.1";
            int port = 1024;

            byte[] byteIpAddress = { 192,168,1,6 };
            byte[] localIpAddress = { 192,168,1,6 };

            try
            {
                //            System.Net.IPAddress ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0];
                System.Net.IPAddress ipAddress = new System.Net.IPAddress(byteIpAddress);
                System.Net.IPAddress ipAddress2 = new System.Net.IPAddress(localIpAddress);

                System.Net.IPEndPoint ipLocalEndPoint = new System.Net.IPEndPoint(ipAddress2, 60000);
                System.Net.IPEndPoint ipRemotePoint = new System.Net.IPEndPoint(ipAddress, 1024);
                System.Net.Sockets.TcpClient tcpClientA = new System.Net.Sockets.TcpClient(ipLocalEndPoint);
                tcpClientA.Connect(ipRemotePoint);
                //            tcpClientA.Connect(ipLocalEndPoint);

                //            System.Net.Sockets.TcpClient tcp = new System.Net.Sockets.TcpClient(ipOrHost, port);
                //            System.Net.Sockets.TcpClient tcp = new System.Net.Sockets.TcpClient(ipOrHost, port);

                // NetworkStreamを取得する
                //            System.Net.Sockets.NetworkStream ns = tcp.GetStream();
                System.Net.Sockets.NetworkStream ns = tcpClientA.GetStream();

                //
                ns.ReadTimeout = 10000;
                ns.WriteTimeout = 10000;


                string sendMsg = this.textBox1.Text;

                // サーバーにデータを送信する
                // 文字列をbyte型配列に変換
                System.Text.Encoding enc = System.Text.Encoding.UTF8;

                //            byte[] sendBytes = enc.GetBytes(sendMsg + '\n');
                //            byte[] sendBytes = { 0x01, 0x02, 0x03, 0x04 };

                byte[] sendBytes = new byte[sendMsg.Length];

                for (int i = 0; i < sendMsg.Length; i++)
                {
                    sendBytes[i] = (byte)sendMsg[i];
                }

                sendBytes[0] = 0x00;
                sendBytes[1] = 0x01;

                byte[] buf = Encoding.ASCII.GetBytes(sendMsg);

                byte[] buf2 = FromHexString(sendMsg); ;
                //            buf2[0] = BitConverter.ToSingle(buf[0], 0);

                // データを送信する
                //            ns.Write(sendBytes, 0, sendBytes.Length);
                ns.Write(buf2, 0, buf2.Length);


                // コミットテスト

                // 閉じる
                ns.Close();
                //            tcp.Close();
                tcpClientA.Close();

            }
            catch (Exception)
            {
                textBox1.Text = "エラーが発生しました";
            }
            finally
            {



            }



        }

        /// <summary>
        /// 16進数の文字列からバイト配列を生成します。
        /// </summary>
        /// <param name="str">16進数文字列</param>
        /// <returns>バイト配列</returns>
        public static byte[] FromHexString(string str)
        {
            int length = str.Length / 2;
            byte[] bytes = new byte[length];
            int j = 0;
            for (int i = 0; i < length; i++)
            {
                bytes[i] = Convert.ToByte(str.Substring(j, 2), 16);
                j += 2;
            }
            return bytes;
        }
    }
}
