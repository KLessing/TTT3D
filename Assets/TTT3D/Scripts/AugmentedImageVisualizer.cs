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
        public GameObject MediumCross1;
        public GameObject LargeCross1;

        public GameObject SmallCross2;
        public GameObject MediumCross2;
        public GameObject LargeCross2;

        public GameObject SmallCircle1;
        public GameObject MediumCircle1;
        public GameObject LargeCircle1;

        public GameObject SmallCircle2;
        public GameObject MediumCircle2;
        public GameObject LargeCircle2;

        private enum Field { TopLeft, TopMiddle, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomMiddle, BottomRight };

        // Returns a position Vector for the Field on the Gamefield img
        private Vector3 GetPositionVector(Field field, float centerX, float centerY)
        {
            switch (field)
            {
                case Field.TopLeft: return (centerX * Vector3.left) + (centerY * Vector3.forward);
                case Field.TopMiddle: return (centerY * Vector3.forward);
                case Field.TopRight: return (centerX * Vector3.right) + (centerY * Vector3.forward);

                case Field.MiddleLeft: return (centerX * Vector3.left);
                case Field.Middle: return Vector3.zero;
                case Field.MiddleRight: return (centerX * Vector3.right);

                case Field.BottomLeft: return (centerX * Vector3.left) + (centerY * Vector3.back);
                case Field.BottomMiddle: return (centerY * Vector3.back);
                case Field.BottomRight: return (centerX * Vector3.right) + (centerY * Vector3.back);

                default: return Vector3.zero;
            }
        }


        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Always deactivate everything first
            // Otherwise they are visible even when not used
            SmallCross1.SetActive(false);
            MediumCross1.SetActive(false);
            LargeCross1.SetActive(false);

            SmallCross2.SetActive(false);
            MediumCross2.SetActive(false);
            LargeCross2.SetActive(false);

            SmallCircle1.SetActive(false);
            MediumCircle1.SetActive(false);
            LargeCircle1.SetActive(false);

            SmallCircle2.SetActive(false);
            MediumCircle2.SetActive(false);
            LargeCircle2.SetActive(false);

            // Show Tokens while tracking the image
            if (Image != null && Image.TrackingState == TrackingState.Tracking)
            {
                // Calcuate the center of the indivdual Fields on the Gamefield                
                float centerX = Image.ExtentX / 3;
                float centerY = Image.ExtentZ / 3;

                SmallCross1.transform.localPosition = GetPositionVector(Field.TopMiddle, centerX, centerY);
                MediumCross1.transform.localPosition = GetPositionVector(Field.Middle, centerX, centerY);
                LargeCross1.transform.localPosition = GetPositionVector(Field.BottomMiddle, centerX, centerY);

                SmallCircle1.transform.localPosition = GetPositionVector(Field.BottomLeft, centerX, centerY);
                MediumCircle1.transform.localPosition = GetPositionVector(Field.MiddleLeft, centerX, centerY);

                SmallCircle2.transform.localPosition = GetPositionVector(Field.TopLeft, centerX, centerY);
                MediumCircle2.transform.localPosition = GetPositionVector(Field.BottomRight, centerX, centerY);


                SmallCross1.SetActive(true);
                MediumCross1.SetActive(true);
                LargeCross1.SetActive(true);

                SmallCircle1.SetActive(true);
                MediumCircle1.SetActive(true);

                SmallCircle2.SetActive(true);
                MediumCircle2.SetActive(true);
            }
        }

    }
}
