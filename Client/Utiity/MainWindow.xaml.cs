﻿/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // RealSense
        PXCMSenseManager psm;
        PXCMTouchlessController ptc;

        // Scrolling Feature
        ScrollViewer myListscrollViwer;
        double initialScrollPoint;
        double initialScrollOffest;
        const double scrollSensitivity = 10f;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartRealSense();

            UpdateConfiguration();

            StartFrameLoop();

            myListscrollViwer = GetScrollViewer(MyList) as ScrollViewer;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopRealSense();
        }

        private void StartRealSense()
        {
            Console.WriteLine("Starting Touchless Controller");

            pxcmStatus rc;

            // creating Sense Manager
            psm = PXCMSenseManager.CreateInstance();
            Console.WriteLine("Creating SenseManager: " + psm == null ? "failed" : "success");
            if (psm == null)
                Environment.Exit(-1);

            // Optional steps to send feedback to Intel Corporation to understand how often each SDK sample is used.
            PXCMMetadata md = psm.QuerySession().QueryInstance<PXCMMetadata>();
            if (md != null)
            {
                string sample_name = "Touchless Listbox CS";
                md.AttachBuffer(1297303632, System.Text.Encoding.Unicode.GetBytes(sample_name));
            }

            // work from file if a filename is given as command line argument
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                psm.captureManager.SetFileName(args[1], false);
            }

            // Enable touchless controller in the multimodal pipeline
            rc = psm.EnableTouchlessController(null);
            Console.WriteLine("Enabling Touchless Controller: " + rc.ToString());
            if (rc != pxcmStatus.PXCM_STATUS_NO_ERROR)
                Environment.Exit(-1);

            // initialize the pipeline
            PXCMSenseManager.Handler handler = new PXCMSenseManager.Handler();
            rc = psm.Init(handler);
            Console.WriteLine("Initializing the pipeline: " + rc.ToString());
            if (rc != pxcmStatus.PXCM_STATUS_NO_ERROR)
                Environment.Exit(-1);

            // getting touchless controller
            ptc = psm.QueryTouchlessController();
            if (ptc == null)
                Environment.Exit(-1);
            ptc.SubscribeEvent(new PXCMTouchlessController.OnFiredUXEventDelegate(OnTouchlessControllerUXEvent));
            
        }

        // on closing
        private void StopRealSense()
        {
            Console.WriteLine("Disposing SenseManager and Touchless Controller");
            ptc.Dispose();
            psm.Close();
            psm.Dispose();
           
        }

        private void UpdateConfiguration()
        {
            pxcmStatus rc;
            PXCMTouchlessController.ProfileInfo pInfo;

            rc = ptc.QueryProfile(out pInfo);
            Console.WriteLine("Querying Profile: " + rc.ToString());
            if (rc != pxcmStatus.PXCM_STATUS_NO_ERROR)
                Environment.Exit(-1);

            pInfo.config = PXCMTouchlessController.ProfileInfo.Configuration.Configuration_Scroll_Vertically;

            rc = ptc.SetProfile(pInfo);
            Console.WriteLine("Setting Profile: " + rc.ToString());
        }

        private void StartFrameLoop()
        {
            psm.StreamFrames(false);
        }

        private void OnTouchlessControllerUXEvent(PXCMTouchlessController.UXEventData data)
        {
            if (this.Dispatcher.CheckAccess())
            {
                switch (data.type)
                {
                    case PXCMTouchlessController.UXEventData.UXEventType.UXEvent_CursorVisible:
                        {
                            Console.WriteLine("Cursor Visible");
                            DisplayArea.Cursor = Cursors.None;
                        }
                        break;
                    case PXCMTouchlessController.UXEventData.UXEventType.UXEvent_CursorNotVisible:
                        {
                            Console.WriteLine("Cursor Not Visible");
                            DisplayArea.Cursor = null;
                        }
                        break;
                    case PXCMTouchlessController.UXEventData.UXEventType.UXEvent_Select:
                        {
                            Console.WriteLine("Select");
                            MouseInjection.ClickLeftMouseButton();
                        }
                        break;
                    case PXCMTouchlessController.UXEventData.UXEventType.UXEvent_StartScroll:
                        {
                            Console.WriteLine("Start Scroll");
                            initialScrollPoint = data.position.y;
                            initialScrollOffest = myListscrollViwer.VerticalOffset;
                        }
                        break;
                    case PXCMTouchlessController.UXEventData.UXEventType.UXEvent_CursorMove:
                        {
                            Point point = new Point();
                            point.X = Math.Max(Math.Min(0.9F, data.position.x), 0.1F);
                            point.Y = Math.Max(Math.Min(0.9F, data.position.y), 0.1F);

                            Point myListBoxPosition = DisplayArea.PointToScreen(new Point(0d, 0d));

                            int mouseX = (int)(myListBoxPosition.X + point.X * DisplayArea.ActualWidth);
                            int mouseY = (int)(myListBoxPosition.Y + point.Y * DisplayArea.ActualHeight);

                            MouseInjection.SetCursorPos(mouseX, mouseY);
                        }
                        break;
                    case PXCMTouchlessController.UXEventData.UXEventType.UXEvent_Scroll:
                        {
                            myListscrollViwer.ScrollToVerticalOffset(initialScrollOffest + (data.position.y - initialScrollPoint) * scrollSensitivity);
                        }
                        break;
                }
            }
            else
            {
                this.Dispatcher.Invoke(new Action(() => OnTouchlessControllerUXEvent(data)));
            }
        }

        public static DependencyObject GetScrollViewer(DependencyObject o)
        {
            // Return the DependencyObject if it is a ScrollViewer
            if (o is ScrollViewer)
            { return o; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;

        }
    }
}
*/
