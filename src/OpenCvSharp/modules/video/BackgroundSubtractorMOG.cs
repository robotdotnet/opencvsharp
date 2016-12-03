﻿using System;

namespace OpenCvSharp.CPlusPlus
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// The Base Class for Background/Foreground Segmentation.
    /// The class is only used to define the common interface for
    /// the whole family of background/foreground segmentation algorithms.
    /// </summary>
    public class BackgroundSubtractorMOG : BackgroundSubtractor
    {
        private Ptr<BackgroundSubtractorMOG> objectPtr;
        private bool disposed = false;

        #region Init & Disposal
        /// <summary>
        /// the default constructor
        /// </summary>
        public BackgroundSubtractorMOG()
        {
            IntPtr po = NativeMethods.video_BackgroundSubtractorMOG_new1();
            if (po == IntPtr.Zero)
                throw new OpenCvSharpException("Failed to create BackgroundSubtractorMOG");
            objectPtr = new Ptr<BackgroundSubtractorMOG>(po);
            ptr = objectPtr.Obj;
        }
        /// <summary>
        /// the full constructor that takes the length of the history, the number of gaussian mixtures, the background ratio parameter and the noise strength
        /// </summary>
        /// <param name="history"></param>
        /// <param name="nmixtures"></param>
        /// <param name="backgroundRatio"></param>
        /// <param name="noiseSigma"></param>
        public BackgroundSubtractorMOG(int history, int nmixtures, double backgroundRatio, double noiseSigma=0)
        {
            IntPtr po = NativeMethods.video_BackgroundSubtractorMOG_new2(history, nmixtures, backgroundRatio, noiseSigma);
            if (po == IntPtr.Zero)
                throw new OpenCvSharpException("Failed to create BackgroundSubtractorMOG");
            objectPtr = new Ptr<BackgroundSubtractorMOG>(po);
            ptr = objectPtr.Obj;
        }

        internal BackgroundSubtractorMOG(Ptr<BackgroundSubtractorMOG> objectPtr, IntPtr ptr)
        {
            this.objectPtr = objectPtr;
            this.ptr = ptr;
        }

        /// <summary>
        /// Creates instance from cv::Ptr&lt;T&gt; .
        /// ptr is disposed when the wrapper disposes. 
        /// </summary>
        /// <param name="ptr"></param>
        internal static new BackgroundSubtractorMOG FromPtr(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new OpenCvSharpException("Invalid BackgroundSubtractorMOG pointer");

            var ptrObj = new Ptr<BackgroundSubtractorMOG>(ptr);
            var obj = new BackgroundSubtractorMOG(ptrObj, ptrObj.Obj);
            return obj;
        }
        /// <summary>
        /// Creates instance from raw T*
        /// </summary>
        /// <param name="ptr"></param>
        internal static new BackgroundSubtractorMOG FromRawPtr(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new OpenCvSharpException("Invalid BackgroundSubtractorMOG pointer");
            var obj = new BackgroundSubtractorMOG(null, ptr);
            return obj;
        }

#if LANG_JP
    /// <summary>
    /// リソースの解放
    /// </summary>
    /// <param name="disposing">
    /// trueの場合は、このメソッドがユーザコードから直接が呼ばれたことを示す。マネージ・アンマネージ双方のリソースが解放される。
    /// falseの場合は、このメソッドはランタイムからファイナライザによって呼ばれ、もうほかのオブジェクトから参照されていないことを示す。アンマネージリソースのみ解放される。
    ///</param>
#else
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        /// If false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
        /// </param>
#endif
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    if (disposing)
                    {                        
                    }
                    if (IsEnabledDispose)
                    {
                        if (objectPtr != null)
                        {
                            objectPtr.Dispose();
                        }
                        else
                        {
                            NativeMethods.video_BackgroundSubtractorMOG_delete(ptr);
                        }
                        objectPtr = null;
                        ptr = IntPtr.Zero;
                    }
                    disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
        #endregion

        /// <summary>
        /// the update operator
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fgmask"></param>
        /// <param name="learningRate"></param>
        public override void Run(InputArray image, OutputArray fgmask, double learningRate = 0)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (fgmask == null)
                throw new ArgumentNullException("fgmask");
            image.ThrowIfDisposed();
            fgmask.ThrowIfNotReady();
            NativeMethods.video_BackgroundSubtractorMOG_operator(ptr, image.CvPtr, fgmask.CvPtr, learningRate);
            fgmask.Fix();
        }
    
        /// <summary>
        /// re-initiaization method
        /// </summary>
        /// <param name="frameSize"></param>
        /// <param name="frameType"></param>
        public virtual void Initialize(Size frameSize, int frameType)
        {
            NativeMethods.video_BackgroundSubtractorMOG_initialize(ptr, frameSize, frameType);
        }
        
        /// <summary>
        /// Pointer to algorithm information (cv::AlgorithmInfo*)
        /// </summary>
        /// <returns></returns>
        public override IntPtr InfoPtr
        {
            get { return NativeMethods.video_BackgroundSubtractorMOG_info(ptr); }
        }
    }
}