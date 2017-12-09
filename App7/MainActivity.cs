using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Content;
using System;
using Android.Hardware.Camera2.Params;
using Android.Util;
using Android.Runtime;
using Android.Content.PM;
using Android;
using System.Collections.Generic;

namespace App7
{
    [Activity(Label = "App7", MainLauncher = true)]
    public class MainActivity : Activity, TextureView.ISurfaceTextureListener 
    {

        public HandlerThread Thread;
        public static Handler Handler;

        public class newclass : CameraDevice.StateCallback 
        {
            public Size imagedimension;
            public CameraDevice cameradevice;
            public TextureView tview;
            public CaptureRequest.Builder Builder;
            public List<Surface> outputs = new List<Surface>();
            public nclass capturecallback = new nclass();
            
            public override void OnDisconnected(CameraDevice camera)
            {
                camera.Close();//throw new NotImplementedException();
            }

            public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
            {
                camera.Close();
                camera = null; //throw new NotImplementedException();
            }

            public override void OnOpened(CameraDevice camera)
            {
                cameradevice = camera;
                createpreview();
            }

            public void createpreview()
            {
                //Toast.MakeText(Application.Context, "Iam there", ToastLength.Long).Show();
                    SurfaceTexture texture = tview.SurfaceTexture;
                    texture.SetDefaultBufferSize(imagedimension.Width, imagedimension.Height);
                    Surface surface = new Surface(texture);
                    Builder = cameradevice.CreateCaptureRequest(CameraTemplate.Preview);
                    Builder.AddTarget(surface);
                    outputs.Add(surface);
                    cameradevice.CreateCaptureSession(outputs, capturecallback, null);
                
                //throw new NotImplementedException();
            }
        }

        public static newclass statecallback;
       
        public class nclass : CameraCaptureSession.StateCallback
        {
            public CameraCaptureSession CameraCaptureSessions;

            public override void OnConfigured(CameraCaptureSession session)
            {
                if(statecallback.cameradevice == null)
                {
                    return;
                }
                CameraCaptureSessions = session;
                UpdatePreview();
                //throw new NotImplementedException();
            }

            private void UpdatePreview()
            {
                if (statecallback.cameradevice == null) return;
                //Toast.MakeText(Application.Context, "Error", ToastLength.Long).Show(); 
                //statecallback.Builder.Set(CaptureRequest.ControlMode,statecallback.Builder.Get(CaptureRequest.ControlMode));
               
                    CameraCaptureSessions.SetRepeatingRequest(statecallback.Builder.Build(), null, Handler);
                
               
                //throw new NotImplementedException();
            }

            public override void OnConfigureFailed(CameraCaptureSession session)
            {
                Toast.MakeText(Application.Context, "Changed", ToastLength.Long).Show();
                //throw new NotImplementedException();
            }
        }


        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
          //  Toast.MakeText(this, "ok", ToastLength.Long).Show();
            OpenCamera();//throw new System.NotImplementedException();
            //Toast.MakeText(this, "ok", ToastLength.Long).Show();
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return true;
          
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            throw new System.NotImplementedException();
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
           // Toast.MakeText(this, "ok", ToastLength.Long).Show();
            //throw new System.NotImplementedException();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button button = (Button)FindViewById(Resource.Id.button1);
             statecallback = new newclass();
             statecallback.tview = (TextureView)FindViewById(Resource.Id.surfaceView1);
             statecallback.tview.SurfaceTextureListener = this;

        }

        protected override void OnResume()
        {
            base.OnResume();
            Startbackgroundthread();

            if (statecallback.tview.IsAvailable)
            {
                statecallback.capturecallback = new nclass();
                statecallback.Builder = null;
                statecallback.outputs = new List<Surface>();
                OpenCamera();
               // statecallback = new newclass();
                //Toast.MakeText(this, "hey there", ToastLength.Long).Show();
                //statecallback.tview = (TextureView)FindViewById(Resource.Id.surfaceView1);
                //statecallback.tview.SurfaceTextureListener = this;
                //OpenCamera();
                
            }
            else
            {
                statecallback.tview.SurfaceTextureListener = this;
               // Toast.MakeText(this, "hi", ToastLength.Long).Show();

            }

           
        }

        protected override void OnPause()
        {   
           // if(statecallback.capturecallback.CameraCaptureSessions!=null)
            //statecallback.capturecallback.CameraCaptureSessions.Close();
            //statecallback.capturecallback.CameraCaptureSessions = null;
            //if(statecallback.cameradevice !=null)
            statecallback.cameradevice.Close();

            //statecallback.cameradevice=null;
            Stopbackgroundthread();
            base.OnPause();
             //backgroundthread();
        }

        private void Stopbackgroundthread()
        {
            Thread.QuitSafely();
            try
            {
                Thread.Join();
                Thread = null;
                Handler = null;
            }
            catch
            {

            }
            //throw new NotImplementedException();
        }

        private void Startbackgroundthread()
        {
            Thread = new HandlerThread("Camera Background");
            Thread.Start();
            Handler = new Handler(Thread.Looper);
            //throw new NotImplementedException();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if(requestCode == 200)
            {
                if(grantResults[0] != Permission.Granted)
                {
                    Toast.MakeText(this, "hello", ToastLength.Long).Show();
                    Finish();
                }

            }
            
        }


        private void OpenCamera()
        {
            CameraManager manager = (CameraManager)GetSystemService(Context.CameraService);
          
                String CameraId = manager.GetCameraIdList()[0];
                CameraCharacteristics cameraCharacteristics = manager.GetCameraCharacteristics(CameraId);
                StreamConfigurationMap map = (StreamConfigurationMap) 
                    cameraCharacteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                statecallback.imagedimension = map.GetOutputSizes(256)[0];
                manager.OpenCamera("0", statecallback, Handler);

            // Toast.MakeText(this, "ok", ToastLength.Long).Show();



            //throw new NotImplementedException();
        }
    }



}

