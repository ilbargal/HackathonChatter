using System;
using System.Threading;

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

        private RealSenseHandler()
	    {
            // Instantiate and initialize the SenseManager
            _senseManager = PXCMSenseManager.CreateInstance();
            _senseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 640, 480, 30);
            _senseManager.EnableHand();
            _senseManager.Init();

            // Configure the Hand Module
            _hand = _senseManager.QueryHand();
            _handConfig = _hand.CreateActiveConfiguration();
            _handConfig.EnableGesture("wave");
            _handConfig.EnableAllAlerts();
            _handConfig.ApplyChanges();

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

        private void ProcessFrame()
        {
            // Start AcquireFrame/ReleaseFrame loop
            while (_senseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                PXCMCapture.Sample sample = _senseManager.QuerySample();

                // Retrieve gesture data
                _hand = _senseManager.QueryHand();

                if (_hand != null)
                {
                    // Retrieve the most recent processed data
                    _handData = _hand.CreateOutput();
                    _handData.Update();
                    _handData.IsGestureFired("wave", out _gestureData);
                }

                // Release the frame
                if (_handData != null) _handData.Dispose();
                    _senseManager.ReleaseFrame();
            }
        }

        public void Dispose()
        {
            if (_handData != null) _handData.Dispose();
                _handConfig.Dispose();
            _senseManager.Dispose();
        }
    }
}
