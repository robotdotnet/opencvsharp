﻿using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace OpenCvSharp
{
    static partial class NativeMethods
    {
        // ReSharper disable InconsistentNaming

        // BOWTrainer

        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWTrainer_add(IntPtr obj, IntPtr descriptors);

        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWTrainer_getDescriptors(IntPtr obj, IntPtr descriptors);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern int features2d_BOWTrainer_descriptorsCount(IntPtr obj);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWTrainer_clear(IntPtr obj);


        // BOWKMeansTrainer

        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr features2d_BOWKMeansTrainer_new(
            int clusterCount, TermCriteria termcrit, int attempts, int flags);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWKMeansTrainer_delete(IntPtr obj);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr features2d_BOWKMeansTrainer_cluster1(IntPtr obj);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr features2d_BOWKMeansTrainer_cluster2(IntPtr obj, IntPtr descriptors);


        // BOWImgDescriptorExtractor

        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr features2d_BOWImgDescriptorExtractor_new1(IntPtr dextractor, IntPtr dmatcher);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr features2d_BOWImgDescriptorExtractor_new2(IntPtr dmatcher);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWImgDescriptorExtractor_delete(IntPtr obj);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWImgDescriptorExtractor_setVocabulary(IntPtr obj, IntPtr vocabulary);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr features2d_BOWImgDescriptorExtractor_getVocabulary(IntPtr obj);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWImgDescriptorExtractor_compute11(
            IntPtr obj, IntPtr image, IntPtr keypoints, IntPtr imgDescriptor,
            IntPtr pointIdxsOfClusters, IntPtr descriptors);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWImgDescriptorExtractor_compute12(
            IntPtr obj, IntPtr keypointDescriptors,
            IntPtr imgDescriptor, IntPtr pointIdxsOfClusters);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern void features2d_BOWImgDescriptorExtractor_compute2(
            IntPtr obj, IntPtr image, IntPtr keypoints, IntPtr imgDescriptor);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern int features2d_BOWImgDescriptorExtractor_descriptorSize(IntPtr obj);
        [DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl)]
        public static extern int features2d_BOWImgDescriptorExtractor_descriptorType(IntPtr obj);
    }
}
