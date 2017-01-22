using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using System.Linq;

namespace BAXamarinDemo
{
    [Activity(Label = "BAXamarinDemo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {


        BluetoothConnection blueConnection = new BluetoothConnection();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            Button connectButton = FindViewById<Button>(Resource.Id.button1);
            Button disconnectButton = FindViewById<Button>(Resource.Id.button2);
            SeekBar red = FindViewById<SeekBar>(Resource.Id.seekBar1);
            SeekBar green = FindViewById<SeekBar>(Resource.Id.seekBar2);
            SeekBar blue = FindViewById<SeekBar>(Resource.Id.seekBar3);
            TextView connected = FindViewById<TextView>(Resource.Id.textView1);

            Button redButton = FindViewById<Button>(Resource.Id.button7);
            Button greenButton = FindViewById<Button>(Resource.Id.button8);
            Button blueButton = FindViewById<Button>(Resource.Id.button9);
            Button rainbowButton = FindViewById<Button>(Resource.Id.button10);
            BluetoothSocket _socket = null;
            int redint = 0;

            connectButton.Click += delegate
            {
                blueConnection = new BluetoothConnection();
                blueConnection.getAdapter();
                blueConnection.thisAdapter.StartDiscovery();

                try
                {
                    blueConnection.getDevice();
                    blueConnection.thisDevice.SetPairingConfirmation(false);
                    blueConnection.thisDevice.SetPairingConfirmation(true);
                    blueConnection.thisDevice.CreateBond();
                }
                catch (Exception deviceEX)
                {
                }
                blueConnection.thisAdapter.CancelDiscovery();
                _socket = blueConnection.thisDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                blueConnection.thisSocket = _socket;

                //   System.Threading.Thread.Sleep(500);
                try
                {
                    blueConnection.thisSocket.Connect();
                    connected.Text = "Bağlı!";
                    disconnectButton.Enabled = true;
                    connectButton.Enabled = false;
                }
                catch (Exception CloseEX)
                {

                }


            };

            disconnectButton.Click += delegate
            {

                try
                {
                    connectButton.Enabled = true;
                    blueConnection.thisDevice.Dispose();
                    blueConnection.thisSocket.OutputStream.WriteByte(187);
                    blueConnection.thisSocket.OutputStream.Close();
                    blueConnection.thisSocket.Close();
                    blueConnection = new BluetoothConnection();
                    _socket = null;
                    connected.Text = "Disconnected!";
                }
                catch { }
            };

            redButton.Click += delegate
            {
                try
                {
                    blueConnection.thisSocket.OutputStream.WriteByte(250);
                    blueConnection.thisSocket.OutputStream.Close();
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            };

            greenButton.Click += delegate
            {
                try
                {
                    blueConnection.thisSocket.OutputStream.WriteByte(251);
                    blueConnection.thisSocket.OutputStream.Close();
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            };

            blueButton.Click += delegate
            {

                try
                {
                    blueConnection.thisSocket.OutputStream.WriteByte(252);
                    blueConnection.thisSocket.OutputStream.Close();
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            };

            rainbowButton.Click += delegate
            {
                try
                {
                    blueConnection.thisSocket.OutputStream.WriteByte(253);
                    blueConnection.thisSocket.OutputStream.Close();
                    System.Threading.Thread.Sleep(10);
                }
                catch { }
            };
            red.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    try
                    {
                        if (e.Progress % 2 == 0)
                        {
                           
                            blueConnection.thisSocket.OutputStream.WriteByte(Convert.ToByte(e.Progress.ToString()));
                            blueConnection.thisSocket.OutputStream.Close();
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    catch { }
                }
            };
            green.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    try
                    {
                        if (e.Progress % 2 == 0)
                        {
                            blueConnection.thisSocket.OutputStream.WriteByte(Convert.ToByte(e.Progress.ToString()));
                            blueConnection.thisSocket.OutputStream.Close();
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    catch { }
                }
            };
            blue.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    try
                    {
                        if (e.Progress % 2 == 0)
                        {
                             blueConnection.thisSocket.OutputStream.WriteByte(Convert.ToByte(e.Progress.ToString()));
                            blueConnection.thisSocket.OutputStream.WriteByte(Convert.ToByte(e.Progress.ToString()));
                            blueConnection.thisSocket.OutputStream.Close();
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    catch { }
                }
            };
        }



    }




    public class BluetoothConnection
    {
        public void getAdapter() { this.thisAdapter = BluetoothAdapter.DefaultAdapter; }
        public void getDevice() { this.thisDevice = (from bd in this.thisAdapter.BondedDevices where bd.Name == "HC-06" select bd).FirstOrDefault(); }

        public BluetoothAdapter thisAdapter { get; set; }
        public BluetoothDevice thisDevice { get; set; }
        public BluetoothSocket thisSocket { get; set; }
    }
}

