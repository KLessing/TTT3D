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

        public GameObject SmallCross1;
        //public GameObject MediumCross1;
        //public GameObject LargeCross1;

        //public GameObject SmallCross2;
        //public GameObject MediumCross2;
        //public GameObject LargeCross2;

        //public GameObject SmallCircle1;
        //public GameObject MediumCircle1;
        //public GameObject LargeCircle1;

        //public GameObject SmallCircle2;
        //public GameObject MediumCircle2;
        //public GameObject LargeCircle2;

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Always deactivate everything first
            // Otherwise they are visible even when not used
            SmallCross1.SetActive(false);
            //MediumCross1.SetActive(false);
            //LargeCross1.SetActive(false);

            //SmallCross2.SetActive(false);
            //MediumCross2.SetActive(false);
            //LargeCross2.SetActive(false);

            //SmallCircle1.SetActive(false);
            //MediumCircle1.SetActive(false);
            //LargeCircle1.SetActive(false);

            //SmallCircle2.SetActive(false);
            //MediumCircle2.SetActive(false);
            //LargeCircle2.SetActive(false);

            //if (Image == null || Image.TrackingState != TrackingState.Tracking)
            //{
            //    SmallCross1.SetActive(false);
            //    MediumCross1.SetActive(false);
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


            SmallCross1.transform.localPosition = topMiddle;
            //MediumCross1.transform.localPosition = middle;
            //LargeCross1.transform.localPosition = bottomMiddle;

            //SmallCircle1.transform.localPosition = bottomLeft;
            //MediumCircle1.transform.localPosition = topRight;

            //SmallCircle2.transform.localPosition = topLeft;
            //MediumCircle2.transform.localPosition = bottomRight;            


            SmallCross1.SetActive(true);
            //MediumCross1.SetActive(true);
            //LargeCross1.SetActive(true);

            //SmallCircle1.SetActive(true);
            //MediumCircle1.SetActive(true);

            //SmallCircle2.SetActive(true);
            //MediumCircle2.SetActive(true);
        }
    }
}
