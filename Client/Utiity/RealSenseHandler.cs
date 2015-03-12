using System;
using System.Drawing;
using System.Threading;
using Client;
using videochatsample;

namespace ClientWPF.Utiity
{
    public class RealSenseHandler : IDisposable
    {
        #region Singleton

        private static RealSenseHandler _instance;

        public static RealSenseHandler Instace
        {
            get { return _instance ?? (_instance = new RealSenseHandler()); }
        }

        #endregion

        #region Events

        public event Action<PXCMHandData> WaveFired = data => { };
        public event Action<PXCMHandData> TapFired = data => { };
        public event Action<PXCMHandData> ThumbUpFired = data => { };
        public event Action<PXCMHandData> ThumbDownFired = data => { };
        public event Action<PXCMHandData> ZoomInFired = data => { };
        public event Action<PXCMHandData> SpreadFingersFired = data => { };
        public event Action<PXCMHandData> FistFired = data => { };
        public event Action<PXCMHandData> SwipeLeftFired = data => { };
        public event Action<PXCMHandData> SwipeRightFired = data => { };
        public event Action<PXCMHandData> VSignFired = data => { }; 
 
        #endregion

        private RealSenseHandler()
	    {
            // Instantiate and initialize the SenseManager
            _senseManager = PXCMSenseManager.CreateInstance();
            _senseManager.EnableHand();
            _senseManager.Init();

            // Configure the Hand Module
            _hand = _senseManager.QueryHand();
            _handConfig = _hand.CreateActiveConfiguration();
            _handConfig.EnableAllGestures();
            _handConfig.EnableAllAlerts();
            _handConfig.ApplyChanges();

            _handConfig.Dispose();
            // Start the worker thread
            _processingThread = new Thread(new ThreadStart(ProcessFrame));
	    }

        #region Data Members

        private readonly Thread _processingThread;
        private readonly PXCMSenseManager _senseManager;
        private PXCMHandModule _hand;
        private readonly PXCMHandConfiguration _handConfig;
        private PXCMHandData _handData;
        private PXCMHandData.GestureData _gestureData;

        #endregion

        public void Start()
        {
            _processingThread.Start();
        }

        public void Stop()
        {
            _processingThread.Abort();
        }

        private void ProcessFrame()
        {
            // Start AcquireFrame/ReleaseFrame loop
            while (_senseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                // Retrieve gesture data
                _hand = _senseManager.QueryHand();

                if (_hand != null)
                {
                    // Retrieve the most recent processed data
                    _handData = _hand.CreateOutput();
                    _handData.Update();
                    if (_handData.IsGestureFired("wave", out _gestureData))
                        WaveFired(_handData);
                    if (_handData.IsGestureFired("tap", out _gestureData))
                        TapFired(_handData);
                    if (_handData.IsGestureFired("thumb_up", out _gestureData))
                        ThumbUpFired(_handData);
                    if (_handData.IsGestureFired("zoom_in", out _gestureData))
                        ZoomInFired(_handData);
                    if (_handData.IsGestureFired("thumb_down", out _gestureData))
                        ThumbDownFired(_handData);
                    if (_handData.IsGestureFired("spreadfingers", out _gestureData))
                        SpreadFingersFired(_handData);
                    if (_handData.IsGestureFired("fist", out _gestureData))
                        FistFired(_handData);
                    if (_handData.IsGestureFired("swipe_left", out _gestureData))
                        SwipeLeftFired(_handData);
                    if (_handData.IsGestureFired("swipe_right", out _gestureData))
                        SwipeRightFired(_handData);
                    if (_handData.IsGestureFired("v_sign", out _gestureData))
                        VSignFired(_handData);
                    
                }

                // Release the frame
                if (_handData != null) _handData.Dispose();
                _senseManager.ReleaseFrame();
            }
        }

        public void Dispose()
        {
            if (_processingThread.IsAlive)
            {
                _processingThread.Abort();
                _processingThread.Join();
            }
            if (_handData != null) _handData.Dispose();
            _handConfig.Dispose();
            _hand.Dispose();
            _senseManager.Dispose();
        }
    }
}
