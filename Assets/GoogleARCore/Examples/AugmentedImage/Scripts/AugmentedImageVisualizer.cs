//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        public GameObject SmallCross;
        public GameObject MediumCross;
        public GameObject LargeCross;

        public GameObject SmallCircle;
        public GameObject MediumCircle;
        public GameObject LargeCircle;

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Always deactivate everything first
            // Otherwise they are visible even when not used
            SmallCross.SetActive(false);
            MediumCross.SetActive(false);
            LargeCross.SetActive(false);

            SmallCircle.SetActive(false);
            MediumCircle.SetActive(false);
            LargeCircle.SetActive(false);

            //if (Image == null || Image.TrackingState != TrackingState.Tracking)
            //{
            //    SmallCross.SetActive(false);
            //    MediumCross.SetActive(false);
            //    return;
            //}

            // Third width and height for middle of outer fields
            float thirdWidth = Image.ExtentX / 3;
            float thirdHeight = Image.ExtentZ / 3;

            // Position Vectors
            Vector3 topLeft = (thirdWidth * Vector3.left) + (thirdHeight * Vector3.forward);
            Vector3 topMiddle = (thirdHeight * Vector3.forward);
            Vector3 topRight = (thirdWidth * Vector3.right) + (thirdHeight * Vector3.forward);

            Vector3 middleLeft = (thirdWidth * Vector3.left);
            Vector3 middle = Vector3.zero;
            Vector3 middleRight = (thirdWidth * Vector3.right);

            Vector3 bottomLeft = (thirdWidth * Vector3.left) + (thirdHeight * Vector3.back);
            Vector3 bottomMiddle = (thirdHeight * Vector3.back);
            Vector3 bottomRight = (thirdWidth * Vector3.right) + (thirdHeight * Vector3.back);            


            SmallCross.transform.localPosition = topMiddle;
            MediumCross.transform.localPosition = middle;
            LargeCross.transform.localPosition = bottomMiddle;

            SmallCircle.transform.localPosition = bottomLeft;
            MediumCircle.transform.localPosition = topRight;


            SmallCross.SetActive(true);
            MediumCross.SetActive(true);
            LargeCross.SetActive(true);

            SmallCircle.SetActive(true);
            MediumCircle.SetActive(true);
        }
    }
}
