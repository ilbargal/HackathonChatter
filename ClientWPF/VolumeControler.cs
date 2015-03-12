using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace VolumeControlerForm
{
    /// <summary>
    /// Range is between 1 to 50
    /// </summary>
    class VolumeControler : Form
    {
        private int vol;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);


        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;


        // mute (toggle)
        //SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);


        //// unmute
        //SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
        //SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        public VolumeControler(int vol)
        {
            this.vol = vol;
            DecreaseTimesVol(50);
            IncreaseTimesVol(vol);
        }

        public void ChangeVolume(int newVol)
        {
            if (newVol > vol)
            {
                IncreaseTimesVol(newVol - vol);
            }
            else
            {
                DecreaseTimesVol(vol - newVol);
            }
        }

        public void Mute()
        {
            SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
        public void Unmute()
        {
            SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
        private void DecreaseVol()
        {
            SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }
        private void IncreaseVol()
        {
            SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
        }
        private void DecreaseTimesVol(int times)
        {
            for(int i=0;i<times;i++)
            {
                DecreaseVol();
            }
        }
        private void IncreaseTimesVol(int times)
        {
            for (int i = 0; i < times; i++)
            {
                IncreaseVol();
            }
        }
    }
}