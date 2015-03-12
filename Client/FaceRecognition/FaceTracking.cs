using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace videochatsample.FaceRecognition
{
    internal class FaceTracking
    {
        private readonly LoginForm m_form;
        private int id;
        private FpsTimer m_timer;
        private bool m_wasConnected;

        public FaceTracking(LoginForm form)
        {
            m_form = form;
            ID = -1;
        }

        public static int ID { get; set; }

        private void DisplayDeviceConnection(bool isConnected)
        {
            if (isConnected && !m_wasConnected) m_form.UpdateStatus("Device Reconnected", LoginForm.Label.StatusLabel);
            else if (!isConnected && m_wasConnected)
                m_form.UpdateStatus("Device Disconnected", LoginForm.Label.StatusLabel);
            m_wasConnected = isConnected;
        }

        private void DisplayPicture(PXCMImage image)
        {
            PXCMImage.ImageData data;
            if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out data) <
                pxcmStatus.PXCM_STATUS_NO_ERROR) return;
            m_form.DrawBitmap(data.ToBitmap(0, image.info.width, image.info.height));
            m_timer.Tick("");
            image.ReleaseAccess(data);
        }

        private void CheckForDepthStream(PXCMCapture.Device.StreamProfileSet profiles, PXCMFaceModule faceModule)
        {
            var faceConfiguration = faceModule.CreateActiveConfiguration();
            if (faceConfiguration == null)
            {
                Debug.Assert(faceConfiguration != null);
                return;
            }

            var trackingMode = faceConfiguration.GetTrackingMode();
            faceConfiguration.Dispose();

            if (trackingMode != PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH) return;
            if (profiles.depth.imageInfo.format == PXCMImage.PixelFormat.PIXEL_FORMAT_DEPTH) return;
            PXCMCapture.DeviceInfo dinfo;
            m_form.Devices.TryGetValue(m_form.GetCheckedDevice(), out dinfo);

            if (dinfo != null)
                MessageBox.Show(
                    String.Format("Depth stream is not supported for device: {0}. \nUsing 2D tracking", dinfo.name),
                    @"Face Tracking",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FaceAlertHandler(PXCMFaceData.AlertData alert)
        {
            m_form.UpdateStatus(alert.label.ToString(), LoginForm.Label.StatusLabel);
        }

        public void SimplePipeline()
        {
            var pp = m_form.Session.CreateSenseManager();

            if (pp == null)
            {
                throw new Exception("PXCMSenseManager null");
            }

            // Set Resolution
            var selectedRes = m_form.GetCheckedColorResolution();

            if (selectedRes != null && !m_form.GetPlaybackState())
            {
                // activate filter only live/record mode , no need in playback mode
                var set = new PXCMCapture.Device.StreamProfileSet
                {
                    color =
                    {
                        frameRate = selectedRes.Item2,
                        imageInfo =
                        {
                            format = selectedRes.Item1.format,
                            height = selectedRes.Item1.height,
                            width = selectedRes.Item1.width
                        }
                    }
                };
                pp.captureManager.FilterByStreamProfiles(set);
            }

            // Set Source & Landmark Profile Index 
            if (m_form.GetPlaybackState())
            {
                //pp.captureManager.FilterByStreamProfiles(null);
                pp.captureManager.SetFileName(m_form.GetFileName(), false);
                pp.captureManager.SetRealtime(false);
            }
            else if (m_form.GetRecordState())
            {
                pp.captureManager.SetFileName(m_form.GetFileName(), true);
            }

            // Set Module            
            pp.EnableFace();
            var faceModule = pp.QueryFace();
            if (faceModule == null)
            {
                Debug.Assert(faceModule != null);
                return;
            }

            var moduleConfiguration = faceModule.CreateActiveConfiguration();

            if (moduleConfiguration == null)
            {
                Debug.Assert(moduleConfiguration != null);
                return;
            }

            var mode = m_form.GetCheckedProfile().Contains("3D")
                ? PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH
                : PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR;

            moduleConfiguration.SetTrackingMode(mode);

            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;

            moduleConfiguration.detection.maxTrackedFaces = m_form.NumDetection;
            moduleConfiguration.landmarks.maxTrackedFaces = m_form.NumLandmarks;
            moduleConfiguration.pose.maxTrackedFaces = m_form.NumPose;

            var econfiguration = moduleConfiguration.QueryExpressions();
            if (econfiguration == null)
            {
                throw new Exception("ExpressionsConfiguration null");
            }
            econfiguration.properties.maxTrackedFaces = m_form.NumExpressions;

            econfiguration.EnableAllExpressions();
            moduleConfiguration.detection.isEnabled = m_form.IsDetectionEnabled();
            moduleConfiguration.landmarks.isEnabled = m_form.IsLandmarksEnabled();
            moduleConfiguration.pose.isEnabled = m_form.IsPoseEnabled();
            if (m_form.IsExpressionsEnabled())
            {
                econfiguration.Enable();
            }

            var pulseConfiguration = moduleConfiguration.QueryPulse();
            if (pulseConfiguration == null)
            {
                throw new Exception("pulseConfiguration null");
            }

            pulseConfiguration.properties.maxTrackedFaces = m_form.NumPulse;
            if (m_form.IsPulseEnabled())
            {
                if (!m_form.GetPlaybackState())
                {
                    // Pulse requirement is 720p
                    pp.captureManager.FilterByStreamProfiles(null);
                    pp.captureManager.FilterByStreamProfiles(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 1280, 720, 0);
                }
                pulseConfiguration.Enable();
            }

            var qrecognition = moduleConfiguration.QueryRecognition();
            //qrecognition.SetDatabaseBuffer
            if (qrecognition == null)
            {
                throw new Exception("PXCMFaceConfiguration.RecognitionConfiguration null");
            }
            if (m_form.IsRecognitionChecked())
            {
                qrecognition.Enable();
            }

            moduleConfiguration.EnableAllAlerts();
            moduleConfiguration.SubscribeAlert(FaceAlertHandler);

            var applyChangesStatus = moduleConfiguration.ApplyChanges();
            //String [] files = getAllFiles("Users");
            //foreach (String file in files)
            //{

            //try
            //{
            //    byte[] data = LoadData("Users.dat");
            //    qrecognition.SetDatabaseBuffer(data);
            //}
            //catch (Exception)
            //{
            //}
            //}
            var rcfg = moduleConfiguration.QueryRecognition();
            var desc = new PXCMFaceConfiguration.RecognitionConfiguration.RecognitionStorageDesc();
            desc.maxUsers = 10;
            try
            {
                var data = LoadData("Users.dat");
                rcfg.SetDatabaseBuffer(data);
            }
            catch (Exception)
            {
                rcfg.CreateStorage("MyDB", out desc);
            }
            rcfg.UseStorage("MyDB");
            rcfg.SetRegistrationMode(
                PXCMFaceConfiguration.RecognitionConfiguration.RecognitionRegistrationMode.REGISTRATION_MODE_CONTINUOUS);
            moduleConfiguration.ApplyChanges();


            //config util ow

            m_form.UpdateStatus("Init Started", LoginForm.Label.StatusLabel);

            if (applyChangesStatus < pxcmStatus.PXCM_STATUS_NO_ERROR || pp.Init() < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                m_form.UpdateStatus("Init Failed", LoginForm.Label.StatusLabel);
            }
            else
            {
                using (var moduleOutput = faceModule.CreateOutput())
                {
                    Debug.Assert(moduleOutput != null);
                    PXCMCapture.Device.StreamProfileSet profiles;

                    var cmanager = pp.QueryCaptureManager();
                    if (cmanager == null)
                    {
                        throw new Exception("capture manager null");
                    }
                    var device = cmanager.QueryDevice();

                    if (device == null)
                    {
                        throw new Exception("device null");
                    }

                    device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, 0, out profiles);
                    CheckForDepthStream(profiles, faceModule);

                    m_form.UpdateStatus("Streaming", LoginForm.Label.StatusLabel);
                    m_timer = new FpsTimer(m_form);

                    while (!m_form.Stopped)
                    {
                        if (pp.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                        var isConnected = pp.IsConnected();
                        DisplayDeviceConnection(isConnected);
                        if (isConnected)
                        {
                            var sample = pp.QueryFaceSample();
                            if (sample == null)
                            {
                                pp.ReleaseFrame();
                                continue;
                            }
                            DisplayPicture(sample.color);

                            moduleOutput.Update();

                            if (moduleConfiguration.QueryRecognition().properties.isEnabled)
                                UpdateRecognition(moduleOutput);
                            var id = IdentifyFace(moduleOutput);
                            if (id != -1)
                            {
                                ID = id;
                                //m_form.MoveNextWindow(id);
                                //var frm = new StartConvForm(id);
                                //m_form.Invoke((Action)(() => m_form.Hide()));
                                //frm.ShowDialog();
                                m_form.Stopped = true;
                                break;
                            }
                            m_form.DrawGraphics(moduleOutput);
                            m_form.UpdatePanel();
                        }
                        pp.ReleaseFrame();
                    }
                }

                moduleConfiguration.UnsubscribeAlert(FaceAlertHandler);
                moduleConfiguration.ApplyChanges();
                m_form.UpdateStatus("Stopped", LoginForm.Label.StatusLabel);
            }
            moduleConfiguration.Dispose();
            pp.Close();
            pp.Dispose();
        }

        private int IdentifyFace(PXCMFaceData moduleOutput)
        {
            var userId = -1;
            for (var i = 0; i < moduleOutput.QueryNumberOfDetectedFaces(); i++)
            {
                var face = moduleOutput.QueryFaceByIndex(i);
                var qrecognition = face.QueryRecognition();
                if (qrecognition != null)
                {
                    userId = qrecognition.QueryUserID();
                    var recognitionText = userId == -1 ? "Not Registered" : String.Format("Registered ID: {0}", userId);
                    if (userId != -1)
                    {
                        break;
                    }
                }
            }
            return userId;
        }

        private String[] getAllFiles(string p)
        {
            return Directory.GetFiles(p);
        }

        private void UpdateRecognition(PXCMFaceData faceOutput)
        {
            //TODO: add null checks
            if (m_form.Register) RegisterUser(faceOutput);
            if (m_form.Unregister) UnregisterUser(faceOutput);
        }

        private void RegisterUser(PXCMFaceData faceOutput)
        {
            m_form.Register = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
                return;

            var qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }
            var rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }
            rdata.RegisterUser();

            var rmd = faceOutput.QueryRecognitionModule();
            var data = new byte[rmd.QueryDatabaseSize()];
            rmd.QueryDatabaseBuffer(data);
            SaveData("Users.dat", data);
            id++;
        }

        private void UnregisterUser(PXCMFaceData faceOutput)
        {
            m_form.Unregister = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
            {
                return;
            }

            var qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }

            var rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }

            if (!rdata.IsRegistered())
            {
                return;
            }
            rdata.UnregisterUser();
        }

        private void SaveData(String path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        private byte[] LoadData(String path)
        {
            return File.ReadAllBytes(path);
        }
    }
}