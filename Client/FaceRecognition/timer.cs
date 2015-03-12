using System.Runtime.InteropServices;
using DF_FaceTracking.cs;

namespace videochatsample.FaceRecognition
{
    class FpsTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long data);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long data);

        private LoginForm form;
        private long freq, last;
        private int fps;

        public FpsTimer(LoginForm mf)
        {
            form = mf;
            QueryPerformanceFrequency(out freq);
            fps = 0;
            QueryPerformanceCounter(out last);
        }

        public void Tick(string text)
        {
            long now;
            QueryPerformanceCounter(out now);
            fps++;
            if (now - last > freq) // update every second
            {
                last = now;
                form.UpdateStatus(text+" FPS=" + fps, LoginForm.Label.StatusLabel);
                fps = 0;
            }
        }
    }
}
